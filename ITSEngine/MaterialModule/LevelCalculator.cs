using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.FuzzyEngine;
using Utilities;

namespace ITS.MaterialModule
{
    public class LevelCalculator
    {
        private static LinguisticVariable _fuzzyVar;
        private static LinguisticVariable _levelFuzzyVar;
        private static List<IMembershipFunction> _mfs;
        private static List<IMembershipFunction> _levelMFs;
        private static IFuzzyEngine _fuzzyEngine;

        //重要性的权重
        private static double x1 = 0.4;
        //复杂度的权重
        private static double x2 = 0.3;
        //难度的权重
        private static double x3 = 0.3;
        //问题的分级名称如下：非常低、低、较低、中等、较高、高、非常高
        /// _levels记录了每个分级的名称和对应的系数
        Dictionary<string, double> _levelDict=new Dictionary<string, double>();

        private static string[] _fuzzySet = new[] { ITSStrings.Low, ITSStrings.Medium, ITSStrings.High };
        private static string[] _levelFuzzySet = new[] {ITSStrings.VeryLow,ITSStrings.Low,ITSStrings.MoreOrLessMedium,
            ITSStrings.Medium,ITSStrings.MoreOrLessHigh,ITSStrings.High,ITSStrings.VeryHigh };

        static LevelCalculator()
        {
            InitFuzzyEngine();
        }
        /// <summary> 
        /// </summary>
        /// <param name="i">每个问题的重要性</param>
        /// <param name="c">每个问题的复杂度</param>
        /// <param name="d">每个问题的难度</param>
        public LevelCalculator(double i,double c,double d)
        {
            if (i < 0 || i > 1)
                throw new Exception("问题的重要性系数必须在[0,1]之间！");
            if (c < 0 || c > 1)
                throw new Exception("问题的复杂性系数必须在[0,1]之间！");
            if (d < 0 || d > 1)
                throw new Exception("问题的难度系数必须在[0,1]之间！");

            double[] iv = FuzzifyImportance(i).ToArray();
            double[] cv = FuzzifyComplexity(c).ToArray();
            double[] dv = FuzzifyDifficulty(d).ToArray();

            double [] level = new double []
            {
                V0(iv,cv,dv),
                V1(iv,cv,dv),
                V2(iv,cv,dv),
                V3(iv,cv,dv),
                V4(iv,cv,dv),
                V5(iv,cv,dv),
                V6(iv,cv,dv)                
            };
            for (int k = 0; k < _levelFuzzySet.Length; k++)
                _levelDict.Add(_levelFuzzySet[k], level[k]);
        }

        public Dictionary<string,double> LevelDictionary
        {
            get { return _levelDict; }
        }

        private static void InitFuzzyEngine()
        {
            _fuzzyEngine = new FuzzyEngineFactory().Default();

            //为学习成绩(learning performance)创建模糊集的成员关系函数(MF)
            _levelFuzzyVar = new LinguisticVariable("Level");
            _levelMFs = new List<IMembershipFunction>()
            {
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[0],0,0,0.15),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[1],0,0.15,0.3),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[2],0.175,0.325,0.475),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[3],0.35,0.5,0.65),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[4],0.525,0.675,0.825),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[5],0.7,0.85,1),
                _levelFuzzyVar.MembershipFunctions.AddTriangle(_levelFuzzySet[6],0.85,1,1)
            };

            //用于输入的importance,complexity and difficulty
            _fuzzyVar = new LinguisticVariable("FuzzyVariable");
            _mfs = new List<IMembershipFunction>()
            {
                _fuzzyVar.MembershipFunctions.AddTrapezoid(_fuzzySet[2], 0, 0, 0.2, 0.4),
                _fuzzyVar.MembershipFunctions.AddTriangle(_fuzzySet[1], 0.2, 0.5, 0.8),
                _fuzzyVar.MembershipFunctions.AddTrapezoid(_fuzzySet[0], 0.6, 0.8, 1,1)
            };

        }

        private IEnumerable<double> FuzzifyImportance(double imp)
        {
            foreach (var e in _mfs)
                yield return e.Fuzzify(imp);
        }
        private IEnumerable<double> FuzzifyComplexity(double com)
        {
            foreach (var e in _mfs)
                yield return e.Fuzzify(com);
        }

        private IEnumerable<double> FuzzifyDifficulty(double diff)
        {
            foreach (var e in _mfs)
                yield return e.Fuzzify(diff);
        }

        private double V0(double[] i,double[] c,double[] d)
        {
            return x1 * i[0] + x2 * c[0] + x3 * d[0];
        }

        private double V1(double[] i, double[] c, double[] d)
        {
            List<double> tmp = new List<double>()
            {
                x1*i[1]+x2*c[0]+x3*d[0],
                x1*i[0]+x2*c[1]+x3*d[0],
                x1*i[0]+x2*c[0]+x3*d[1]
            };

            return tmp.Max();
        }
        private double V2(double[] i, double[] c, double[] d)
        {
            List<double> tmp = new List<double>()
            {
                x1*i[1]+x2*c[1]+x3*d[0],
                x1*i[1]+x2*c[0]+x3*d[1],
                x1*i[0]+x2*c[1]+x3*d[1],
                x1*i[0]+x2*c[0]+x3*d[2],
                x1*i[0]+x2*c[2]+x3*d[0],
                x1*i[2]+x2*c[0]+x3*d[0],
            };
            return tmp.Max();
        }
        private double V3(double[] i, double[] c, double[] d)
        {
            List<double> tmp = new List<double>()
            {
                x1*i[1]+x2*c[1]+x3*d[1],
                x1*i[0]+x2*c[1]+x3*d[2],
                x1*i[1]+x2*c[2]+x3*d[0],
                x1*i[2]+x2*c[0]+x3*d[1],
                x1*i[1]+x2*c[0]+x3*d[2],
                x1*i[0]+x2*c[2]+x3*d[1],
                x1*i[2]+x2*c[1]+x3*d[0]
            };
            return tmp.Max();
        }
        private double V4(double[] i, double[] c, double[] d)
        {
            List<double> tmp = new List<double>()
            {
                x1*i[1]+x2*c[1]+x3*d[2],
                x1*i[1]+x2*c[2]+x3*d[1],
                x1*i[2]+x2*c[1]+x3*d[1],
                x1*i[2]+x2*c[2]+x3*d[0],
                x1*i[2]+x2*c[0]+x3*d[2],
                x1*i[0]+x2*c[2]+x3*d[2]
            };
            return tmp.Max();
        }
        private double V5(double[] i, double[] c, double[] d)
        {
            List<double> tmp = new List<double>()
            {
                x1*i[2]+x2*c[2]+x3*d[1],
                x1*i[2]+x2*c[1]+x3*d[2],
                x1*i[1]+x2*c[2]+x3*d[2],
            };
            return tmp.Max();
        }
        private double V6(double[] i, double[] c, double[] d)
        {
            List<double> tmp = new List<double>()
            {
                x1*i[2]+x2*c[2]+x3*d[2]
            };
            return tmp.Max();
        }
    }
}
