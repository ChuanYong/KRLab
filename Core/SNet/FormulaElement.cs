using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core;
using KRLab.Core.SNet;

using KRLab.Core.DataStructures.Lists;
using KRLab.Core.DataStructures.Graphs;
using KRLab.Core.Algorithms.Graphs;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 描述一个公式中某个操作符,比如下面的，包括一个操作符结点和若干个操作数结点
    /// /-------\         /-------\
    /// |   x   |         |   y   |
    /// \-------/         \-------/
    ///     \               /
    ///      \            /
    ///        /------\
    ///        |  +   |
    ///        \------/
    /// </summary>
    public class FormulaElement
    { 
        private SNNode _node;          
        private List<SNEdge> _inEdges;
        private List<SNEdge> _outEdges;
        private int _index;

        public SNNode Node
        {
            get { return _node; }
            set { _node = value; }
        }        
        public int Index
        {
            get { return _index; }
        }

        public List<SNEdge> InEdges
        {
            get { return _inEdges; } 
        }
        public List<SNEdge> OutEdges
        {
            get { return _outEdges; }
        }         

        public List<SNNode> OutNodes
        {
            get
            {
                List<SNNode> nodes = new List<SNNode>();
                foreach (var edge in OutEdges)
                    nodes.Add(edge.Destination);
                return nodes;
            }
        }
        public FormulaElement()
        {
            _inEdges = new List<SNEdge>();
            _outEdges = new List<SNEdge>();
        }

        /// <summary> 
        /// 表示一个运算，operatorNode是运算符结点
        /// resultNode是运算结果结点，operandEdges
        /// 指向操作数的边，要注意，边的数量一般是两个，比如“-,/,^" 运算,
        /// 也可以是三个以上，比如"*,+"运算，如果操作数是一个，则运算符直接加到
        /// 操作数上，比如“-”排在第一的必须是ACT类型，第二必须是ACTED类型 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="i"></param>
        /// <param name="outEdges"></param>
        /// <param name="inEdges"></param>
        public FormulaElement(SNNode node,int i,List<SNEdge> outEdges, List<SNEdge> inEdges):this()
        {
            _node =node; 
            _index = i;
            _outEdges = outEdges;
            _inEdges = inEdges; 
        }

        /// <summary>
        /// 判断other的操作符结点与自身的操作符节点直接相连接，如果相连接,返回1，
        /// 否则返回0 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int IsNeiboured(FormulaElement other)
        { 
            foreach(var edge in OutEdges)
            {
                if (other.InEdges.Contains(edge))
                    return 1; 
            }
            return -1;
        }

        /// <summary>
        /// 获取结点排列顺序，比如，x*y,x/y,
        /// </summary>
        public List<SNEdge> GetArrayedOutEdges()
        {
            if (!SemanticNet.IsOperationNode(_node))
                return null;

            List<SNEdge> edges = new List<SNEdge>();

            if(_node.Name=="-")
            {
                if (OutEdges.Count == 1)
                {
                    edges.Add(OutEdges[0]);
                }
                else if (OutEdges.Count == 2)
                {
                    if (_outEdges[0].Rational.Rational == SNRational.ACT)
                    {
                        edges.Add(_outEdges[0]);
                        edges.Add(_outEdges[1]);
                    }
                    else
                    {
                        edges.Add(_outEdges[1]);
                        edges.Add(_outEdges[0]);
                    }
                }
                else
                    throw new Exception("减号运算设计错误！");
            }
            else if (_node.Name == "/")
            {
                if(OutEdges.Count!=2)
                    throw new Exception("除号运算设计错误！");

                if (_outEdges[0].Rational.Rational == SNRational.ACT)
                {
                    edges.Add(_outEdges[0]);
                    edges.Add(_outEdges[1]);
                }
                else
                {
                    edges.Add(_outEdges[1]);
                    edges.Add(_outEdges[0]);
                } 
            }
            else if (_node.Name == "^")
            {
                if (OutEdges.Count != 2)
                    throw new Exception("^运算设计错误！");

                if (OutEdges[0].Rational.Rational == SNRational.ACT)
                {
                    edges.Add(_outEdges[1]);
                    edges.Add(_outEdges[0]);
                }
                else
                {
                    edges.Add(_outEdges[0]);
                    edges.Add(_outEdges[1]);
                }
            }
            else
            {
                foreach (var edge in OutEdges)
                {
                    edges.Add(edge); 
                } 
            }
            return edges;
        }

        public SNEdge FindEdgeInPath(SNNode s,SNNode d)
        {
            foreach (var edge in OutEdges)
            {
                if (edge.Source == s && edge.Destination == d)
                    return edge;
            }
            return null;
        }
    }
}
