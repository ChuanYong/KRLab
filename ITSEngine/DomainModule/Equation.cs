using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;
using Utilities;
using Analytics;
using Analytics.Syntactic;

using Expr = MathNet.Symbolics.Expression;
using MathNet.Symbolics;

namespace ITS.DomainModule
{ 
    public class Equation
    {

        //方程的等号结点
        private EquElem _equElm;

        private BaseExpression _leftFormula;
        private BaseExpression _rightFormula; 

        //方程的名称
        public string Name
        {
            get { return _equElm.EquNode.Name; }
        }

        public EquElem EquElem
        {
            get { return _equElm; }
        }
         
        public string LeftString
        {
            get { return _leftFormula.ToString(); }
        }
        public string RightString
        {
            get { return _rightFormula.ToString(); }
        }

        public List<string> Consts
        {
            get
            {
                List<string> list = new List<string>();
                foreach(var node in _equElm.ConstNodes)
                {
                    list.Add(node.Name);
                }
                return list;
            }
        }

        public List<string> ConstVariables
        {
            get
            {
                List<string> list = new List<string>();
                foreach(var node in _equElm.ConstVarNodes)
                {
                    list.Add(node.Name);
                }
                return list;
            }
        }

        public List<string> Variables
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var node in _equElm.VariableNodes)
                {
                    list.Add(node.Name);
                }
                return list;
            }
        } 

        public Equation(EquElem equElm)
        {
            _equElm = equElm;

            _leftFormula = new SumExpression("left", new List<string>(), new List<BaseExpression>());
            _rightFormula = new SumExpression("righ",new List<string>(),new List<BaseExpression>());

            BaseExpression x = new VariableExpression("x");
            _leftFormula =  2 * x ^ 2 + 8 * x + 1;
            _rightFormula = 2 * x;

            BaseExpression v0 = new VariableExpression("a") + LiteralExpression.Make(1);
            BaseExpression v1 = LiteralExpression.Make(2);
            v0 = v0 * new VariableExpression("b");

            PowerExpression p = (PowerExpression)PowerExpression.MakePower(v0, v1);
            //p.Simplify();
            BaseExpression p1 = p.Expand();
            //p1.Simplify();

            if (p.CanExpand())
            {
                Console.WriteLine(p1.Reconstruct());
            }
        } 
         

        public bool CompareEquation(string str)
        {
            string[] fs = str.Split(new char[] { '=' });
            if (fs.Length != 2)
                return false;

            //List<Equation> fList = EquationMaker.Equations;

            //foreach (Equation fl in fList)
            //{
            //    if ((fl.LeftString == fs[0] && fl.RightString == fs[1])
            //        || (fl.LeftString == fs[1] && fl.RightString == fs[0]))

            //        return true;
            //}

            return false;
        }

        /// <summary>
        /// 将方程字符串转换为字节数组，
        /// 用于GUI显示
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {

            string latex = FormulaParser.StrToLatex(LeftString + "=");
            latex += FormulaParser.StrToLatex(RightString);
            byte[] bs = FormulaParser.StrToBytes(latex);

            return bs;
        }

        public override string ToString()
        {
            return LeftString + "=" + RightString;
        } 
    }
}
