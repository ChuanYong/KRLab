using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

using ITS.DomainModule;

namespace ITS.MaterialModule
{
    /// <summary>
    /// 对应一个考试题目或问题，包括问题背景故事QuestionStory，若干个提问Question
    /// ，以及提问对应的正确答案Answer。一个问题要包含至少一个提问。
    /// PQA=problem+question+answer
    /// </summary>
    public class PQA
    {
        /// <summary>
        /// 问题所在的学习课题
        /// </summary>
        protected string _topic;

        protected Problem _problem; 

        protected int _id;
        //一个题目下面可以有多个问题，但至少要有一个。 
        protected Dictionary<int,QAPair> _qas;

        public int QNumber
        {
            get { return _id; }
        }

        public Dictionary<int,QAPair> QAs
        {
            get { return _qas; }
            set { _qas = value; }
        } 

        public Problem Problem
        {
            get { return _problem; }
        }

        public int QANumber
        {
            get { return _qas.Count; }
        } 

        public PQA(string topic,Problem p)
        {
            _id = 0;
            _topic = topic;
            _problem = p;
            _qas = new Dictionary<int, QAPair>();
        }

        public void AddQA(QAPair pa)
        {
            _id++;
            _qas[_id] = pa;
        }

    }
}
