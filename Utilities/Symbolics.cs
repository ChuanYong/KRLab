using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace Utilities
{
    public static class Symbolics
    {
        /// <summary>
        /// 计算formula表达式的值，表达式中的变量的值在第二个参数中
        /// 给定。
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public static double Calculate(string formula,Dictionary<string,FloatingPoint> symbols)
        {
            Expr str = Infix.ParseOrThrow(formula);
            return Math.Round(str.Evaluate(symbols).RealValue, 2);
        }

        /// <summary>
        /// 确保formula中只有一个变量
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="x"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Calculate(string formula,string x,FloatingPoint v)
        {
            Expr str = Expr.Parse(formula);
            Dictionary<string, FloatingPoint> xv = new Dictionary<string, FloatingPoint>();
            xv.Add(x, v);
            return Math.Round(str.Evaluate(xv).RealValue,2);
        }

        /// <summary>
        /// 确保formula中只有两个变量
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="x"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double Calculate(string formula, 
            string x0, FloatingPoint v0,
            string x1,FloatingPoint v1)
        {

            Expr str = Expr.Parse(formula);
            Dictionary<string, FloatingPoint> xv = new Dictionary<string, FloatingPoint>();
            xv.Add(x0, v0);
            xv.Add(x1, v1);
            return Math.Round(str.Evaluate(xv).RealValue, 2);
        }
    }
}
