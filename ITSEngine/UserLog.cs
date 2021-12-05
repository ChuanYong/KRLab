using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS.StudentModule;
using ITS.DomainModule;
using KRLab.Core;

namespace ITS
{

    /// <summary>
    /// 用于保存退出系统时，保存当前的
    /// 学习题目、课程和时间
    /// </summary>
    
    [Serializable]
    public class UserLog
    {
        protected string _chosenSubject;
        protected string _chosenCourse;
        protected Dictionary<string, LearningTopicRecord> _topics; 

        //某门课程的章，节，课题
        //用于保存系统退出，用户最后一次学习的章、节和课题，
        //用于下次进入，查询历史。
        public Dictionary<string, LearningTopicRecord> Topics
        {
            get { return _topics; }
        }

        public string ChosenCourse
        {
            get { return _chosenCourse; }
            set { _chosenCourse = value; }
        }

        public string ChosenSubject
        {
            get { return _chosenSubject; }
            set { _chosenSubject = value; }
        }

        public bool IsEmpty
        {
            get { return _topics.Count == 0; }
        }

        public UserLog()
        {
            _topics = new Dictionary<string, LearningTopicRecord>();
        }

        //public UserLog(DateTime date,
        //    string course,
        //    ChapterItem chapter,
        //    SectionItem section,
        //    string topic,
        //    double score)
        //{
        //    _topics = new Dictionary<string, LearningTopicRecord>();
        //    _topics.Add(course, new LearningTopicRecord(course,chapter,section, topic,score));
        //}

        public UserLog(DateTime date,string course, LearningTopicRecord topic)
        {
            _topics = new Dictionary<string, LearningTopicRecord>();
            _topics.Add(course, topic);
        }
         

        /// <summary>
        /// 获取学习记录中最后学习记录
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public LearningTopicRecord GetLastTopicRecord()
        {
            DateTime now = DateTime.Now;
            TimeSpan ts = new TimeSpan(10000000, 0, 0, 0, 0); ;
            string choosed = null;

            foreach (var course in _topics.Keys)
            {
                //这种情况下，用户上一次选择了某门课程，但没有进行学习，
                //所以只是保存了科目的名称，没有章节和课题。
                if (_topics[course] == null)
                    continue;

                TimeSpan t = _topics[course].Date.Subtract(now);
                if (t < ts)
                {
                    ts = t;
                    choosed = course;
                }
            }

            if (choosed == null)
                return null; 

            return _topics[choosed];
        }
         
        public void AddTopic(LearningTopicRecord topic)
        {
            _topics[topic.Course] = topic;
        }

        public bool IsContains(string course)
        {
            return _topics.Keys.Contains(course);
        }

        public LearningTopicRecord GetRecord(string course)
        {
            if (!_topics.Keys.Contains(course))
                return null;

            return _topics[course];
        }
    }
}
