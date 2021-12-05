using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.StudentModule
{
    /// <summary>
    /// 
    /// 描述学生的认知水平cognitive state,
    /// </summary>
    public class CognitiveState
    {
        //学生的能力等级,参见Performance类
        protected Performance _performance;
        protected Ability _abilities;
        //学生学过的所有主题
        protected Coverage _coverage;

        public Performance Perforamce
        {
            get { return _performance; }
            set { _performance = value; }
        }
        public Ability Abilities
        {
            get { return _abilities; }
            set { _abilities = value; }
        }

        public Coverage Coverage
        { get { return _coverage; } }

        public CognitiveState()
        { 
        }

        public void UpdateCoverage(string topic)
        {
            _coverage.AddTopic(topic);
        }

    }
}
