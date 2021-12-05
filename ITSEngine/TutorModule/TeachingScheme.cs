using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using ITS.StudentModule;
using ITS.MaterialModule;

using KRLab.Core;
using ITS.DomainModule;

using Utilities;

namespace ITS.TutorModule
{
    /************************************************************************
     * 该类表示一个教案，用于指导学生的学习。一个教案针对一个学习课题而制定。
     * 该学习课题为_learningTopic。
     *
     ************************************************************************/
    [Serializable]
    public class TeachingScheme
    {  
        protected LearningTopic _learningTopic;
        //学习课题对应的SQA，每个学习课题有可能对应多个知识类型的语义网
        protected Stack<PQA> _sqas;
        //学习课题的相关知识点对应的PQA(没有学过或学过但没有通过的知识点)
        protected Dictionary<KnowledgeTopic, Stack<PQA>> _deptPQAs;
        protected Stack<KnowledgeTopic> _preNeededTopics; 

        protected PQA _currentPQA;
        protected QAPair _currentQA;
        protected Stack<PQA> _currentSQAStack;
        protected KnowledgeTopic _currentTopic;

        /// <summary>
        /// 记录每个课题的得分，为折算分数，得分除以总分，小于等于1
        /// 用于保存学习历史
        /// </summary>
        protected Dictionary<KnowledgeTopic, double> _deptTopicResultDict;
        protected double _learningTopicResult=0;
         

        //问题的序号
        protected int _qIndex;

        List<int> _usedQs;
        List<int> _vailableQs;
        List<int> _failedQs;

        StringWriter _writer = new StringWriter();  

        public string Course
        {
            get { return _learningTopic.Course; }
        } 

        public LearningTopic LearningTopic
        {
            get { return _learningTopic; }
        }        

        public StringWriter StringWriter
        {
            get { return _writer; }
        }

        public Stack<PQA> SQAs
        {
            get { return _sqas; }
        }

        public KnowledgeTopic CurrentTopic
        {
            get { return _currentTopic; }
        }

        public PQA CurrentPQA
        {
            get { return _currentPQA; }
        }
         

        public QAPair CurrentQA
        {
            get { return _currentQA; }
        }

        public string[] CurrentAnswer
        {
            get
            {
                if (CurrentQA == null)
                    return null;

                if (CurrentQA.Answer is TextAnswer)
                {
                    string[] ans = (string[])CurrentQA.Answer.Content;
                    return ans;
                }
                else
                    return null;
            }
        }

        public double CurrentQScore
        {
            get { return CurrentQA.Score; }
        }

        public int CurrentQIndex
        { get; set; }

        public List<int> VailableQs
        {
            get { return _vailableQs; }
        } 

        public bool IsFinished
        {
            get
            {
                //当学习课题及其相应的问题为0，则表示对教案的学习结束
                if (_sqas.Count==0 && _vailableQs.Count == 0 )
                    return true;
                else
                    return false;
            }
        } 

        public Dictionary<KnowledgeTopic, Stack<PQA>> DeptTopics
        {
            get { return _deptPQAs; }
        } 

        public Dictionary<KnowledgeTopic,double> DeptTopicResultDict
        {
            get { return _deptTopicResultDict; }
        }

        public double LearningTopicResult
        {
            get { return _learningTopicResult; }
        }

        public bool HasDeptTopics
        {
            get { return _preNeededTopics.Count > 0; }
        }


        public TeachingScheme(LearningTopic topic,Stack<PQA> sqas,Dictionary<KnowledgeTopic, Stack<PQA>> deptPQAs)
        {
            _learningTopic = topic;
            _sqas = sqas;

            _deptPQAs = deptPQAs; 
            _preNeededTopics = new Stack<KnowledgeTopic>(deptPQAs.Keys);

            _vailableQs = new List<int>();
            _deptTopicResultDict = new Dictionary<KnowledgeTopic, double>();

            ArrangeWeightForQuestion();
        }

        protected void InitForCurrentSQA(KnowledgeTopic topic,PQA currentSQA)
        {
            _currentTopic = topic;
            _currentPQA = currentSQA;
            _usedQs = new List<int>();
            _failedQs = new List<int>();
            _vailableQs = _currentPQA.QAs.Keys.ToList();
            _qIndex = 0;
            CurrentQIndex = int.MaxValue;
        } 

        /// <summary>
        /// 检查提交的回答answer，并对结果进行打分。
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="callback"></param>
        public void CompareAnswer(string answer,Action<string,double>callback)
        { 
            string [] corrects = CurrentAnswer;
            List<string> errors = new List<string>();

            string info = string.Empty;
            double result = 0;
            foreach (var d in corrects)
            {
                if (!answer.Contains(d))
                    errors.Add(d);
            }

            if (corrects.Length == 1)
            {
                if (errors.Count != 0)
                {
                    result = 0;
                    info="你的回答错误！";
                }
                else
                {
                    result = CurrentQScore;
                    info = "回答正确！";
                }
            }

            if (corrects.Length > 1)
            {
                if (errors.Count == corrects.Length)
                {
                    result = 0;
                    info = "满分为：" + CurrentQScore.ToString() + "，你的回答全错！";
                }
                else if (errors.Count == 0)
                {
                    result = CurrentQScore;
                    info = "满分为：" + CurrentQScore.ToString() + "，你的回答正确！得满分。";
                }
                else
                {
                    double x = CurrentQScore / corrects.Length;
                    result = CurrentQScore - x * errors.Count;
                    info = "满分为：" + CurrentQScore.ToString() + "，你只是回答对了一部分。";
                }
            }

            if(CurrentTopic.Topic== _learningTopic.Topic)
            {
                _learningTopicResult += result / _topicTotalWeight[CurrentTopic];
            }
            else
            {
                if (!_deptTopicResultDict.ContainsKey(CurrentTopic))
                    _deptTopicResultDict[CurrentTopic] = result / _topicTotalWeight[CurrentTopic];
                else
                    _deptTopicResultDict[CurrentTopic] += result / _topicTotalWeight[CurrentTopic];
            }


            callback.Invoke(info, result);
        }

        public Stack<PQA> GetSQAsOfDeptTopic(KnowledgeTopic topic)
        {
            if (!_deptPQAs.ContainsKey(topic))
                return null;
            return _deptPQAs[topic];
        }

        protected Dictionary<KnowledgeTopic, double> _topicTotalWeight = new Dictionary<KnowledgeTopic, double>();
        /// <summary>
        /// 根据目录语义网中的权重，再根据章节语义网中的依赖知识点
        /// 将权重进行重新分配，将权重分一些给依赖知识点的学习。
        /// </summary> 
        public void ArrangeWeightForQuestion()
        {
            double weight = _learningTopic.WeightInChapter;
            double x = weight;
            ///每个学习课题有可能对应多个知识类型的语义网
            if(DeptTopics!=null && DeptTopics.Count>0)
            {
                x *= 0.6;
            }
            int nb0 = SQAs.Count;
            double x0 = x / nb0;
            foreach (var sqa in SQAs)
            {
                double y = x0 / sqa.QAs.Count;
                foreach (var q in sqa.QAs)
                {
                    q.Value.Score = y;
                }
            }

            _topicTotalWeight[_learningTopic] = weight;

            //每个关联知识点平均分配学习课题中的40%
            if(DeptTopics!=null && DeptTopics.Count>0)
            { 
                double x1 = (0.4 * weight) / DeptTopics.Count;
                x1 /= DeptTopics.Count;
                foreach (var topic in DeptTopics)
                {
                    _topicTotalWeight[topic.Key] = x1;
                    Stack<PQA> sqas = topic.Value;
                    foreach(var sqa in sqas)
                    {
                        double y = x1 / sqa.QAs.Count;
                        foreach(var q in sqa.QAs)
                        {
                            q.Value.Score = y;
                        }
                    }
                }
            }

        }


        /// <summary>
        /// 获取问题故事和相关问题
        /// </summary>
        /// <returns></returns>
        public string GetSQText()
        {
            string str = CurrentPQA.Problem.Story+System.Environment.NewLine;

            int i = 1;
            foreach (var q in CurrentPQA.QAs)
            {
                str += "(" + i.ToString() + ") " + q.Value + "\n";
                ++i;
            }

            return str;
        }

        /// <summary>
        /// 用户获取问题，同时将该问题设定为当前问题。
        /// </summary>
        /// <param name="idx"></param>
        /// <returns>返回问题的序号和问题本身</returns>
        public Question PopupAQuestion()
        { 
            //先考核学习课题关联的知识点
            if(_preNeededTopics.Count>0 && _vailableQs.Count==0)
            {
                KnowledgeTopic lt = _preNeededTopics.Pop();
                _currentSQAStack = _deptPQAs[lt]; //知识点作为索引，得到值为PQA栈
                InitForCurrentSQA(lt,_currentSQAStack.Pop());//出栈PQA
            }
            //再考核学习课题
            else if(_sqas.Count>0 && _vailableQs.Count==0)//学习课题的知识点数大于零，且有效问题数为零
            {
                InitForCurrentSQA(_learningTopic, _sqas.Pop());//获得_currentPQA
            }

            int i = Rand.Random(_vailableQs);//获取字典中可用问题的随机索引值
            if (i == -1)
                return null;

            CurrentQIndex = i;
            _currentQA = CurrentPQA.QAs[i];//获得PQA类型值的字典QAs

            _usedQs.Add(i);
            _vailableQs.Remove(i);//移除已经使用过的问题
            _qIndex++;

            Question q = _currentQA.Question;//最后获得问题
            //q.Topic = _currentTopic.Topic;
            //q.Text = "(" + _qIndex.ToString() + ") " + q.Text;

            //if (_qIndex == 1)
            //{
            //    LearningTopic lt = (LearningTopic)_currentTopic;
            //    if (lt != null)
            //    {
            //        q.Text = "为你推荐学习课题" + lt.ChaptItem.Format + "---" + lt.SectItem.Format + lt.Topic + "\n" +
            //            q.Text;
            //    }
            //    else
            //        q.Text = "为你推荐学习课题" + _currentTopic.Topic + "\n" +
            //            q.Text;

            //}
            return q;          

        } 


        public void AddFailedQs(int key)
        {
            _failedQs.Add(key);
        }

        public string GetFailedQuestionText(int key)
        {
            if (_failedQs.Count==0 || !CurrentPQA.QAs.Keys.Contains(key))
                return null;
            _currentQA= CurrentPQA.QAs[key];
            CurrentPQA.QAs.Remove(key);

            return _currentQA.Question.Content;
        }

        //获取第idx个提问的答案
        public object GetAnswerContent(int idx)
        {
            if (idx >= CurrentPQA.QAs.Count)
            {
                throw new Exception("输入的序号大于答案的数目");
            }

            return CurrentPQA.QAs[idx].Answer.Content;
        } 

        public string ReadCourseName()
        {
            var reader = new StringReader(StringWriter.ToString());
            string line;
            int lineNb = 0;

            do
            {
                lineNb += 1;
                line = reader.ReadLine();
                if (line.Contains("课程"))
                {
                    return reader.ReadLine();
                }

            } while (line != null);

            return null;
        }         


    }

    public class TeachingSchemeDocIO
    {
        protected string _Text;
        public TeachingSchemeDocIO(string text)
        {
            _Text = text;
        }
        public string ReadExpName()
        {
            var reader = new StringReader(_Text);
            string line;
            int lineNb = 0;

            do
            {
                lineNb += 1;
                line = reader.ReadLine();
                if (line.Contains("实验名称"))
                {
                    return reader.ReadLine();
                }

            } while (line != null);

            return null;
        }

        public string ReadExpWithoutExam()
        {
            int index = _Text.IndexOf("问题与讨论");
            if (index > 0)
            {
                return _Text.Substring(0, index - 0);
            }
            else
            {
                return "";
            }
        }

        public string ReadExpPrinciple()
        {
            var reader = new StringReader(_Text);
            string line;
            int lineNb = 0;

            do
            {
                lineNb += 1;
                line = reader.ReadLine();
                if (line.Contains("实验原理"))
                {
                    break;
                }

            } while (line != null);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line);

            do
            {
                line = reader.ReadLine();
                if (line.Contains("实验方法") || line.Contains("实验步骤"))
                {
                    break;
                }
                sb.AppendLine(line);
            } while (line != null);

            return sb.ToString();
        }

        public string ReadExpMethods()
        {
            var reader = new StringReader(_Text);
            string line;
            int lineNb = 0;

            do
            {
                lineNb += 1;
                line = reader.ReadLine();
                if (line.Contains("实验方法") || line.Contains("实验步骤"))
                {
                    break;
                }

            } while (line != null);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line);

            do
            {
                line = reader.ReadLine();
                if (line.Contains("问题与讨论") || line.Contains("问题"))
                {
                    break;
                }
                sb.AppendLine(line);
            } while (line != null);

            return sb.ToString();
        }

        /// <summary>
        /// 读取实验后测试的问题和答案
        /// </summary>
        /// <returns></returns>
        public string ReadPosTestQuesAns()
        {
            var reader = new StringReader(_Text);
            string line;
            int lineNb = 0;

            do
            {
                lineNb += 1;
                line = reader.ReadLine();
                if (line.Contains("问题与讨论") || line.Contains("问题"))
                {
                    break;
                }

            } while (line != null);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line);

            do
            {
                line = reader.ReadLine();
                sb.AppendLine(line);
            } while (line != null);

            return sb.ToString();
        } 

        public static string ChangeResultDicToString(List<Dictionary<string, float>> resultDicts)
        {
            string text = "";
            foreach (var dict in resultDicts)
            {
                foreach (var errorDictKey in dict.Keys)
                {
                    text += errorDictKey + "：扣" + dict[errorDictKey] + "分" + "\n";
                }
            }

            if (text == "")
                text = "无错误\n";

            return text;
        }
    }
}
