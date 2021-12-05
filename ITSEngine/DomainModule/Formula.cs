using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.DataStructures.Lists;
using KRLab.Core.DataStructures.Graphs;
using KRLab.Core.Algorithms.Graphs;
using KRLab.Core.SNet;

using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;



namespace ITS.DomainModule
{
    public class Formula
    {
        private string _name; 
        private string _str;
        private Expr _expr;   

        public string ExprString
        { get { return _expr.ToString(); } }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 由字符串str构成一个表达式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="name"></param>
        public Formula(string str,string name="")
        {
            _name = name;
            _str = str;
            _expr = Expr.Parse(str);
        }



        /// <summary>
        /// 将方程字符串转换为字节数组，
        /// 用于GUI显示
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            string latex = FormulaParser.StrToLatex(ToString()); 
            byte[] bs = FormulaParser.StrToBytes(latex);

            return bs;
        }


        /// <summary>
        /// 判断symbol是否是该表达式中的符号
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool IsASymbol(string symbol)
        {
            return _expr.ToString().Contains(symbol);
        }



        public bool CompareFormula(string str)
        {
            //string[] fs = str.Split(new char[] { '=' });
            //if (fs.Length != 2)
            //    return false;

            //List<Equation> fList = EquationMaker.Equations;

            //foreach (Equation fl in fList)
            //{
            //    if ((fl.LeftString == fs[0] && fl.RightString == fs[1])
            //        || (fl.LeftString == fs[1] && fl.RightString == fs[0]))

            //        return true;
            //}

            return false;
        }

        public override string ToString()
        {
            return _str;
        }

    }
}
