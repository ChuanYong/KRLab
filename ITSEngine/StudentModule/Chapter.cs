using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS.DomainModule;

namespace ITS.StudentModule
{ 
    public class Chapter
    {
        /// <summary>
        /// 某章中包含的所有小节,key是小节名称，比如“第1节-时间和长度”
        /// </summary>
        protected Dictionary<string, Section> _sectDict ;
        /// <summary>
        /// 章的全名，如，第1章-机械运动
        /// </summary>
        protected string _chapterName;

        public List<string> Sections
        {
            get { return _sectDict.Keys.ToList(); }
        }

        public Chapter(string chapterName)
        {
            _chapterName = chapterName;
            _sectDict = new Dictionary<string, Section>();
        }

        public void AddSection(SectionItem sect, Section sectDic)
        {
            _sectDict[sect.ToString()] = sectDic;
        }

        public Dictionary<string, LearningResult> GetTopicDict(string sect)
        {
            if (!_sectDict.Keys.Contains(sect))
                return null;
            return _sectDict[sect].SectDict;
        }

        public Section GetSectionDict(string chapter)
        { 
            if (!_sectDict.Keys.Contains(chapter))
                return null;

            return _sectDict[chapter];
        }
        public Section GetSectionDict(string chapterIndex,
            string chapterName)
        {
            ChapterItem item = new ChapterItem(chapterIndex, chapterName); 
            if(_sectDict.Keys.Contains(item.ToString()))
                return _sectDict[item.ToString()];
            return null;
        }

        public LearningResult GetTopicResult(LearningTopic topic)
        {
            if (topic == null)
                return null;
            if (!_sectDict.Keys.Contains(topic.SectItem.ToString()))
                return null;

            Section sectDict = _sectDict[topic.SectItem.ToString()];
            LearningResult result = sectDict.GetLearningResult(topic.Topic);

            return result;
        }

        public LearningResult GetTopicResult(string section,string topic)
        {
            if (topic == null)
                return null;
            if (!_sectDict.Keys.Contains(section))
                return null;

            Section sectDict = _sectDict[section];
            LearningResult result = sectDict.GetLearningResult(topic);

            return result;
        }

        public void AddTopic(LearningTopic topic, LearningResult result)
        {
            if (_chapterName != topic.ChaptItem.ToString())
                return; 
            if(_sectDict.Keys.Contains(topic.SectItem.ToString()))
            {
                _sectDict[topic.SectItem.ToString()].AddLearningResult(topic.Topic,result);
            }
            else
            {
                Section newSect = new Section(topic.SectItem.ToString());
                newSect.AddLearningResult(topic.Topic, result);
                _sectDict[topic.SectItem.ToString()] = newSect;
            }

        } 
    }
}
