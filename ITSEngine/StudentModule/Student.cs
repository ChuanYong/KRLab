using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Threading;

using KRLab.BNet;
using KRLab.Core.FuzzyEngine;
using KRLab.Core;

using Utilities;

using ITS.DomainModule;
using ITS.MaterialModule;
using ITS.TutorModule;

namespace ITS.StudentModule
{
    public delegate void OnCourseChanged(string course);
    public delegate void OnStart();
    public delegate void OnPresentedQuestion(Action<bool, Question> callback);
    public delegate void OnObtainAnswer(Action<string>callback);
    public delegate void OnFeedbacked(string input, Action<string, double> callback);

    /// <summary>
    /// Student Module stores information about the student's
    /// performance state in terms of fuzzy variable, her 
    /// preferences and her learning history
    /// </summary>
    public class Student 
    {
        protected LearningTopicRecord _lastLTR;
        protected LearningHistory _history; 
        protected Ability _ability;
        protected UserLog _userLog;
        protected TeachingScheme _scheme;

        protected bool _hasBegun = false;

        public event OnCourseChanged ChooseCourseEvent;
        public event OnStart StartEvent;
        public event OnPresentedQuestion PresentedQuestionEvent;
        public event OnObtainAnswer ObtainAnswerEvent;
        public event OnFeedbacked FeedbackEvent; 
 
        public string Course
        {
            get;set;
        } 

        public Ability Ability
        {
            get { return _ability; }
            set { _ability = value; }
        }

        public UserLog UserLog
        {
            get { return _userLog; }
            set { _userLog = value; }
        }

        public LearningHistory LearningHistory
        {
            get { return _history; }
            set { _history = value; }
        }

        public LearningTopicRecord LastLTR
        {
            get { return _lastLTR; }
            set { _lastLTR = value; }
        }

        /// <summary>
        /// 判断是否开始学习
        /// </summary>
        public bool HasBegun
        {
            get { return _hasBegun; }
        }

        public Student() 
        {
            _userLog = new UserLog();
        }

        public void Start(Action<bool, Question> callback)
        {
            _userLog = LoadUserLog();
            if (_userLog ==null || _userLog.IsEmpty)
            {
                string str ="没有学习日志，请按F3键选择学习课程！";
                callback(false, new Question(str));
                ITSSpeech.Speak(str);
                return;
            }

            //根据日志记录，确定当前的学习课程
            LastLTR = GetLastTopicRecord();
            if (ChooseCourseEvent != null)
                ChooseCourseEvent.Invoke(LastLTR.Course);

            string txt = $"你于{LastLTR.Date}学习了<{LastLTR.Course}>课程。" +
                $"学习课题为：{LastLTR.LearningTopic.Topic}\n 点击'Shift+Return'按钮，智能系统将为你安排本次学习的最佳课题。";
            ITSSpeech.Speak(txt);
            callback(true, new Question(txt));
             
            Course = LastLTR.Course; 
            _history = LoadXMLLearningHistory(Course); 
        }


        /// <summary>
        /// 获取日志中最新的学习记录
        /// </summary>
        /// <returns></returns>
        protected LearningTopicRecord GetLastTopicRecord()
        {
            if (_userLog == null)
                return null;

            return UserLog.GetLastTopicRecord();
        } 

        public bool HasLearningHistory
        {
            get { return _history != null; }
        } 

        /// <summary>
        /// 学生被问问题，由虚拟教师提出该问题。
        /// </summary>
        /// <param name="callback"></param>
        public void PresentedAQuestion(Action<bool,Question>callback)
        {
            if(PresentedQuestionEvent!=null)
                PresentedQuestionEvent.Invoke(callback); //直接事件.Invoke
        }

        public void Search(string inputStr,Action<bool,string>callback)
        {
            //_tutor.Search(inputStr, callback);
        }

        public void ObtainCorrects(Action<string> callback)
        { 
            if(ObtainAnswerEvent!=null)
                ObtainAnswerEvent.Invoke(callback); 
        }

        /// <summary>
        /// 选择学习课程时产生相应的用户日志
        /// </summary>
        /// <param name="course"></param>
        /// <param name="callback"></param>
        public void ChangeLearningCourse(string course,Action<bool,Question>callback)
        {
            if (!HasBegun)///如果学习还没有开始，就改变学习课程，则没有数据需要保存
            {
                callback(false, new Question($"选择的学习课程为：{course}")); 
                if (ChooseCourseEvent != null)
                    ChooseCourseEvent.Invoke(course);
                return;
            }

            if(Course!=course)//
            {
                Course = course;

                SaveXMLLearningHistory();
                SaveUserLog();

                if (ChooseCourseEvent != null)
                    ChooseCourseEvent.Invoke(course);

                if (_userLog == null || _userLog.GetRecord(course) == null)
                {
                    string info = $"你选择的学习课程<{course}>没有相关学习记录！点击‘Shift+Return'键开始学习’";
                    callback.Invoke(false, new Question(info));
                    return;
                }
                _history = LoadXMLLearningHistory(Course);
            }
            else
            {
                callback.Invoke(false, new Question("选择的课程与当前学习课程相同！"));
            }
        } 

        /// <summary>
        /// 当形成一个教案，并完成了其中一个问题的学习时
        /// </summary>
        /// <param name="scheme"></param>
        public void OnLearningTopicFinished(TeachingScheme scheme)
        {
            if(_scheme!=scheme)
                _scheme = scheme;

            _hasBegun = true;

            SaveData();
        }

        protected void SaveData()
        {
            ///当学生没有这门课程的学习历史时
            if (_history == null)
                _history = new LearningHistory();

            if (_userLog == null)
                _userLog = new UserLog();

            Dictionary<KnowledgeTopic, double> results = _scheme.DeptTopicResultDict;

            ///保存学习课题中的关联知识点的学习结果
            foreach (var tr in results)
            {
                LearningResult result = new LearningResult(tr.Value);
                _history.AddKnowledgeTopicHistory(tr.Key.Topic, result);
            }

            LearningTopicRecord ltr = new LearningTopicRecord(_scheme.LearningTopic,
                _scheme.LearningTopicResult);

            LastLTR = ltr;

            //在UserLog中只是记录学习课题的学习，不记录学习课题关联的知识点
            //的学习。 
            _userLog.AddTopic(LastLTR); 

            _history.AddLearningTopicHistory(LastLTR); 

        } 
        
        /// <summary>
        /// 当Tutor评估完一个学习课题时调用。
        /// </summary>
        ///<param name="inputStr"></param>
        ///<param name="callback"></param>
        public void Feedbacked(string inputStr, Action<string, double> callback)
        { 
            if(FeedbackEvent!=null)
            {
                FeedbackEvent.Invoke(inputStr, (info, score) => {

                    callback(info, score);
                });
            }             
        }  

        public void Exit()
        {
            if(_scheme!=null && _userLog!=null && _history!=null)///表示已经开始学习
            {
                SaveUserLog();
                SaveXMLLearningHistory();
            }
        }

        protected void SaveUserLog()
        {
            FileIO.SaveMemoryToFile(FileManager.DataPath + @"\UserLog", _userLog);
        }

        protected UserLog LoadUserLog()
        {
            string file = FileManager.DataPath + @"\UserLog";
            if (!File.Exists(file))
                return null;
            UserLog ul = (UserLog)FileIO.ReadFileToMemory(file);
            return ul;
        }

        /// <summary>
        /// 保存学生学习相关的所有数据到XML文件
        /// </summary>
        /// <param name="fileName"></param>
        protected void SaveXMLLearningHistory()
        {

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("学习历史");
            doc.AppendChild(root);

            _history.SerializeLearningTopic(root);
            _history.SerializeKnowledgeTopic(root);

            string filePath = FileManager.LearningHistoryPath + @"\" + Course;

            try
            {
                doc.Save(filePath + ".xml");
            }
            catch (Exception e)
            {
                throw new IOException("保存学习历史失败！",e);
            }

        }

        
        /// <summary>
        /// uh是学生上一次学习的记录，LearningHistory是详细的
        /// 一门课程的学习记录。
        /// 参考exp项目中的FVCTutoring文件，里面有很多文件的读写操作
        /// </summary>
        /// <param name = "uh" ></ param >
        /// < returns ></ returns >
        protected LearningHistory LoadXMLLearningHistory(string course)
        {
            string filePath = FileManager.GetLearningHistoryFilePath(course);
            if (!File.Exists(filePath))
                return null;

            LearningHistory history = new LearningHistory(); 

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filePath);
            }
            catch (Exception e)
            {
                throw new InvalidDataException("导入失败", e);
            }
            XmlElement root = doc["学习历史"];
            try
            {
                history.Deserialize(root);
            }
            catch (Exception e)
            {
                throw new InvalidDataException(e.ToString());
            }
            return history;
        }

    }
}
