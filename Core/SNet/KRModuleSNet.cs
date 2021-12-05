using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utilities;
using ITSText;
 

namespace KRLab.Core.SNet
{
    /************************************************************************
     * 所有的语义网是按知识类型进行建模。在KCNames类规划了多个类型，请参见之。
     * 根据KCNames类中的知识类型，由本抽象类派生了对应的语义模板类。
     * 一个语义网对应某个知识类型中的一个知识点或学习课题。在语义网建模时必须指出
     * 知识点或学习课题对应的知识类型。
     *                          【概念】 
     *                             /|\     
     *                              |IS
     *                              | 
     *                           【课题】                         
     *
     **************************************************************************/
    public class KRModuleSNet
    {
        protected SemanticNet _net;
        protected string _krType;
        protected SNNode _krNode;
        protected SNNode _topicNode;
        protected SNNode _storyNode; 

        public SemanticNet Net
        {
            get { return _net; }
        }

        public string Topic
        {
            get
            {
                if (_topicNode == null)
                    return _net.Topic;
                else
                    return _topicNode.Name;
            }
        }

        public string Story
        {
            get
            {
                if (_storyNode == null)
                {
                    return string.Empty;
                }
                else
                    return _storyNode.Name;
            }
        }

        public List<SNNode> Nodes
        {
            get
            {
                return Net.Vertices.ToList();
            }
        }

        public virtual string KRType
        {
            get { return null; }
        }

        public KRModuleSNet(SemanticNet net,string type=null)
        {
            _krType = type;
            _net = net;

            CheckAndInit();
        } 

        public static void Check(List<IEntity> entities, List<Relationship> relations,
            Action<bool, string> callback)
        {
            List<IEntity> probNodes = new List<IEntity>();
            foreach(var node in entities)
            {
                if(node.Name=="问题")
                {
                    probNodes.Add(node);
                }
            }

            foreach(var node in probNodes)
            {
                bool ok0 = false;
                bool ok1 = false;
                bool ok2 = false;
                bool ok3 = false;
                bool ok4 = false;
                bool ok5 = false;
                foreach(var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if(snr.First==node && snr.SNRelationshipType.ToString()==SNRational.ATT &&
                        snr.Second.Name=="文字描述")
                    {
                        ok0 = true;
                    }    
                    if(snr.First==node && snr.SNRelationshipType.ToString()==SNRational.ATT &&
                        snr.Second.Name.Contains("提问"))
                    {
                        ok1 = true;
                    }
                    if(snr.First.Name.Contains("提问") && snr.SNRelationshipType.ToString()==SNRational.ATT && 
                        snr.Second.Name=="答案")
                    {
                        ok2 = true;
                    }
                    if(snr.First.Name.Contains("提问") && snr.SNRelationshipType.ToString()==SNRational.VAL)
                    {
                        ok3 = true;
                    }
                    if(snr.First.Name=="答案"&& snr.SNRelationshipType.ToString()==SNRational.VAL)
                    {
                        ok4 = true;
                    }
                    if(snr.First.Name=="文字描述" && snr.SNRelationshipType.ToString()==SNRational.VAL)
                    {
                        ok5 = true;
                    }

                }
                if(!ok0)
                {
                    callback(false, "必须提供<问题>的<文字描述>，并用ATT指向");
                    return;
                }
                if(!ok1)
                {
                    callback(false, "必须提供<问题>的<提问>，并用ATT指向");
                    return;
                }
                if(!ok2)
                {
                    callback(false, "必须有<提问>的<答案>，并用ATT指向");
                    return;
                }
                if(!ok5)
                {
                    callback(false, "必须有<文字描述>内容，用VAL指向");
                    return;
                }
                if(!ok3)
                {
                    callback(false, "必须有<提问>的内容，用VAl指向");
                    return;
                }
                if(!ok4)
                {
                    callback(false, "必须有<答案>的内容，用VAL指向");
                    return;
                }
            }


            bool isOk = false;  
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if(entities.Contains(snr.First) || entities.Contains(snr.Second))
                {
                    isOk = true;
                }

                if (snr.SNRelationshipType.ToString() == SNRational.TIME)
                {
                }
                else if (snr.SNRelationshipType.ToString() == SNRational.HAS)
                {
                }

            }

            if(entities.Count!=0 && !isOk)
            {
                callback(false, "有孤立的节点，如果是不需要的，请删除！否则需要与其他节点连接！");
                return;
            }

        }

        public virtual void CheckAndInit()
        {
            if (Net == null)
            {
                throw new NetException("语义网不存在！");
            }

            _topicNode = GetTopicNode();
            _storyNode = GetStory(_topicNode);
        } 

        public SNNode GetTopicNode()
        {
            _krNode = Net.FastGetNode(_krType);

            List<SNEdge> edges = Net.GetIncomingEdges(_krNode, SNRational.IS);
            if (edges.Count == 1)
                return edges[0].Source;
            else
            {
                foreach (var edge in edges)
                {
                    if (edge.Rational.Label == "主题")
                        return edge.Source;
                }
            }

            return null;
        }  

        /// <summary>
        /// 如果node有父节点，并且父节点是知识类型结点，返回true
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual SNNode HasKRTypeNode(SNNode node,out string type)
        {
            foreach(var str in KCNames.Names)
            {
                var father = Net.GetOutgoingDestination(node,SNRational.IS);
                if(father!=null && father.Name==str)
                {
                    type = str;
                    return father;
                }

            }
            type = null;
            return null;
        }

        public virtual List<SNNode> GetDeptTopicNodes(SNNode topicNode)
        {
            if (topicNode == null)
                return new List<SNNode>();

            List<SNNode> nodes = GetKCNodes();
            List<SNNode> deptNodes = new List<SNNode>();
            foreach(var node in nodes)
            {
                deptNodes.AddRange(Net.GetIncomingSources(node, SNRational.IS));
            }

            //deptNodes包含了topicNode本身，要移除
            deptNodes.RemoveAll(target => target.Name == topicNode.Name); 

            return deptNodes;
        }

        /// <summary>
        /// 获取语义网中所有的知识类型结点，也就是结点名称为KCNames中的知识类型名称
        /// </summary>
        /// <returns></returns>
        public List<SNNode> GetKCNodes()
        {
            List<SNNode> nodes = new List<SNNode>();
            foreach(var node in Net.Vertices)
            {
                if(KCNames.Names.Contains(node.Name))
                {
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        public List<SNNode> GetKCNodes(SNNode node)
        {
            if (node == null)
                return new List<SNNode>();
            return Net.GetOutgoingDestinations(node, SNRational.IS);
        }

        public bool HasConnection(SNNode from,SNNode to,string type)
        {
            List<SNNode> nodes = Net.GetOutgoingDestinations(from, type);
            if (nodes.Contains(to))
                return true;
            else
                return false;
        }


        /// <summary>
        /// 判断node是否与知识类型结点关联
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public virtual SNNode IsAssociatedKRTypeNode(SNNode node,out string type)
        {
            foreach (var str in KCNames.Names)
            {
                var father = Net.GetOutgoingDestination(node, SNRational.ASSOC);
                if (father != null && father.Name==str)
                {
                    type = str;
                    return father;
                }

            }
            type = null;
            return null;
        }

        public List<SNEdge> GetEdges(SNNode from,SNNode to)
        {
            List<SNEdge> edges = Net.GetOutgoingEdges(from);
            return edges;
        }

        public SNEdge GetEdge(SNNode from,SNNode to)
        {
            List<SNEdge> edges = Net.GetOutgoingEdges(from);
            foreach(var edge in edges)
            {
                if(edge.Destination==to)
                {
                    return edge;
                }
            }
            return null;
        }


        public SNNode GetStory(SNNode topicNode)
        {
            SNNode introNode = Net.GetASSOCNode(topicNode, "简要说明");
            return introNode;
        }

        /// <summary>
        /// 查找从startVertex开始的，连接方向是发出，不是进入。
        /// </summary>
        /// <param name="StartVertex"></param>
        /// <returns></returns>
        public HashSet<SNNode> VisitAll(SNNode startVertex) 
        {
            // Check if graph is empty
            if (Net.VerticesCount == 0)
                throw new Exception("Graph is empty!");

            // Check if graph has the starting vertex
            if (!Net.HasVertex(startVertex))
                throw new Exception("Starting vertex doesn't belong to graph.");

            var visited = new HashSet<SNNode>();
            var stack = new Stack<SNNode>(Net.VerticesCount);

            stack.Push(startVertex);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Contains(current))
                {
                    // DFS VISIT NODE STEP 
                    visited.Add(current);

                    // Get the adjacent nodes of current
                    foreach (var adjacent in Net.Neighbours(current))
                        if (!visited.Contains(adjacent))
                            stack.Push(adjacent);
                }
            }
            return visited;
        }


        public HashSet<SNNode> VisitAll(SNNode startVertex, string relationType)
        {
            // Check if graph is empty
            if (Net.VerticesCount == 0)
                throw new Exception("Graph is empty!");

            // Check if graph has the starting vertex
            if (!Net.HasVertex(startVertex))
                throw new Exception("Starting vertex doesn't belong to graph.");

            var visited = new HashSet<SNNode>();
            var stack = new Stack<SNNode>(Net.VerticesCount);

            stack.Push(startVertex);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Contains(current))
                {
                    // DFS VISIT NODE STEP 
                    visited.Add(current);

                    // Get the adjacent nodes of current
                    foreach (var adjacent in Net.AllNeighbours(current))
                        if (!visited.Contains(adjacent))
                            stack.Push(adjacent);
                }
            }
            return visited;
        }


        /// <summary>
        /// 获取语义网中的主题结点，用于网络制作过程中的网络语法检查。
        /// </summary>
        /// <param name="topNode"></param>
        /// <param name="entities"></param>
        /// <param name="relations"></param>
        /// <returns></returns>
        public static IEntity GetTopicNode(IEntity topNode, List<IEntity> entities, List<Relationship> relations)
        {
            IEntity topicNode = null;

            List<IEntity> tmp = new List<IEntity>();

            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;

                if (snr.SNRelationshipType.ToString() == SNRational.KTYPE&& snr.Second == topNode)//
                    tmp.Add(snr.First);
            }
            if (tmp.Count > 1)
            {
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;

                    if (snr.SNRelationshipType.ToString() == SNRational.KTYPE && snr.Second == topNode && snr.Label == "主题")//
                        topicNode = snr.First;
                }
            }
            if (tmp.Count == 1)
                topicNode = tmp[0];

            return topicNode;
        }

        public static SemanticNet CreateASNet(string topic,string path)
        {
            SNetProject project = new SNetProject();
            project.LoadFromFile(path);

            SemanticNet net=project.GetSNet(topic);
            return net;
        }

    }
}
