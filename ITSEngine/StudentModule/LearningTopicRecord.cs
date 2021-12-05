using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using ITS.DomainModule; 

namespace ITS.StudentModule
{
    /// <summary>
    /// 这个类主要用于保存学习课程的记录
    /// </summary>
    [Serializable]
    public class LearningTopicRecord
    {
        protected LearningTopic _topic;
        protected LearningResult _result;
        protected DateTime _date; 

        public DateTime Date
        {
            get { return _date; }
        }

        public LearningTopic LearningTopic
        {
            get { return _topic; }
        }

        public string Course
        {
            get { return _topic.Course; }
        }

        public ChapterItem Chapter
        {
            get { return _topic.ChaptItem; }
        }

        public string Topic
        {
            get { return _topic.Topic; }
        }

        public SectionItem Section
        {
            get { return _topic.SectItem; }
        }

        public bool IsEmpty
        {
            get { return _topic.IsEmpty && _result==null; }
        }

        public LearningResult Result
        {
            get { return _result; }
        } 

        public LearningTopicRecord(LearningTopic topic,double score) 
        {
            _topic = topic;
            _result = new LearningResult(score);
            _date = DateTime.Now;  
        }

        public LearningTopicRecord(LearningTopic topic, LearningResult result)
        {
            _topic = topic;
            _result = result;
            _date = DateTime.Now;             
        }
    }
}
