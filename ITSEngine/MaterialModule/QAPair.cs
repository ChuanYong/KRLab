using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS.DomainModule;

namespace ITS.MaterialModule
{
    /// <summary>
    /// Question-Answer Pair
    /// </summary>
    [Serializable]
    public class QAPair
    {

        //每个提问的答案，键用于答案的描述或序号，值表示问题的答案关键词。
        protected Question _q;
        protected Answer _a;   

        public Question Question
        {
            get { return _q; }
        }

        public Answer Answer
        {
            get { return _a; }
        }
        //从学习课题中分配得到的权重
        public double Score
        {
            get;set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q">问题</param>
        /// <param name="icd">importance,complexity, difficulty</param>
        public QAPair(Question q,Answer a)
        {
            _q = q;
            _a = a;
        }
    }
}
