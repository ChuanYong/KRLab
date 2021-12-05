using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.FuzzyEngine;

namespace ITS.StudentModule
{
    public class FuzzyEvaluation
    {
        private static IFuzzyEngine _fuzzyEngine;
        private static LinguisticVariable _performanceFuzzyVar;
        private static List<IMembershipFunction> _levelMFs;

        static FuzzyEvaluation()
        {
            _fuzzyEngine = new FuzzyEngineFactory().Default();

            //为学习成绩(learning performance)创建模糊集的成员关系函数(MF)
            _performanceFuzzyVar = new LinguisticVariable("Performance");
            _levelMFs = new List<IMembershipFunction>()
            {
                _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.Poor, 0, 0, 0.35),
                _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.Average, 0.3, 0.5, 0.55),
                _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.Good, 0.5, 0.65, 0.75),
                _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.VeryGood, 0.7, 0.8, 0.9),
                _performanceFuzzyVar.MembershipFunctions.AddTriangle(Performance.Excellent, 0.85, 1, 1),
            };
        }

        private static IEnumerable<double> _Fuzzy(double x)
        {
            foreach (var e in _levelMFs)
                yield return e.Fuzzify(x);

        }

        public static string Fuzzy(double correct)
        {
            //简单的分级评价
            if (correct < 0.6)
                return Performance.Poor;
            else if (correct < 0.7)
                return Performance.Average;
            else if (correct < 0.8)
                return Performance.Good;
            else if (correct < 0.9)
                return Performance.VeryGood;
            else
                return Performance.Excellent;
            //List<double> results = _Fuzzy(correct).ToList();
            //double max = results.Max();
            //int index = results.FindIndex(target => target == max);
            //return _performanceFuzzyVar.MembershipFunctions[index].Name;
        }
    }
}
