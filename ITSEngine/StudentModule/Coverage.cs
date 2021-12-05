using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.StudentModule
{
    public class Coverage
    {
        /// <summary>
        /// 学生考核通过的所有学习主题
        /// </summary>
        protected List<string> _topics;

        public List<string> Topics
        { get { return _topics; } }

        public Coverage()
        {
            _topics = new List<string>();
        }

        public void AddTopic(string name)
        {
            if(!_topics.Contains(name))
                _topics.Add(name);
        }

    }
}
