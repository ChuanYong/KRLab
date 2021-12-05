using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS.DomainModule;

namespace ITS.StudentModule
{
    ///小节下所有的学习记录
    using ResultDcit = Dictionary<string, LearningResult>;
    public class Section
    {
        protected ResultDcit _sectionDict;
        //小节的全名，如，第1节-时间的测量
        protected string _sectName;
        public Dictionary<string,LearningResult> SectDict
        {
            get { return _sectionDict; }
        }

        public List<string> Sections
        {
            get { return _sectionDict.Keys.ToList(); }
        }

        public Section(string name)
        {
            _sectName = name;
            _sectionDict = new ResultDcit(); 
        }

        public LearningResult GetLearningResult(string topic)
        {
            if (!_sectionDict.Keys.Contains(topic))
                return null;
            return _sectionDict[topic];
        }

        public void AddLearningResult(string topic,LearningResult result)
        {
            _sectionDict[topic] = result;
        }
    }
}
