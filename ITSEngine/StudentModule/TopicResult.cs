using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core;

namespace ITS.StudentModule
{
    /// <summary>
    /// 学生对某道题目的测试结果
    /// </summary>
    [Serializable]
    public class LearningResult
    {
          
        public string Level
        {
            get;set;
        }
        public double Score
        {
            get;set;
        } 

        static LearningResult()
        {

        } 

        public LearningResult(double correct)
        {
            if (correct < 0 || correct > 1.001)
                throw new ArgumentNullException("enter error!!!");
            Score = correct;
            Level=FuzzyEvaluation.Fuzzy(correct);
        } 

        public LearningResult(double score,string level)
        {
            Score = score;
            Level = level;
        }

    }

}
