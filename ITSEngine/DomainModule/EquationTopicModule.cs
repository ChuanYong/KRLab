using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using KRLab.Core.SNet;
using Utilities;

using Symbolism;

using ITS.MathSolvers;


namespace ITS.DomainModule
{
    public class EquationTopicModule:TopicModule
    {
        protected List<Equation> _equs; 

        public EquationKRModuleSNet SNet
        {
            get { return (EquationKRModuleSNet)_sNet; }
        } 

        public List<string> EquNames
        {
            get
            {
                List<SNNode> nodes = SNet.GetEquNodes();
                List<string> names = new List<string>();
                foreach (var node in nodes)
                    names.Add(node.Name);
                return names;
            }
        }

        public EquationTopicModule(string course,KRModuleSNet net):base(course,net)
        {

        }

        public EquationTopicModule(EquationKRModule krModule, string topic) :
            base(krModule, topic)
        {
            _equs = new List<Equation>();
        } 

        /// <summary>
        /// 获取方程的算法
        /// </summary>
        /// <param name="equName"></param>
        /// <returns></returns>
        public List<string> GetAlgorithms(string equName)
        {
            List<string> strs = new List<string>();
            List<SNNode> equNodes = SNet.GetEquNodes();
            SNNode finded = equNodes.Find(target => target.Name == equName);
            if (finded == null)
                return strs;

            List<SNNode> nodes = SNet.GetAlgorithmNodes(finded);
            foreach (var node in nodes)
                strs.Add(node.Name);
            return strs;
        } 

        public override string ToString()
        {
            return Topic;
        }

        public List<EquElem> GetEquElems()
        {
            return SNet.GetEquElems();
        } 

        /// <summary>
        /// 获取方程的c#类名
        /// </summary>
        /// <param name="equ"></param>
        /// <returns></returns>
        public string GetEquClassName(EquElem equ)
        {
            string type = equ.EquTypeNode.Name;
            if (EquClassNames.ClassNames.ContainsKey(type))
                return EquClassNames.ClassNames[type];
            else
                return null; 
        }

        public Symbolism.Equation CreateEqu(EquElem ee)
        {
            MathObject left = null;
            MathObject right = null;
            Symbolism.Equation equ = new Symbolism.Equation(left, right, Symbolism.Equation.Operators.Equal);
            return equ;
        }
         

        /// <summary>
        /// 从公式语义网中构建公式
        /// </summary>
        protected List<Equation> CreateEquations()
        {
            List<Equation> equs = new List<Equation>();
            List<EquElem> equElms = SNet.GetEquElems();
            foreach (var e in equElms)
            {
                Formula left = CreateFormula(e.LeftStart);
                Formula right = CreateFormula(e.RightStart);
                Equation equ = new Equation(e);
                equs.Add(equ);
            }
            return equs;
        }

        /// <summary>
        /// 从起始节点创建一个表达式
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected Formula CreateFormula(SNNode source)
        { 
            List<FormulaElement> elements = EquationKRModuleSNet.TraverseFromNode(source,SNet.Net);
            string str = EquationKRModuleSNet.CreateFormulaString(source, elements,SNet.Net);
            Formula f = new Formula(str);
            return f;
        }

        public static Formula CreateFormula(SNNode source, SemanticNet net)
        {
            List<FormulaElement> elements = EquationKRModuleSNet.TraverseFromNode(source, net);
            string str = EquationKRModuleSNet.CreateFormulaString(source, elements, net);
            Formula f = new Formula(str);
            return f;
        } 

        public List<MathObject> GetEquExprs(EquElem equ)
        {
            List<MathObject> equs = new List<MathObject>();
            Debug.Assert(equs.Count != 0);
            return equs;
        }

        public override string Parse()
        {
            return base.Parse();
        }

        public override string Parse(string name)
        {
            return base.Parse(name);
        }

    }
}
