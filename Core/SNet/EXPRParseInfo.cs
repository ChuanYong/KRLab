using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 处理EXPR连接语义
    /// </summary>
    public class EXPRParseInfo:ParseInfo
    {
        //EXPR连接的到达节点，赋值的节点，右边
        protected SNNode _rightNode;
        //EXPR连接的起始节点，被赋值的节点，左边
        protected SNNode _leftNode;

        protected string _expr; 

        public string VariableName
        {
            get { return _rightNode.Name; }
        }

        public string ExprStr
        {
            get { return _expr; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">ASSGN连接的起始节点</param>
        /// <param name="net"></param>
        /// <param name="projectType"></param>
        public EXPRParseInfo(SNNode node,SemanticNet net)
        :base(net)
        {
            _leftNode = node;
            _rightNode = net.GetOutgoingDestination(node, SNRational.EXPR);
            _expr = _rightNode.Name;
             
            //List<FormulaElement> elms = EquationKRModuleSNet.TraverseFromNode(_rightNode, net);
            //_expr = EquationKRModuleSNet.CreateFormulaString(_rightNode, elms, net);          

        }

        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            qas = new List<System.Tuple<string, string[]>>();
            string q = "请给出" + _leftNode.Name + "的表达式"; 

            qas.Add(new System.Tuple<string, string[]>(q, new[] { _expr }));
        } 

        public List<string> FindVariables()
        {
            List<string> variables = new List<string>();
            return variables;
        }
        
        public List<string> FindConsts()
        {
            List<string> consts = new List<string>();
            return consts;
        }
    }
}
