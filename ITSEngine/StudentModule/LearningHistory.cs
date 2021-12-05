using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
 
using ITS.DomainModule; 

namespace ITS.StudentModule
{     

    /// <summary>
    /// 整个一门课程的学习历史保存在一个文件中。学习历史包括学习课题的学习历史
    /// 和知识点的学习历史。 
    /// </summary> 
    [Serializable]
    public class LearningHistory
    {

        /// <summary>
        /// 用于保存学习历史，每个学习主题对应的结点，标明
        /// 是否学过，是否准备好，是否未学，学习成绩等等。
        /// 在Domain module中的Topic dependency graph基础上修改而来。
        /// 所以_sNet的初始网络为知识库里面的TDG
        /// </summary>  
        protected string _course;
        /// <summary>
        /// 用于保存学习课题的学习历史
        /// </summary>
        protected Dictionary<string, Chapter> _chapterDict;
        /// <summary>
        /// 用于保存知识点的学习历史。key是知识点的名称
        /// </summary>
        protected Dictionary<string, LearningResult> _knowledgeTopicReuslts;
        
        public Dictionary<string,Chapter> ChapterDict 
        {
            get { return _chapterDict; }
        } 

        public string Course
        {
            get { return _course; } 
        }

        public List<string> Chapters
        {
            get { return _chapterDict.Keys.ToList(); }
        }

        public LearningHistory()
        {
            _chapterDict = new Dictionary<string, Chapter>();
            _knowledgeTopicReuslts = new Dictionary<string, LearningResult>();
        }

        public double GetScore(string topic)
        {
            return 0;
        }

        /// <summary>
        /// 获取课程章节下面的所有学习课题结果
        /// </summary> 
        /// <param name="chapter"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public Dictionary<string,LearningResult> GetTopicResults(ChapterItem chapter,SectionItem section)
        {
            if (!_chapterDict.Keys.Contains(chapter.ToString()))
                return null;
            Chapter cp = _chapterDict[chapter.ToString()];
            return cp.GetTopicDict(section.ToString());
        }

        public void AddChapter(ChapterItem chapter, Chapter chaptDict)
        {
            _chapterDict[chapter.ToString()] = chaptDict;
        }



        /// <summary>
        /// 判断课题是否学过或者学了是否通过
        /// </summary>
        /// <param name="course"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public bool IsLearnedORPassed(LearningTopic topic)
        {
            return IsLearnedORPassed(topic.Course, topic.ChaptItem.ToString(), topic.SectItem.ToString(), topic.Topic);
        }

        public bool IsLearnedORPassed(string course,string chapter,string sect,string topic)
        {
            if (_course != course)
                return false;

            if (_chapterDict == null)
                return false;

            if (!_chapterDict.Keys.Contains(chapter))
                return false;
            Chapter cpt = _chapterDict[chapter];
            LearningResult result = cpt.GetTopicResult(sect,topic);
            if (result == null)
                return false;

            if (result.Level == Performance.Poor)
                return false;
            else
                return true;
        }

        public bool IsLearnedORPassed(KnowledgeTopic topic)
        {
            if (!_knowledgeTopicReuslts.ContainsKey(topic.Topic))
                return false;
            LearningResult result = _knowledgeTopicReuslts[topic.Topic];
            if (result.Level == Performance.Poor || result.Level == Performance.Average)
                return false; 
            return true;                
        }

        /// <summary>
        /// 判断所有的学习课题是否通过
        /// </summary>
        /// <param name="course"></param>
        /// <param name="topics"></param>
        /// <returns></returns>
        public bool IsAllLearnedORPassed(List<LearningTopic> topics)
        {            
            foreach(var topic in topics)
            {
                if (!IsLearnedORPassed(topic))
                    return false;
            }
            return true;
        } 

        public void AddLearningTopicHistory(LearningTopicRecord record)
        {
            AddTopicHistory(record.LearningTopic, record.Result);
        }

        public void AddKnowledgeTopicHistory(KnowledgeTopicRecord record)
        {
            if (record == null)
                return;
            _knowledgeTopicReuslts[record.Topic.Topic] = record.Result; 
        }

        public void AddKnowledgeTopicHistory(string topic,LearningResult result)
        {
            if (result == null)
                return;
            _knowledgeTopicReuslts[topic] = result;
        }

        /// <summary>
        /// 添加一个学习结果
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="info"></param>
        public void AddTopicHistory(LearningTopic topic, LearningResult info)
        {
            if (topic == null || info == null)
                return;

            _course = topic.Course;

            if(_chapterDict.Keys.Contains(topic.ChaptItem.ToString()))
            {
                _chapterDict[topic.ChaptItem.ToString()].AddTopic(topic,info); 
            }
            else
            {
                Section sect = new Section(topic.SectItem.ToString());
                sect.AddLearningResult(topic.Topic, info);
                Chapter chapter = new Chapter(topic.ChaptItem.Name);
                chapter.AddSection(topic.SectItem, sect);
                _chapterDict[topic.ChaptItem.ToString()] = chapter;
            }
        }
         
        /// <summary>
        /// 保存一门课程的学习历史
        /// </summary>
        /// <param name="node"></param>
        /// <param name="course"></param>
        public void SerializeLearningTopic(XmlElement node)
        {
            XmlElement courseElement = node.OwnerDocument.CreateElement("course");
            courseElement.InnerText = _course;
            node.AppendChild(courseElement); 

            foreach (var chaptName in Chapters)
            {
                XmlElement chaptItem = node.OwnerDocument.CreateElement("chapter");
                chaptItem.InnerText = chaptName;
                node.AppendChild(chaptItem); 

                Chapter chapt = _chapterDict[chaptName];
                foreach (var sectName in chapt.Sections)
                {
                    XmlElement sectionItem = chaptItem.OwnerDocument.CreateElement("section");
                    sectionItem.InnerText = sectName;
                    chaptItem.AppendChild(sectionItem);  

                    Dictionary<string, LearningResult> topics = chapt.GetTopicDict(sectName);
                    foreach(var topic in topics)
                    {
                        XmlElement topicItem = sectionItem.OwnerDocument.CreateElement("topic");
                        topicItem.SetAttribute("name", topic.Key);
                        topicItem.SetAttribute("score", topic.Value.Score.ToString());
                        topicItem.SetAttribute("level", topic.Value.Level); 

                        //XmlElement time = topicItem.OwnerDocument.CreateElement("time");
                        //time.InnerText = topic.Value.Time.ToString();
                        //topicItem.AppendChild(time);

                        sectionItem.AppendChild(topicItem);
                    }
                }
            }
        }

        public void SerializeKnowledgeTopic(XmlElement node)
        {
            XmlElement ktElement = node.OwnerDocument.CreateElement("KnowledgeTopic");
            ktElement.InnerText = "record";
            node.AppendChild(ktElement);

            foreach (var dict in _knowledgeTopicReuslts)
            {
                XmlElement krElement = ktElement.OwnerDocument.CreateElement("topic");
                krElement.InnerText = dict.Key;
                krElement.SetAttribute("score", dict.Value.Score.ToString());
                krElement.SetAttribute("level", dict.Value.Level);
                ktElement.AppendChild(krElement);  
            }
        }
        public void DeserializeKnowledgeTopic(XmlElement node, out Dictionary<string, LearningResult> recordDict)
        {
            recordDict = new Dictionary<string, LearningResult>();
            foreach (XmlElement knowledgeItem in node.GetElementsByTagName("KnowledgeTopic"))
            {
                foreach (XmlElement recordItem in knowledgeItem.GetElementsByTagName("topic"))
                {
                    string name = recordItem.FirstChild.InnerText; 
                    XmlAttribute scoreAtt = recordItem.Attributes["score"];
                    XmlAttribute levelAtt = recordItem.Attributes["level"]; 
                    LearningResult result = new LearningResult(double.Parse(scoreAtt.InnerText), levelAtt.InnerText);
 
                    recordDict[name] = result;
                }

            }
        }

        public virtual void Deserialize(XmlElement node)
        {  
            foreach( XmlElement courseItem in node.GetElementsByTagName("course"))
            { 
                _course = courseItem.InnerText; 

                foreach (XmlElement chapterItem in node.GetElementsByTagName("chapter"))
                {
                    Chapter chapter;
                    DeserializeChapter(chapterItem,_course, out chapter);
                    _chapterDict[chapterItem.FirstChild.InnerText]=chapter;
                }

            }
             
        }

        private void DeserializeChapter(XmlElement node,string course,
            out Chapter chapter)
        {
            string[] strs = node.FirstChild.InnerText.Split('-');
            ChapterItem cpItem = new ChapterItem(strs[0], strs[1]);

            chapter = new Chapter(cpItem.ToString());

            foreach (XmlElement sectItem in node.GetElementsByTagName("section"))
            {
                string[] strs1 = sectItem.FirstChild.InnerText.Split('-');
                SectionItem stItem = new SectionItem(strs1[0],strs1[1]);                  
                Section section = new Section(stItem.ToString());
                foreach (XmlElement topicItem in sectItem.GetElementsByTagName("topic"))
                {
                    XmlAttribute nameAtt = topicItem.Attributes["name"];
                    XmlAttribute scoreAtt = topicItem.Attributes["score"];
                    XmlAttribute levelAtt = topicItem.Attributes["level"];

                    LearningResult result = new LearningResult(double.Parse(scoreAtt.InnerText),levelAtt.InnerText);
                    section.AddLearningResult(nameAtt.InnerText, result);
                } 
                chapter.AddSection(stItem, section);
            }

        }

        public void Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("从文件导入学习历史输入的路径错误！");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("学习历史文件没有找到！");

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(fileName);
            }
            catch(Exception ex)
            {
                throw new IOException("读入学习历史出错！",ex);
            }

            XmlElement root = document["LearningHistory"];
             
            try
            {
                Deserialize(root);
                DeserializeKnowledgeTopic(root, out _knowledgeTopicReuslts);
            }
            catch(Exception ex)
            {
                throw new IOException("读入学习历史出错！", ex);
            } 
        }

        public void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("保存学习历史时输入路径错误！");

            XmlDocument document = new XmlDocument();
            XmlElement root = document.CreateElement("LearningHistory");
            document.AppendChild(root);

            SerializeLearningTopic(root);
            SerializeKnowledgeTopic(root);

            try
            {
                document.Save(fileName);
            }
            catch(Exception ex)
            {
                throw new IOException("保存学习历史出错", ex);
            }
        }

    }
}
