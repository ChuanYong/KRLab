using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Analytics;
using Exversion;
using Exversion.Analytics;
using WpfMath;
using WpfMath.Controls;

using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;

using Utilities;

namespace ITS
{
    /// <summary>
    /// 符号计算和公式可视化
    /// </summary>
    public class FormulaParser
    {
        private static Translator _translator = new Translator();
        private static BaseConverter _converter = new AnalyticsTeXConverter();
        private static TexFormulaParser _parser = new TexFormulaParser();

        public static string StrToLatex(string str)
        {
            return _converter.Convert(str);
        }

        public static byte[] StrToBytes(string str)
        {
            string latex = _converter.Convert(str);
            TexFormula formula = _parser.Parse(latex);
            byte[] bs = formula.RenderToPng(20.0, 0.0, 0.0, "Arial");

            return bs;
        }

        public static double CalculateFormula(string expr,
            System.Tuple<string, float> x1,
            System.Tuple<string, float> x2,
            System.Tuple<string, float> x3)
        {
            _translator.Add(x1.Item1, x1.Item2);
            _translator.Add(x2.Item1, x2.Item2);
            _translator.Add(x3.Item1, x3.Item2);

            Analytics.Formulae.Formula f = _translator.BuildFormula(expr);
            return (double)f.Calculate();
        }
    }
}
