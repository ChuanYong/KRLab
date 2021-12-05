using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 用于描述问题的背景
    /// </summary>
    public class StoryKRModuleSNet:KRModuleSNet
    { 
        public StoryKRModuleSNet(SemanticNet net):base(net,"故事")
        {

        }

        public override void CheckAndInit()
        {
            base.CheckAndInit();
        }


        /// <summary>
        /// 在语义网中查找指定题目topic的问题网络。问题网络是用于提问的语义网，该语义网
        /// 有一个名为“问题”的结点，
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public List<SemanticNet> CreateStoryNets(string topic)
        {
            List<SNNode> nodes = Net.GetNodes("故事");
            if (nodes.Count == 0)
                throw new Exception("在" + topic + "中没有找到名称为'故事'的结点");
            List<SemanticNet> nets = new List<SemanticNet>();
            foreach (var node in nodes)
            {
                List<SNEdge> neighbors = Net.GetIncomingEdges(node);
                foreach (var tp in neighbors)
                {
                    if (tp.Rational.Rational == SNRational.ISP &&
                        GetDeptNode(tp.Source).Name == topic)
                    {
                        SemanticNet questionNet = Net.CreateSubNetWithAllNeighbors(node, topic);
                        nets.Add(questionNet);
                        break;
                    }
                }
            }
            return nets;
        }


        public SNNode GetDeptNode(SNNode storylineNode)
        {
            if (IsAStorylineNode(storylineNode))
            {
                List<SNEdge> edges = Net.GetOutgoingEdges(storylineNode);
                foreach (var edge in edges)
                {
                    if (edge.Rational.Rational == SNRational.DEPT)
                        return edge.Destination;
                }
            }
            return null;
        }

        public bool IsAStorylineNode(SNNode node)
        {
            List<SNEdge> edges = Net.GetOutgoingEdges(node);
            foreach (var edge in edges)
            {
                if ((edge.Rational.Rational == SNRational.ISP && edge.Destination.Name == "故事"))
                    return true;
            }
            return false;
        }

    }
}
