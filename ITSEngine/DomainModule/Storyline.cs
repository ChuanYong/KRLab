using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.DataStructures.Lists;
using KRLab.Core.Algorithms.Graphs;

using KRLab.Core;
using KRLab.Core.SNet;



namespace ITS.DomainModule
{
    using Path = List<SNNode>;
    /// <summary>
    /// 一个情节用于描述某个故事中的一个片段，而一个故事由一个或多个情节组成，陈述
    /// 一个复杂的问题。
    /// 情节由一个名为“情节”的结点及其它关联的结点构成。由情节结点发出有至少如下2条属性边：
    /// (1)内容；（2）隐含条件； 
    /// 有一个依赖关系边，用于指明该情节依赖的计算方法。
    /// </summary>
    public class Storyline
    {
        private SNNode _storylineNode;
        private SemanticNet _net;
        private SNNode _preStorylineNode;
        private SNNode _nextSotrylineNode;
        private SNNode _knowledgePointNode;
        private SNNode _contentNode; 
        private List<SNNode> _impliedConditionNodes;

        public SemanticNet Net
        { get { return _net; }}
        public SNNode StorylineNode
        { get { return _storylineNode; } }
        public SNNode ContentNode
        { get { return _contentNode; } }
        public SNNode NextStorylineNode
        { get { return _nextSotrylineNode; } }
        public SNNode PreStorylineNode
        { get { return _preStorylineNode; } }
        public SNNode KnowledgePointNode
        { get { return _knowledgePointNode; } }
        public List<SNNode> ImpliedConditionNodes
        { get { return _impliedConditionNodes; } }


        public Storyline(SemanticNet net, SNNode node)
        { 
            _storylineNode = node;
            _net = net;
            _impliedConditionNodes = new List<SNNode>();

            IEnumerable<SNEdge> allEdges = net.GetOutgoingEdges(_storylineNode); 
            foreach (var edge in allEdges)
            {
                if (edge.Rational.Rational == SNRational.ATT)
                {
                    if(edge.Rational.Label == "隐含条件" ||
                    edge.Rational.Label == "ImpliedCondition")
                    {
                        _impliedConditionNodes.Add(edge.Destination);
                    }
                    else if(edge.Rational.Label=="内容" ||
                        edge.Rational.Label=="content")
                    {
                        _contentNode = edge.Destination;
                    }

                }
                else if(edge.Rational.Rational==SNRational.TIME)
                { 
                    _nextSotrylineNode = edge.Destination;
                }
                else if(edge.Rational.Rational==SNRational.DEPT && (edge.Rational.Label == "知识点" ||
                edge.Rational.Label == "KnowledgePoint"))
                {
                    _knowledgePointNode = edge.Destination;
                }
            }
            allEdges = net.GetIncomingEdges(_storylineNode);
            foreach(var edge in allEdges)
            {
                if(edge.Rational.Rational==SNRational.TIME &&
                    edge.Rational.Label=="时间关系")
                {
                    _preStorylineNode = edge.Source;
                }
            }
        }

        public string GetImpliedInfo()
        {
            if (_impliedConditionNodes.Count == 0)
                return null;

            List<Path> paths = new List<Path>();
            foreach(var node in _impliedConditionNodes)
            {
                List<SNNode> leaves = _net.GetLeafNodes(node);
                foreach(var leaf in leaves)
                {
                    paths.Add(_net.GetAPath(node, leaf));
                }
            }

            string info = "";
            foreach(var path in paths)
            {
                info += PathInfo(path);
                info += "\n";
            }

            return info;
        }

        private string PathInfo(Path path)
        {
            string info = path[0].Name;
            for(int i=1;i<path.Count;i++)
            {
                SNEdge edge = _net.GetEdge(path[i - 1], path[i]) as SNEdge;
                if(edge!=null)
                {
                    info += edge.Rational.Label;
                }
                info += path[i].Name;
            }
            return info;
        }

        public string GetContent()
        {
            List<SNEdge> edges = _net.GetOutgoingEdges(_contentNode);
            int i = new Random().Next(0, edges.Count);
            SNNode actNode = edges[i].Destination;
            SNEdge edge = _net.GetEdge(_contentNode, actNode) as SNEdge; 

            string txt = edge.Rational.Label; 

            System.Tuple<SNNode,SNRational> spaceNode = GetLocationNode(_net,actNode);
            if(spaceNode==null)
                txt = actNode.Name + txt + _contentNode.Name;
            else
                txt=actNode.Name + "在" + spaceNode.Item1.Name+ txt + _contentNode.Name;

            return txt+"。";
        }  

        public static System.Tuple<SNNode, SNRational> GetLocationNode(SemanticNet net, SNNode node)
        {
            List<System.Tuple<SNNode, SNRational>> dics = net.OutgoingNodeRationalNeighbours(node);
            foreach (var elm in dics)
            {
                if (elm.Item2.Rational == SNRational.ATT && (elm.Item2.Label == "地点" || elm.Item2.Label == "location"))
                    return elm;
            }
            return null;
        }

    }
}
