using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; 

using ITS.DomainModule;
using ITS.StudentModule;
using ITS.MaterialModule;
using KRLab.Core;
using Utilities;

namespace ITS.TutorModule
{
    /*************************************************************************************
     * 虚拟教师的教学过程主要由这个类完成，其基本过程主要在PresentAQuestion()
     * 中完成：
     *（1）【实例化Tutoring】----->【State=Pre】
     *（2）【启动】--->【调用Start()函数】--->【_scheme=null】-->【_record=null】
     *      --->【确定辅导学生】--->【创建课程工厂CourseFactory】------>【State=Prepared】
     *（3）【向学生提出问题】--->【调用PresentAQuestion()函数】：
     *      【State==Prepared】---->【选择学习课题】--->【制定教案】---->【State=SchemePrepared】
     *       --->【提出一个问题】---->【_scheme.PresentAQuestion()】---->
     *      【State=TeachingState.ProblemBegin】
     * 
     * 
     *
     *************************************************************************************/
    public class Tutoring 
    {  
        protected CourseFactory _courseFactory; 
        protected TeachingScheme _scheme;
        protected LearningTopicRecord _record;
        protected Student _student;  

        public TeachingState State
        {
            get; set;
        } 

        DomainTopicKRModule DomainTopicKRModule
        {
            get { return _courseFactory.DomainTopicKRModule; }
        }

        public TeachingScheme CurrentScheme
        {
            get { return _scheme; }
        }

        public string CurrentCourse
        {
            get { return _courseFactory.Course; }
        } 

        public Tutoring(Student student)
        {
            State = TeachingState.Pre;
            _student = student;
        }
         
        public void OnCourseChanged(string course)
        {
            _scheme = null;
            _record = null;
            _courseFactory = new CourseFactory(course);
            State = TeachingState.Prepared;
        }

        public void PresentAQuestion(Action<bool,Question> callback)
        {
            if (State == TeachingState.Prepared || State==TeachingState.SchemeFinished)
            { 
                LearningTopic lt=SelectReadyTopic(_student.LastLTR);
                if(lt==null)
                {
                    callback(false, new Question($"没有选择到{_student.LastLTR.Topic}的后续学习课题。"));
                    return;
                }
                _scheme=MakeAScheme(lt);
                if(_scheme==null)
                {
                    callback(false, new Question($"{_student.LastLTR.Topic}无法形成教案。"));
                    return;
                } 
                State = TeachingState.SchemePrepared; 
            }

            if(State==TeachingState.SchemePrepared ||
                (State == TeachingState.ProblemFinished && !_scheme.IsFinished))
            {

                Question q = _scheme.PopupAQuestion();
                if(q==null)
                {
                    q = new Question($"<{_scheme.LearningTopic.Topic}>的所有学习问题学习结束。");
                    callback(true, q);
                    State = TeachingState.Prepared;
                }
                else
                {
                    callback(true, q);
                    State = TeachingState.ProblemBegin;
                }
                return;        
            }

            if (State==TeachingState.ProblemBegin)
            {
                callback(false, new Question("请回答前面提出的问题，再提出新问题！"));
                return;
            }  
            if(_scheme != null && _scheme.IsFinished)
            {
                callback(false, new Question("当前学习课题结束，点击Shift+Return智学系统为你选择下一个学习课题！"));
                State = TeachingState.SchemeFinished;
                return;
            }
        }

        public void OnSearch(string inputStr,Action<bool,string>callback)
        {
            DomainTopicKRModule.Parse(inputStr, callback);
        }

        public void Feedback(string inputStr,Action<string,double> callback)
        {

            if (State == TeachingState.ProblemBegin)
            {
                Evaluate(inputStr, callback);
                State = TeachingState.ProblemFinished;
            }
            else
            {
                callback("没有新问题！", 0);
            }
        }


        /// <summary>
        /// 根据输入的答案，检查回答是否正确。目前只是简单的打分，
        /// 以后需要进一步优化。
        /// </summary>
        /// <param name="answer">输入的答案</param> 
        /// <param name="callback">将评价和分数返回</param>
        /// <returns></returns>
        public void Evaluate(string answer, Action<string, double> callback)
        {
            _scheme.CompareAnswer(answer, callback);
        }         
        
        protected LearningTopic SelectReadyTopic(LearningTopicRecord record)
        {
            if (record == null)
            {
                //表示用户没有就该门课程进行相关学习，所以系统推荐从
                //头开始进行学习：选择第一章节中的最简单的课题进行学习。 
                //查找这门课程最简单的课题，一般从第一章第一节开始
                LearningTopic topic = DomainTopicKRModule.GetStartTopic(1, 1); 
                return topic;
            }

            LearningTopic newTopic = null;
            newTopic = FindFirstMatch(record.Chapter.NumberIndex, record.Section.NumberIndex);
            if (newTopic != null)
            {
                return newTopic;
            }

            ///获取record的下一个小节
            SectionItem nextSect = DomainTopicKRModule.GetNextSection(record.Chapter, record.Section);
            if(nextSect!=null)
            {
                newTopic = FindFirstMatch(record.Chapter.NumberIndex, nextSect.NumberIndex);
                if (newTopic != null)
                {
                    return newTopic;
                }
            }

            ///获取record的下一章
            ChapterItem nextChapter = DomainTopicKRModule.GetNextChapter(record.Chapter);
            if (nextChapter != null)
            {
                SectionItem sect = DomainTopicKRModule.GetStartSectionItem(nextChapter);
                LearningTopic lt = FindFirstMatch(nextChapter.NumberIndex, sect.NumberIndex);
                if (lt != null)
                { 
                    return lt;
                } 
            }

            ///这门课程学习结束！！！
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        protected bool Match(LearningTopic topic)
        {
            if (!_student.HasLearningHistory)
                return true;

            bool b = _student.LearningHistory.IsLearnedORPassed(topic);
            List<LearningTopic> preTopics = DomainTopicKRModule.GetPrequelTopics(topic);
            if (preTopics.Count == 0 && !b)//没有前序课程且没有学过或通过
                return true;

            foreach (var t in preTopics)
            {
                bool b1 = _student.LearningHistory.IsLearnedORPassed(t);
                if (!b1)//只要有一个前序课程没有通过，则不满足条件
                    return false;
            }
            if (!b)          ///前序课程都学过并通过，但本门课程没有学过，则满足条件
                return true;
            else             //前序课程都学过并通过，本门课程也通过，则不满足条件
                return false;
        }
        /// <summary>
        /// 从章节中查找第一个合适的学习课题
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="sect"></param>
        /// <returns></returns>
        protected LearningTopic FindFirstMatch(int chapter, int sect)
        {
            return DomainTopicKRModule.FindFirstMatch(chapter, sect, Match);
        } 


        /// <summary>
        /// 对应一个学习课题，制作一个教案
        /// TeachingScheme.
        /// </summary>
        /// <param name="topic"></param>
        protected TeachingScheme MakeAScheme(LearningTopic topic)
        {
            //一个学习课题可以对应多个知识类型的语义网
            List<System.Tuple<string,string>> typeAndNets= DomainTopicKRModule.GetKRTypeAndNameOfTopic(
                topic.ChaptItem.NumberIndex,
                topic.SectItem.NumberIndex,
                topic.Topic);

            if (typeAndNets.Count == 0)
                return null;

            Stack<PQA> sqas = new Stack<PQA>();

            ///(1)获取topic本身的相关问题////////////////////////////////////// 
            List<KnowledgeTopic> depts = new List<KnowledgeTopic>();
            foreach(var tp in typeAndNets)
            {
                string type=ProjectType.KCNameToProjectType(tp.Item1);
                PQAFactory sqaFactory = _courseFactory.CreateSQAFactory(type);

                if (sqaFactory == null)
                    continue;

                //PQA pqa = sqaFactory.CreateSingleRelPQA(topic.Topic);
                //sqas.Push(pqa);
                PQA pqa = sqaFactory.CreateSpecificPQA(topic.Topic);
                if (pqa == null)
                    continue;

                sqas.Push(pqa);
 
                depts.AddRange(sqaFactory.GetDeptTopics(topic.Topic));
            }
            /////////////////////////////////////////////////////////////////////

            //（2）在一个语义图中往往有关联知识点，这些知识点不是在该语义图内部建模，而是别的语义图中。
            //查询本课程相关的关联语义网
            Dictionary<KnowledgeTopic, Stack<PQA>> deptSQAs = new Dictionary<KnowledgeTopic, Stack<PQA>>();  
            foreach(var tp in depts)
            {
                if (_student.LearningHistory!=null && !_student.LearningHistory.IsLearnedORPassed(tp))
                {
                    Stack<PQA> tmp = new Stack<PQA>();
                    foreach (var krType in tp.KRTypes)
                    {
                        string type = ProjectType.KCNameToProjectType(krType);
                        PQAFactory sqaFactory = _courseFactory.CreateSQAFactory(type);
                        
                        if(sqaFactory==null)
                        {
                            continue;
                        }

                        PQA sqa = sqaFactory.CreateSpecificPQA(tp.Topic);
                        if (sqa == null)
                            continue;

                        tmp.Push(sqa);
                    }
                    deptSQAs[tp] = tmp; 
                } 
            }
            TeachingScheme scheme=new TeachingScheme(topic, sqas,deptSQAs);
            return scheme;
        }  

        private void AddReadyTopic(string topic, ref List<string> topics)
        {
            //int idx = _readyTopics.FindIndex(x => x == topic);
            //if (idx == -1)
            //    topics.Add(topic);
        } 

        /// <summary>
        /// 基于模糊规则fuzzy rules，根据学生的学习历史和学习performance，
        /// 推送下一个学习主题。这些规则制定了相应的教学策略。
        /// </summary>
        /// <param name="tdn"></param>
        /// <returns></returns>
        //public string GetMostSuitableTopic()
        //{
        //    List<Suitability> ss = new List<Suitability>();
        //    foreach(var r in _readyTopics)
        //    {
        //        ss.Add(CalculateSuitability(r));
        //    }
        //    int maxLevel = ss.Max(x => x.Level);
        //    int idx = ss.FindIndex(x => x.Level == maxLevel);

        //    if (idx < 0)
        //        throw new Exception("没有找到最佳学习主题");

        //    return _readyTopics[idx];
        //}

        /// <summary>
        /// 计算学习主题topic对学生的适应度suitability。该值的计算
        /// 涉及到学生模块和知识库模块。Suitability value of a topic
        /// is the degree of appropriateness between this topic and a student.
        /// Therefore,calculation of this parameter involves a comparison
        /// between the topic and the student's current state of
        /// knowledge and her cognitive ability.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public Suitability CalculateSuitability(string topic)
        {
            //从知识库中获取topic的相关属性，比如难易程度等。
            //从学生模块中获取学生的当前知识水平和认知能力。
            throw new Exception("适应度没有计算");
        }  

        //private Image ExpressionToImage(string str, Tuple<string, float> x1, Tuple<string, float> x2,
        //    Tuple<string, float> x3)
        //{
        //    string[] fs = str.Split(new char[] { '=' });

        //    string str1 = fs[1];
        //    str1 = str1.Replace(x1.Item1, x1.Item2.ToString());
        //    str1 = str1.Replace(x2.Item1, x2.Item2.ToString());
        //    str1 = str1.Replace(x3.Item1, x3.Item2.ToString());
        //    str += "=" + str1;

        //    str += "=" + CalculateFormula(fs[1], x1, x2, x3);

        //    fs = str.Split(new char[] { '=' });

        //    string latex = "";

        //    for (int i = 0; i < fs.Length; i++)
        //    {
        //        if (i < fs.Length - 1)
        //            latex += _converter.Convert(fs[i]) + "=";
        //        else
        //            latex += _converter.Convert(fs[i]);
        //    }

        //}

    }
}
