using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITS.DomainModule;
namespace ITS.StudentModule
{
    [Serializable]
    public class KnowledgeTopicRecord
    {
        protected KnowledgeTopic _topic;
        protected LearningResult _result;
        protected DateTime _date;

        public KnowledgeTopic Topic
        {
            get { return _topic; }
        }
        public LearningResult Result
        {
            get { return _result; }
        }

        public KnowledgeTopicRecord(KnowledgeTopic topic,LearningResult result)
        {
            _topic = topic;
            _result = result;
            _date = DateTime.Now;
        }
    }
}
