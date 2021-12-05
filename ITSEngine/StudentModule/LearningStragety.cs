using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.BNet;
using KRLab.Core.FuzzyEngine;

namespace ITS.StudentModule
{
    /// <summary>
    ///  
    /// </summary>
    public class LearningStragety
    {
        private FSM _fsm;
        private LinguisticVariable _performanceFuzzyVar;
        private IFuzzyEngine _fuzzyEngine;

        public LinguisticVariable PerformanceFuzzyVar
        {
            get { return _performanceFuzzyVar; }
        } 

        public LearningStragety()
        {
            InitFuzzyEngine();
        }
        /// <summary>
        /// 根据某道题的答题正确和答题时间，计算改题目的得分。
        /// 这个分数不是学生的实际得分，而是用来输入FSM进行推理
        /// </summary>
        /// <param name="c">答题的正确性，答对为1，答错为0</param>
        /// <param name="t">答题所用时间</param>
        /// <param name="tou">答题的时间阈值</param>
        /// <returns></returns>
        public double GetScore(double c,double t,double tou)
        {
            if (t <= tou)
                return c;
            else if (t > tou && t <= (3 * tou))
            {
                double tmp = (t - tou) / (2 * tou);
                return c * (1 - tmp * tmp);
            }
            else
                return 0;
        }

        private void InitFuzzyEngine()
        {
            _fuzzyEngine = new FuzzyEngineFactory().Default();

            //为学习成绩(learning performance)创建模糊集的成员关系函数(MF)
            _performanceFuzzyVar = new LinguisticVariable("Performance");
            var excel = _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.Excellent,0.95, 1, 1);
            var veryGood = _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.VeryGood, 0.8, 0.9, 0.9);
            var good = _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.Good, 0.7, 0.8, 0.9);
            var average = _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.Average, 0.5, 0.6, 0.75);
            var poor = _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.Poor, 0, 0, 0.55);
        }
    }
}
