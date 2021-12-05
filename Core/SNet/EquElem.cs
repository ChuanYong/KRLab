using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Analytics.Formulae;
using KRLab;
using Utilities;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 用于描述单个公式的语义网
    /// </summary>
    public class EquElem 
    {
        //方程类型名称，比如一元一次方程，一元二次方程等
        protected SNNode _equTypeNode;

        //方程名称结点
        private SNNode _equNode;
        private SNNode _leftStart;
        private SNNode _rightStart; 

        /// <summary>
        /// 公式对应的子语义网
        /// </summary>
        private SemanticNet _net;

        //方程的名称
        public string Name
        {
            get { return _equNode.Name; }
        }

        public SemanticNet Net
        {
            get { return _net; }
        }

        public SNNode EquTypeNode
        {
            get 
            {
                Debug.Assert(_equTypeNode != null);
                return _equTypeNode;
            }
        } 

        public SNNode EquNode
        {
            get { return _equNode; } 
        }  

        public List<SNNode> ConstNodes
        {
            get { return _net.GetConstNodes(); }
        }
        public List<SNNode> ConstVarNodes
        {
            get { return _net.GetConstVarNodes(); }
        }
        public List<SNNode> VariableNodes
        {
            get { return _net.GetVariableNodes(); }
        }

        public SNNode LeftStart
        {
            get { return _leftStart; }
        }
        public SNNode RightStart
        {
            get { return _rightStart; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="equNode">表明公式名的节点</param>
        /// <param name="net">公式对应的子语义网</param>
        public EquElem(SNNode equNode,SemanticNet net)
        {
            _equNode = equNode;
            _net = net;

            ATTParseInfo att = new ATTParseInfo(_equNode, _net);
            _leftStart = att.GetAttValueNode("左边");
            _rightStart = att.GetAttValueNode("右边");
        }          

        /// <summary>
        /// 根据输入的操作单元，对公式语义网进行变化
        /// </summary>
        /// <param name="oe"></param>
        public void Transform(ProcStepParseInfo oe)
        {
            //SNNode node = GetNode(oe.OprndNode.Name);
            //SNNode oprNode = new SNNode(oe.OprNode.Name);

            /////将指向node的连接指向新的节点oprNode
            //List<SNEdge> edges = _net.GetIncomingEdges(node);
            //foreach(var edge in edges)
            //{
            //    edge.Destination = oprNode;
            //}

            //_net.AddNode(oprNode);
            //_net.AddEdge(new SNEdge(oprNode, node, new SNRational(SNRational.OPRND,"","","","","")));

            //foreach(var nd in oe.OpredNodes)
            //{
            //    node = GetNode(nd.Name);
            //    _net.AddEdge(new SNEdge(oprNode, node, new SNRational(SNRational.OPRED,"","","","","")));
            //}
        }
         

        /// <summary>
        /// 获取公式中的变量节点，由于在同一个公式语义网中允许用同名的节点
        /// 表示同一个变量，所以，获取的节点可以有多个
        /// </summary>
        /// <param name="net">对应一个公式的子语义网</param>
        /// <returns></returns>
        public List<SNNode> GetVariableNodes(string varName)
        {
            List<SNNode> results = new List<SNNode>();
            List<SNNode> nodes = _net.GetNodes(ITSStrings.Variable);
            if (nodes.Count == 0)
                return results;

            foreach (var node in nodes)
            {
                List<SNNode> tmps = _net.GetIncomingSources(node, SNRational.IS, SNRational.ISA);
                foreach (var nd in tmps)
                {
                    if (nd.Name == varName)
                        results.Add(nd);
                }
            }
            return results;
        }

        /// <summary>
        /// 在一个公式语义网中可以用同名的节点表示同一个常变量，
        /// 所以，获取的节点可以有多个。
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public List<SNNode> GetConstVariableNodes(string varName)
        {
            List<SNNode> results = new List<SNNode>();
            List<SNNode> nodes = _net.GetNodes(ITSStrings.ConstVar);
            if (nodes.Count == 0)
                return results;

            foreach (var node in nodes)
            {
                List<SNNode> tmps = _net.GetIncomingSources(node, SNRational.IS, SNRational.ISA);
                foreach(var nd in tmps)
                {
                    if(nd.Name==varName)
                        results.Add(nd);
                }
            }
            return results;
        }

        /// <summary>
        /// 在一个公式语义网中可以用同名的节点表示同一个常量，
        /// 所以，获取的节点可以有多个
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public List<SNNode> GetConstNodes(string varName)
        {
            List<SNNode> results = new List<SNNode>();
            List<SNNode> nodes = _net.GetNodes(ITSStrings.Const);
            if (nodes.Count == 0)
                return results;

            foreach (var node in nodes)
            {
                List<SNNode> tmps = _net.GetIncomingSources(node, SNRational.IS, SNRational.ISA);
                foreach (var nd in tmps)
                {
                    if (nd.Name == varName)
                        results.Add(nd);
                }
            }
            return results;
        }

        /// <summary>
        /// 对方程进行多项式展开并合项之后，再调用该函数，所以公式中对应次数的变量系数只有
        /// 只有一个。
        /// 获取公式语义网中方程的某个变量的系数节点，比如
        /// n=0,常数项
        /// n=1,一次项系数，
        /// n=2,二次项系数，
        /// n=3,三次项系数，
        /// ！！！！！！目前该函数只适用于不需要进行括号展开！！！！！
        /// </summary>
        /// <param name="net">是某个公式的子语义网</param> 
        /// <param name="n"></param>
        /// <returns>true+result=null, 系数为1</returns>
        public SNNode GetTermCoef(int n,string varName)
        {
            List<SNNode> nodes = GetVariableNodes(varName);  

            foreach (var node in nodes)
            {
                SNNode node1 = _net.GetIncomingSource(node, SNRational.OPRED);
                if(node1.Name=="^")
                {
                    SNNode node2 = _net.GetOutgoingDestination(node1, SNRational.OPRND);
                    int k;
                    if(int.TryParse(node2.Name,out k) && k==n)
                    {
                        SNNode nd = _net.GetIncomingSource(node1, SNRational.OPRND);
                        if (nd != null && nd.Name=="*")
                        {
                            return _net.GetOutgoingOtherDest(nd,node1, SNRational.OPRND);
                        }
                        if(nd!=null && nd.Name=="/")
                        {
                            return new SNNode("1/"+nd.Name);
                        }
                    }
                }
            }

            return null;
        }

        #region operation 
        /// <summary>
        /// 通过改变语义连接的结构，展开公式中的括号
        /// </summary>
        public List<FormulaElement> ExpandBrackets(SNNode source, SemanticNet net)
        { 
            var visited = new HashSet<SNNode>();
            var queue = new DataStructures.Lists.Queue<SNNode>();
            int i = 0;
            visited.Add(source);
            queue.Enqueue(source);

            while (!queue.IsEmpty)
            {
                var current = queue.Dequeue();
                i++;

                //if (SemanticNet.IsOperationNode(current) || net.IsAssignedNode(current))
                //    operatorElements.Add(element);

                var neighbors = net.Neighbours(current);
                foreach (var adjacent in neighbors)
                {
                    if (!visited.Contains(adjacent))
                    {
                        //SNEdge edge = net.GetEdge(current, adjacent) as SNEdge;
                        visited.Add(adjacent);
                        queue.Enqueue(adjacent);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 判断输入的运算符node是否会有括号参与，是否能展开， 
        /// 当一个运算符相连接的是运算符，并且这个连接的运算符
        /// 是+或-， 则可以进一步展开
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool CanExpanded(SNNode node)
        {
            SNNode oprnNode = _net.GetOutgoingDestination(node, SNRational.OPRND);
            List<SNNode> opredNodes = _net.GetOutgoingDestinations(node, SNRational.OPRED); 

            if(node.Name=="*")
            {
                if (oprnNode.Name == "+" || oprnNode.Name == "-")
                    return true;
                foreach (var nd in opredNodes)
                {
                    if (nd.Name == "+" || nd.Name == "-")
                        return true;
                }
            }
            else if(node.Name=="/")
            {
                if (oprnNode.Name == "+" || oprnNode.Name == "-")
                    return true;
            }
            else if(node.Name=="-")
            {
                foreach (var nd in opredNodes)
                {
                    if (nd.Name == "+" || nd.Name == "-")
                        return true;
                }
            }
            else if(node.Name=="^")
            {
                int i;
                if(int.TryParse(oprnNode.Name,out i) && SemanticNet.IsOperationNode(opredNodes[0]))
                {
                    return true;
                }
            }

            return false;
        }


        #endregion

    }
}
