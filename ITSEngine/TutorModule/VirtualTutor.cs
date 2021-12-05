using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using ITS.DomainModule;
using ITS.MaterialModule;
using ITS.StudentModule;  

namespace ITS.TutorModule
{ 
    /// <summary>
    /// Tutoring Module is the pedagogical agent of the system
    /// and it executes the actual tutoring process.In logical sense
    /// this module lies in the centre of the system and by 
    /// communicating with the other modules. It provides adaptive 
    /// instructions to the students. It is rule-based, where the
    /// rules define the tutoring strategy.
    /// </summary>
    public class VirtualTutor
    {
        protected Student _student;
        protected string _course;
        protected Tutoring _tutoring;
        protected Dictionary<string, TeachingScheme> _teachingSchemeDict; 

        public TeachingState State
        {
            get { return _tutoring.State; }
        }

        public TeachingScheme CurrentScheme
        {
            get { return _tutoring.CurrentScheme; } 
        }  

        public LearningTopic CurrentTopic
        {
            get
            {
                return CurrentScheme.LearningTopic;
            }
        } 

        public string CurrentCourse
        {
            get { return _course; }
        } 

        public VirtualTutor(Student student)//构造器中订阅关系
        {
            _student = student;
            _student.ChooseCourseEvent += OnCourseChanged;
            _student.PresentedQuestionEvent += PresentAQuestion;
            _student.ObtainAnswerEvent += ObtainCorrectAnswer;
            _student.FeedbackEvent += Feedback;

            _teachingSchemeDict = new Dictionary<string, TeachingScheme>();
            _tutoring = new Tutoring(student); //
        } 

        public void OnCourseChanged(string course)
        {
            _course = course;
            _tutoring.OnCourseChanged(course); 
        } 
        
        public void Search(string inputStr, Action<bool, string> callback)
        {
            _tutoring.OnSearch(inputStr, callback);
        }

        public void PresentAQuestion(Action<bool,Question> callback)
        {
            _tutoring.PresentAQuestion(callback);
        }

        /// <summary>
        /// 将answer与当前TeachingScheme中的当前QAPair进行比对，
        /// 得到反馈信息
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="callback"></param>
        public void Feedback(string inputStr, Action<string,double> callback)
        {
            _tutoring.Feedback(inputStr, callback);
            _student.OnLearningTopicFinished(CurrentScheme);
        }  

        protected void ObtainCorrectAnswer(Action<string> callback)
        {
            if (CurrentScheme == null)
            {
                callback.Invoke("没有选定学习课程，系统无法给出正确答案！");
                return;
            }
            if (CurrentScheme.CurrentQA == null)
            {
                callback.Invoke("请点击‘提出问题’按钮，系统才能给出问题的答案！");
                return;
            }

            string[] ans = CurrentScheme.CurrentAnswer;
            string info = string.Empty;
            if (ans != null)
            {
                foreach (var str in ans)
                    info += str+"，";
            }
            else
                info += "没有获取到文本答案！";

            callback(info);
        } 

    }
}
