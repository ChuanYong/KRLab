using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
 
using KRLab.Translations;
using KRLab.Core.DataStructures.Graphs;
using KRLab.Core.DataStructures.Dictionaries;
using KRLab.Core.Algorithms.Graphs;
using KRLab.Core.DataStructures.Lists;

using ITSText;
using System.Runtime.InteropServices;
using Utilities;

/*
Semantic Web is a set of technologies that want to make the data on 
the web readable and understood by machines. The domain that semantic 
web is the web. Therefore the URI is in the bottom of the technologies stack.
Semantic Network is a graph model to store the information. If you can model 
your data as a graph with node and edge, you can think it as a semantic network.
Knowledge graph is also a kind of graph model. However, it emphasis the store of
all data on the level of both schema and individual. Therefore, it has no 
essential difference with Semantic network. Knowledge base is an instantiated 
storage of a domain knowledge. You can think it as a database. 
It has not to be a graph model, if you can store the knowledge in some way,
you can call it as knowledge base. 
*/

/*
 * Semantic networks are made up of interlinked nodes connected by arcs.
 * The nodes represent objects,and arcs the relationships between objects. 
 * An example of a semantic network for the zoo animals problem is shown in 
 * Figure below. There are many variants of semantic networks, with its 
 * origins dating from 1909 with the ‘existential graphs’ of Pierce 
 * (cited by Russell and Norvig). Semantic networks became popular in the 1970s,
 * with important work done by Collins and Quillian. A prominent debate in AI 
 * pertaining to semantic networks concerns its relative merits compared with logic.
 * Russell and Norvig point out, however, that semantic networks can also be considered 
 * a form of logic, as is the case with the other forms of knowledge representation
 * discussed above. A more recent example of a semantic network is WordNet that is a
 * large lexical database of English words that are grouped by synonyms.
 * 
 * Although semantic networks are relatively easy to set up, the main problem is deciding 
 * on the type of knowledge that goes into the network–that is, what type of knowledge 
 * should be represented by the states and what type of knowledge by the arcs. A rule of
 * thumb is that state labels should be nouns and arc labels should be verbs. 
 */

namespace KRLab.Core.SNet
{
    using Path = List<SNNode>;

    //G=(V,E),G:directed acycled graph, V:Node set,E:Relation Set
    public class SemanticNet: DirectedWeightedSparseGraph<SNNode, SNEdge>//DirectedWeightedSparseGraph<SNNode>// 
    {
        public string Topic { get; set; } 

        public List<SNEdge> AllEdges
        {
            get
            {
                List<SNEdge> edges = new List<SNEdge>();
                foreach(var e in Edges)
                {
                    edges.Add((SNEdge)e);
                }
                return edges;
            }
        }

        public List<SNNode> AllNodes
        {
            get
            {
                List<SNNode> nodes = new List<SNNode>();
                foreach(var n in Vertices)
                {
                    nodes.Add((SNNode)n);
                }
                return nodes;
            }
        }

        public SemanticNet(string topic)
        {
            Topic = topic; 
        }

        public bool AddEdge(SNEdge edge)
        {
            return AddEdge(edge.Source, edge.Destination, edge.Rational);
        }
        public bool AddEdge(SNNode source, SNNode destination, SNRational rational)
        {
            // Check existence of nodes, the validity of the weight value, and the non-existence of edge
            if (rational == null)
                return false;
            if (!HasVertex(source) || !HasVertex(destination))
                return false;

            // Add edge from source to destination
            var edge = new SNEdge();
            edge.Create(source, destination, rational);
            _adjacencyList[source].Append(edge);

            // Increment edges count
            ++_edgesCount;

            return true;
        }

        public IEnumerable<SNNode> TopologicalSort()
        {
            return TopologicalSorter.Sort<SNNode>(this);
        }
         

        public void AddNode(SNNode node)
        {
            base.AddVertex(node);
        }

        public void AddNodes(SNNode[] nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException();
            }
            foreach (SNNode node in nodes)
            {
                AddVertex(node);
            }

        }

        public bool IsActionNode(SNNode node)
        {
            if (node == null)
                return false;
            List<SNNode> nodes = GetOutgoingDestinations(node, SNRational.ACTR, SNRational.ACTED);
            if (nodes.Count == 0)
                return false;
            else
                return true;
        }

        public bool IsAndOrNode(SNNode node)
        {
            if (node == null)
                return false;

            List<SNNode> nodes = GetIncomingSources(node, SNRational.AND, SNRational.OR);
            return nodes.Count != 0;
        }

        /// <summary>
        /// 查找从startVertex开始的AND或OR连接。
        /// </summary>
        /// <param name="StartVertex"></param>
        /// <returns>返回多个链表，每个链表中的元素构成AND语义关系，多个链表之间构成OR的语义关系</returns>
        public List<List<SNNode>> FindAndOrSets(SNNode startVertex)
        {
            if (!IsAndOrNode(startVertex))
                return null;

            var visited = new HashSet<SNNode>();
            var stack = new System.Collections.Generic.Stack<SNNode>(VerticesCount);

            List<List<SNNode>> andOrLists = new List<List<SNNode>>();
            andOrLists.Add(new List<SNNode>() { startVertex });

            stack.Push(startVertex);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                List<SNNode> nodes;
                int i = IsAndOrNode(current, out nodes);
                if (!visited.Contains(current) && i != 0)
                {
                    // DFS VISIT NODE STEP 
                    visited.Add(current);

                    if (i == 1)//AND
                    {
                        List<List<SNNode>> tmps = new List<List<SNNode>>();
                        foreach (var ao in andOrLists)
                        {
                            if (ao.Contains(current))
                            {
                                ao.Remove(current);
                            }
                            ao.AddRange(nodes);
                            tmps.Add(ao);
                        }
                        andOrLists = tmps;
                    }
                    else if (i == 2)//OR
                    {
                        foreach (var ao in andOrLists)
                        {
                            if (ao.Contains(current))
                            {
                                ao.Remove(current);
                            }
                        }
                        List<List<SNNode>> tmps = new List<List<SNNode>>();
                        foreach (var nd in nodes)
                        {
                            List<SNNode> tmpNodes = new List<SNNode>();
                            tmpNodes.Add(nd);
                            foreach (var ao in andOrLists)
                            {
                                tmpNodes.AddRange(ao);
                            }
                            tmps.Add(tmpNodes);
                        }

                        andOrLists = tmps;
                    }

                    // Get the adjacent nodes of current
                    foreach (var adjacent in nodes)
                        if (!visited.Contains(adjacent))
                            stack.Push(adjacent);
                }
            }
            return andOrLists;
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodes"></param>
        /// <param name="net"></param>
        /// <returns>1s</returns>
        public int IsAndOrNode(SNNode node, out List<SNNode> nodes)
        {
            nodes = GetIncomingSources(node, SNRational.AND);
            if (nodes.Count > 0)
            {
                return 1;
            }
            nodes = GetIncomingSources(node, SNRational.OR);
            if (nodes.Count > 0)
            {
                return 2;
            }
            return 0;
        }

        public bool IsISPNode(SNNode node, out List<SNNode> nodes)
        {
            nodes = GetIncomingSources(node, SNRational.ISP);
            if (nodes.Count != 0)
                return true;
            else
                return false;
        }

        public bool IsISONode(SNNode node, out List<SNNode> nodes)
        {
            nodes = GetIncomingSources(node, SNRational.ISO);
            if (nodes.Count != 0)
                return true;
            else
                return false;
        }


        /// <summary>
        /// 判断from结点是否与to结点有ISA关系
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool IsISARational(SNNode from, SNNode to)
        {
            if (from == to)
                return false;

            bool isa = false;
            List<SNNode> nodes = GetAPath(from, to);
            if (nodes.Count == 0)
                return false;

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                SNRational r = Rational(nodes[i], nodes[i + 1]);
                if (r != null)
                {
                    string str = r.Rational;
                    if (str != SNRational.IS && str != SNRational.ISA)
                    {
                        return false;
                    }

                    if (str == SNRational.ISA)
                        isa = true;
                }

            }

            return isa;
        }



        public static bool IsProblemNode(SNNode node)
        {
            if (node.Name == "提问")
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 判断节点是否是被赋值的节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsAssignedNode(SNNode node)
        {
            SNNode nd = GetOutgoingDestination(node, SNRational.ASSGN);
            return nd != null;
        }

        public bool IsExprNode(SNNode node)
        {
            SNNode nd = GetOutgoingDestination(node, SNRational.EXPR);
            return nd != null;
        }


        /// <summary>
        /// 查找从source开始的所有后续节点，即发出ANTE连接的节点
        /// </summary>
        /// <param name="source">确保source是一个发出ANTE连接的节点</param>
        /// <param name="nodes"></param>
        public void WalkForANTENodes(SNNode source,out List<SNNode> nodes)
        {
            if(source==null)
            {
                nodes = null;
                return;
            }

            var visited = new HashSet<SNNode>();
            var queue = new KRLab.Core.DataStructures.Lists.Queue<SNNode>();
            nodes = new List<SNNode>(); 

            nodes.Add(source);
            visited.Add(source);

            queue.Enqueue(source);

            while (!queue.IsEmpty)
            {
                var current = queue.Dequeue();
                var neighbors = Neighbours(current);

                if (GetOutgoingDestination(current,SNRational.ANTE)!=null)
                { 
                    nodes.Add(current);
                }

                foreach (var adjacent in neighbors)
                {
                    if (!visited.Contains(adjacent))
                    { 
                        visited.Add(adjacent);
                        queue.Enqueue(adjacent);
                    }
                }
            }

        }

        /// <summary>
        /// BreadthFirstWalk方法查询公式中的运算符结点和该结点相连的链接
        /// </summary>
        /// <param name="listOfNodes">访问过的节点</param>
        /// <param name="results"></param>
        public void WalkForOperatorNodes(SNNode source,
            out ArrayList<SNNode> listOfNodes,
            out List<Tuple<SNNode, List<SNEdge>, SNNode>> results)
        {
            var visited = new HashSet<SNNode>();
            var queue = new KRLab.Core.DataStructures.Lists.Queue<SNNode>();
            listOfNodes = new ArrayList<SNNode>();
            results = new List<Tuple<SNNode, List<SNEdge>, SNNode>>();

            listOfNodes.Add(source);
            visited.Add(source);

            queue.Enqueue(source);

            while (!queue.IsEmpty)
            {
                var current = queue.Dequeue();
                var neighbors = Neighbours(current);

                if (IsOperationNode(current))
                {
                    List<SNEdge> outs = GetOutgoingEdges(current);
                    //运算符结点只能有一个进入链接
                    List<SNEdge> ins = GetIncomingEdges(current);
                    Tuple<SNNode, List<SNEdge>, SNNode> opTuple = new Tuple<SNNode, List<SNEdge>, SNNode>(
                            current, outs, ins[0].Source);
                    results.Add(opTuple);

                }

                foreach (var adjacent in neighbors)
                {

                    if (!visited.Contains(adjacent))
                    {
                        listOfNodes.Add(adjacent);
                        visited.Add(adjacent);
                        queue.Enqueue(adjacent);

                    }
                }
            }

        }

        public static bool IsOperationNode(SNNode node)
        {
            if (node.Name == "-" || node.Name == "+" || node.Name == "*" ||
                node.Name == "/" || node.Name == "^" || node.Name == "=")
            {
                return true;
            }
            else
            {
                return false;
            }
        } 

        /// <summary>
        /// 获取node的全部IS和ISA上级结点，
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<SNNode> GetFatherNodes(SNNode node)
        {
            return GetAllDestinations(node, SNRational.IS, SNRational.ISA);
        }
        /// <summary>
        /// 获取全部具有关系type从node发出的上级结点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param> 
        /// <returns></returns>
        public List<SNNode> GetAllDestinations(SNNode node, string type)
        {
            var visited = new List<SNNode>();
            var stack = new System.Collections.Generic.Stack<SNNode>(VerticesCount);

            stack.Push(node);

            ///查找node的父节点
            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Contains(current))
                {
                    // DFS VISIT NODE STEP 
                    visited.Add(current);

                    // 获取当前节点的所有父节点
                    List<SNNode> fathers = GetOutgoingDestinations(current, type);
                    foreach (var adjacent in fathers)
                    {
                        if (!visited.Contains(adjacent))
                            stack.Push(adjacent);
                    }
                }
            }

            return visited;
        }
        /// <summary>
        /// 获取全部具有关系type1或type2从node发出的上级结点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public List<SNNode> GetAllDestinations(SNNode node,string type1,string type2)
        {
            var visited = new List<SNNode>();
            var stack = new System.Collections.Generic.Stack<SNNode>(VerticesCount);

            stack.Push(node);

            ///查找node的父节点
            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Contains(current))
                {
                    // DFS VISIT NODE STEP 
                    visited.Add(current);

                    // 获取当前节点的所有父节点
                    List<SNNode> fathers = GetOutgoingDestinations(current, type1, type2);
                    foreach (var adjacent in fathers)
                    {
                        if (!visited.Contains(adjacent))
                            stack.Push(adjacent);
                    }
                }
            }

            visited.Remove(node);

            return visited;
        }


        public List<SNNode> Neighbours(SNNode vertex, string edgeType)
        {
            var neighbours = Neighbours(vertex);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var node in neighbours)
            {
                SNEdge edge = (SNEdge)GetEdge(vertex, node);
                if (edge.Rational.Rational == edgeType)
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        /// <summary>
        /// 输入字符串与节点名称上包含关系，不是严格相等。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SNNode SearchNode(string name)
        {
            Dictionary<SNNode, DLinkedList<SNEdge>>.Enumerator itor =
                _adjacencyList.GetEnumerator();

            while (itor.MoveNext())
            {
                if (itor.Current.Key.Name.Contains(name) || name.Contains(itor.Current.Key.Name))
                {
                    return itor.Current.Key;
                }

                DLinkedList<SNEdge> edges = itor.Current.Value;
                foreach (var edge in edges)
                {
                    if (edge.Source.Name.Contains(name) || name.Contains(edge.Source.Name))
                        return edge.Source;
                    else if (edge.Destination.Name.Contains(name) || name.Contains(edge.Destination.Name))
                        return edge.Destination;
                }
            }
            return null;
        }

        /// <summary>
        /// 查找相关的节点
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public List<SNNode> SearchNodes(string inputStr)
        {
            List<SNNode> nodes = new List<SNNode>();

            SNNode node = FastGetNode(inputStr);
            if(node!=null)
            {
                nodes.Add(node);
                return nodes;
            }

            string[] strs = inputStr.Split(new char[] { ' ', ',', '，' });
            foreach (var str in strs)
            {
                str.Trim();
            }

            Dictionary<SNNode, DLinkedList<SNEdge>>.Enumerator itor =
                _adjacencyList.GetEnumerator();

            string ands = strs[0];
            string ors = strs[0];
            for(int i=1;i<strs.Length;i++)
            {
                ands += "&" + strs[i];
                ors += "|" + strs[i];
            }

            while (itor.MoveNext())
            {
                string name = itor.Current.Key.Name;
                if (TextProcessor.Comp(name, ands))
                {
                    nodes.Add(itor.Current.Key);
                }
                if (TextProcessor.Comp(name, ors) && !nodes.Contains(itor.Current.Key))
                {
                    nodes.Add(itor.Current.Key);
                } 
            }
            return nodes;
        }

        /// <summary>
        /// 模糊查找结点
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public SNNode FuzzyGetNode(string inputStr)
        { 
            Dictionary<SNNode, DLinkedList<SNEdge>>.Enumerator itor =
                _adjacencyList.GetEnumerator();

            while (itor.MoveNext())
            {
                if (TextProcessor.Comp(itor.Current.Key.Name, inputStr))
                {
                    return itor.Current.Key;
                }

                DLinkedList<SNEdge> edges = itor.Current.Value;
                foreach (var edge in edges)
                {
                    if (TextProcessor.Comp(edge.Source.Name , inputStr))
                        return edge.Source;
                    else if (TextProcessor.Comp(edge.Destination.Name , inputStr))
                        return edge.Destination;
                }
            }
            return null;
        }


        public SNNode FastGetNode(string name)
        {
            Dictionary<SNNode, DLinkedList<SNEdge>>.Enumerator itor =
                _adjacencyList.GetEnumerator();

            while (itor.MoveNext())
            {
                if (itor.Current.Key.Name == name)
                {
                    return itor.Current.Key;
                }

                DLinkedList<SNEdge> edges = itor.Current.Value;
                foreach (var edge in edges)
                {
                    if (edge.Source.Name == name)
                        return edge.Source;
                    else if (edge.Destination.Name == name)
                        return edge.Destination;
                }
            }
            return null;
        }

        public SNNode GetNode(string name)
        {
            return GetFirstNode(name);
        }

        public SNNode GetNode(IEnumerable<string> names)
        {
            return GetFirstNode(names);
        }

        public SNNode GetFirstNode(string name)
        {
            Dictionary<SNNode, DLinkedList<SNEdge>>.Enumerator itor =
                _adjacencyList.GetEnumerator();

            while (itor.MoveNext())
            {
                if (itor.Current.Key.Name == name)
                {
                    return itor.Current.Key;
                }
            }
            return null;
        }



        /// <summary>
        /// 找到其中一个名字的结点即可
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public SNNode GetFirstNode(IEnumerable<string> names)
        {
            Dictionary<SNNode, DLinkedList<SNEdge>>.Enumerator itor =
                _adjacencyList.GetEnumerator();

            while (itor.MoveNext())
            {
                if (names.Contains(itor.Current.Key.Name))
                {
                    return itor.Current.Key;
                }
            }
            return null;
        }

        public SNNode GetFirstMatch(SNNode startNode, string name)
        {
            Predicate<SNNode> match = new Predicate<SNNode>(target => { return target.Name == name; });
            return BreadthFirstSearcher.FindFirstMatch<SNNode>(this, startNode, match);
        }

        /// <summary>
        /// 获取相同名字的所有结点
        /// </summary> 
        public List<SNNode> GetNodes(string name)
        {
            List<SNNode> nodes = new List<SNNode>();

            Dictionary<SNNode, DLinkedList<SNEdge>>.Enumerator itor =
                AdjacencyList.GetEnumerator();

            while (itor.MoveNext())
            {
                if (itor.Current.Key.Name == name)
                {
                    nodes.Add(itor.Current.Key);
                }
            }
            return nodes;
        } 

        public List<SNNode> GetNodes(IEnumerable<string> names)
        {
            List<SNNode> nodes = new List<SNNode>();

            Dictionary<SNNode, DLinkedList<SNEdge>>.Enumerator itor =
                AdjacencyList.GetEnumerator();

            while (itor.MoveNext())
            {
                if (names.Contains(itor.Current.Key.Name))
                {
                    nodes.Add(itor.Current.Key);
                }
            }
            return nodes;
        }


        /// <summary>
        /// 查询并记录从start开始的所有结点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SNNode> TraverseForAllNodes(SNNode start, string name = "")
        {
            List<SNNode> nodes;
            Predicate<SNNode> match = new Predicate<SNNode>(target =>
            {
                if (name == "")//如果不输入第二个参数，或第二个参数为空字符，则返回所有结点
                    return true;
                return target.Name == name;
            }
            );
            BreadthFirstSearcher.VisitAll<SNNode>(this, start, match, out nodes);
            return nodes;
        }

        /// <summary>
        /// 查询并记录所有从start开始的发出连接的节点，也就是说如果节点只有到达的连接，则查询到此
        /// 终止。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SNNode> TraverseForSubNodes(SNNode start,string name="")
        {
            List<SNNode> nodes;
            Predicate<SNNode> match = new Predicate<SNNode>(target =>
            {
                if (name == "")//如果不输入第二个参数，或第二个参数为空字符，则返回所有结点
                    return true;
                return target.Name == name;
            }
            );
            BreadthFirstSearcher.VisitAllSubNodes<SNNode>(this, start, match, out nodes);
            return nodes;
        }

        /// <summary>
        /// 查找从start开始的所有的子节点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<SNNode> GetSubNodes(SNNode start, string name = "")
        {
            List<SNNode> nodes;
            Predicate<SNNode> match = new Predicate<SNNode>(target =>
            {
                if (name == "")//如果不输入第二个参数，或第二个参数为空字符，则返回所有结点
                    return true;
                return target.Name == name;
            }
            );
            BreadthFirstSearcher.VisitAllSubNodes<SNNode>(this, start, match, out nodes);
            return nodes;
        }



        public List<SNNode> GetLeafNodes(SNNode start)
        {
            List<SNNode> nodes = GetSubNodes(start);
            List<SNNode> leaves = new List<SNNode>();
            foreach (var node in nodes)
            {
                if (GetOutgoingEdges(node).Count == 0)
                    leaves.Add(node);
            }
            return leaves;
        }

        /// <summary>
        /// 该路径是有方向的，从source到destination
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public List<SNNode> GetAPath(SNNode s, SNNode d)
        {
            BreadthFirstShortestPaths<SNNode> firstShortestPath = new BreadthFirstShortestPaths<SNNode>(this, s);
            IEnumerable<SNNode> path = firstShortestPath.ShortestPathTo(d);
            if (path == null)
                return new List<SNNode>();
            return path.ToList();

        }

        /// <summary>
        /// 获取从node1到node2之间的通路，不考虑方向性。
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public List<SNNode> GetAConnection(SNNode node1, SNNode node2)
        {
            BreadthFirstShortestConnections<SNNode> firstShortestPath = new BreadthFirstShortestConnections<SNNode>(this, node1);
            IEnumerable<SNNode> path = firstShortestPath.ShortestPathTo(node2);
            if (path == null)
                return new List<SNNode>();
            return path.ToList();
        }


        /// <summary>
        /// 获取以某个结点node作为起点的邻接<节点-关系>对
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<System.Tuple<SNNode, SNRational>> OutgoingNodeRationalNeighbours(SNNode node)
        {
            if (!HasVertex(node))
                return null;

            var neighbors = GetOutgoingEdges(node);
            List<System.Tuple<SNNode, SNRational>> tuples = new List<System.Tuple<SNNode, SNRational>>();

            foreach (var adjacent in neighbors)
                tuples.Add(new System.Tuple<SNNode, SNRational>(adjacent.Destination, adjacent.Rational));

            return tuples;
        }

        /// <summary>
        /// 获取以某个结点node作为起点的邻接<节点-关系>对
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<System.Tuple<SNNode, SNRational>> InComingNodeRationalNeighbours(SNNode node)
        {
            return InComingNeighboursMap(node);
        }
        public List<System.Tuple<SNNode, SNRational>> InComingNeighboursMap(SNNode node)
        {
            if (!HasVertex(node))
                return null;

            var neighbors = GetIncomingEdges(node);
            List<System.Tuple<SNNode, SNRational>> tuples = new List<System.Tuple<SNNode, SNRational>>();

            foreach (var adjacent in neighbors)
                tuples.Add(new System.Tuple<SNNode, SNRational>(adjacent.Source, adjacent.Rational));

            return tuples;
        }

        /// <summary>
        /// 获取进入边的相邻点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<SNNode> GetInNeighbors(SNNode node)
        {
            List<SNEdge> edges = GetIncomingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
                nodes.Add(e.Source);
            return nodes;
        }

        /// <summary>
        /// 获取出去边的相邻点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<SNNode> GetOutNeighbors(SNNode node)
        {
            DLinkedList<SNNode> nodes = Neighbours(node);
            return nodes.ToList();
        } 


        /// <summary>
        ///一个普通意义的语义网可以包含多个独立的语义网，
        ///从已有的一个语义网中，创建一个子网,保护每个节点的所有邻接节点。 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="subName"></param>
        /// <returns></returns>
        public SemanticNet CreateSubNetWithAllNeighbors(SNNode start, string name="")
        {
            SemanticNet subNet = new SemanticNet(name);
            List<SNNode> nodes = TraverseForAllNodes(start);
            foreach (var nd in nodes)
            {
                if (!subNet.HasVertex(nd))
                    subNet.AddNode(nd);
                List<SNEdge> edges = GetAllEdges(nd);
                foreach (var edge in edges)
                    if (!subNet.HasEdge(edge))
                        subNet.AddEdge(edge);
            }
            return subNet;
        }

        /// <summary>
        /// 在已有的语义网基础上，从start节点开始遍历，创建一个子网，
        /// 只是考虑每个节点的单向(发出)邻接节点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public SemanticNet CreateSubNetWithNeighbors(SNNode start,string name="")
        {
            SemanticNet subNet = new SemanticNet(name);
            List<SNNode> nodes = TraverseForSubNodes(start);
            foreach(var nd in nodes)
            {
                if (!subNet.HasVertex(nd))
                    subNet.AddNode(nd);
                List<SNEdge> edges = GetOutgoingEdges(nd);
                foreach(var edge in edges)
                {
                    if (!subNet.HasEdge(edge))
                        subNet.AddEdge(edge);
                }
            }

            return subNet;
        }

        /// <summary>
        /// 将输入的节点和连接，加入到net，得到一个新的语义网
        /// </summary>
        /// <param name="net"></param>
        /// <param name="nodes"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static SemanticNet CreateNewNet(SemanticNet net,List<SNNode> nodes,List<SNEdge> edges)
        {
            foreach(var node in nodes)
            {
                if (!net.HasVertex(node))
                    net.AddNode(node);
            }
            foreach(var edge in edges)
            {
                if (!net.HasEdge(edge))
                    net.AddEdge(edge);
            }
            
            return net;
        }
        /// <summary>
        /// 将两个语义网合并得到一个新的语义网
        /// </summary>
        /// <param name="net1"></param>
        /// <param name="net2"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public SemanticNet AddNets(SemanticNet net1,SemanticNet net2,string name="")
        {
            SemanticNet net = new SemanticNet(name);
            net.AddVertices(net1.Vertices.ToList()); 

            foreach(var node in net2.Vertices)
            {
                if (!net.HasVertex(node))
                    net.AddVertex(node);
            }
            foreach(var edge in net1.Edges)
            {
                SNEdge e = (SNEdge)edge;
                if(!net.HasEdge(e))
                {
                    net.AddEdge(e);
                }
            }
            foreach(var edge in net2.Edges)
            {
                SNEdge e = (SNEdge)Edges;
                if(!net.HasEdge(e))
                {
                    net.AddEdge(e);
                }
            }
            return net;
        }
        #region edges

        public List<SNEdge> GetAllEdges(SNNode node)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            edges.AddRange(GetIncomingEdges(node));
            return edges;
        }

        public List<SNEdge> GetIncomingEdges(SNNode node)
        {
            if (node == null)
                return new List<SNEdge>();

            List<SNEdge> edges = new List<SNEdge>();
            IEnumerable<IEdge<SNNode>> es = IncomingEdges(node);
            foreach (IEdge<SNNode> e in es)
            {
                edges.Add(e as SNEdge);
            }
            return edges;
        }

        public List<SNEdge> GetIncomingEdges(SNNode node, params string[] types)
        {
            List<SNEdge> edges = GetIncomingEdges(node);
            List<SNEdge> results = new List<SNEdge>();
            foreach (var edge in edges)
            {
                if (types.Contains(edge.Rational.Rational))
                    results.Add(edge);
            }
            return results;
        }

        public List<SNEdge> GetOutgoingEdges(SNNode node)
        {
            if (node == null)
                return new List<SNEdge>();

            List<SNEdge> edges = new List<SNEdge>();
            IEnumerable<IEdge<SNNode>> es = OutgoingEdges(node);
            foreach (IEdge<SNNode> e in es)
            {
                edges.Add(e as SNEdge);
            }
            return edges;
        }

        public List<SNEdge> GetOutgoingEdges(SNNode node, params string [] types)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNEdge> results = new List<SNEdge>();
            foreach (var edge in edges)
            {
                if (types.Contains(edge.Rational.Rational))
                    results.Add(edge);
            }
            return results;
        }

        public string GetRational(SNNode from, SNNode to)
        {
            SNRational conn = Rational(from, to);
            if (conn != null)
            {
                return conn.Rational;
            }
            else
            {
                return SNRational.NULLRational;
            }
        }

        public bool HasConnection(SNNode from, SNNode to, string type)
        {
            SNRational conn = Rational(from, to);
            if (conn != null && conn.Rational == type)
                return true;
            else
                return false;
        }

        public SNRational Rational(SNNode from,SNNode to,params string[] types)
        {
            SNRational ral = Rational(from, to);
            foreach(var type in types)
            {
                if (ral.Rational == type)
                    return ral;
            }
            return null;
        }

        /// <summary>
        /// 具有方向性的关系
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public SNRational Rational(SNNode from, SNNode to)
        {
            IEnumerable<SNEdge> edges = GetOutgoingEdges(from);
            if (edges.Count<SNEdge>() == 0)
            {
                return null;
            }
            else
            {
                foreach (SNEdge edge in edges)
                {
                    if (edge.Destination == to)
                    {
                        return edge.Weight as SNRational;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 没有考虑方向性的关系
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public SNRational ConnectionRational(SNNode node1, SNNode node2)
        {
            IEnumerable<SNEdge> edges = GetAllEdges(node1);
            if (edges.Count<SNEdge>() == 0)
            {
                return null;
            }
            else
            {
                foreach (SNEdge edge in edges)
                {
                    if (edge.Destination == node2 || edge.Source == node2)
                    {
                        return edge.Weight as SNRational;
                    }
                }

                return null;
            }
        }

        #endregion

        public void Print()
        {
            Console.WriteLine("语义网络: " + Topic);
            IEnumerable<SNNode> nodes = Vertices;
            foreach (SNNode node in nodes)
            {
                Console.WriteLine(node.Name);
            }

            IEnumerable<IEdge<SNNode>> edges = Edges;
            foreach(IEdge<SNNode> e in edges)
            {
                SNEdge edge = e as SNEdge;
                Console.WriteLine(((SNRational)edge.Weight).Rational + ": " + edge.Source.Name + " to " + edge.Destination.Name);
            }
        }
        
        public void Print(SNNode start)
        {
            BreadthFirstSearcher.PrintAll<SNNode>(this, start);
        }

        #region Common Connection


        /// <summary>
        /// 获取father结点下，与node的兄弟结点，
        /// </summary>
        /// <param name="net"></param>
        /// <param name="node"></param>
        /// <param name="father"></param>
        /// <returns></returns>
        public List<SNNode> GetSiblingNodes(SNNode node, SNNode father)
        {
            if (father == null)
                return new List<SNNode>();

            List<SNNode> nodes = GetIncomingSources(father);
            nodes.Remove(node);
            return nodes;
        }

        public static List<SNEdge> GetIncomingEdges(SemanticNet net, SNNode node)
        {
            if (node == null)
                return new List<SNEdge>();
            return net.GetIncomingEdges(node);
        }

        public static List<SNEdge> GetOutgoingEdges(SemanticNet net, SNNode node)
        {
            if (node == null)
                return new List<SNEdge>();
            return net.GetOutgoingEdges(node);
        }

        public SNNode GetIncomingSource(SNNode node, string type)
        {
            List<SNEdge> edges = GetIncomingEdges(node); 
            foreach (var e in edges)
            {
                if (e.Rational.Rational == type)
                    return e.Source;
            }

            return null;
        }
        public SNNode GetIncomingSource(SNNode node, string type1,string type2)
        {
            List<SNEdge> edges = GetIncomingEdges(node); 
            foreach (var e in edges)
            {
                if (e.Rational.Rational == type1 || e.Rational.Rational==type2)
                    return e.Source;
            }

            return null;
        }
        public List<SNNode> GetIncomingSources(SNNode node,string type)
        {
            List<SNEdge> edges = GetIncomingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
            {
                if (e.Rational.Rational == type)
                    nodes.Add(e.Source);
            }

            return nodes;
        }


        public List<SNNode> GetIncomingSources(SNNode node, params string [] types)
        {
            List<SNEdge> edges = GetIncomingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
            {
                if (types.Contains(e.Rational.Rational))
                    nodes.Add(e.Source);
            }

            return nodes;
        }
        

        public List<SNNode> GetOutgoingDestinations(SNNode node,string type)
        {
            if (node == null)
                return new List<SNNode>();

            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
            {
                if (e.Rational.Rational == type)
                    nodes.Add(e.Destination);
            }

            return nodes;

        }

        /// <summary>
        /// 获取满足type1或type2关系的目的结点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public List<SNNode> GetOutgoingDestinations(SNNode node, string type1,string type2)
        {
            if (node == null)
                return new List<SNNode>();

            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
            {
                if (e.Rational.Rational == type1 || e.Rational.Rational==type2)
                    nodes.Add(e.Destination);
            }

            return nodes;

        }

        /// <summary>
        /// 获取满足types中关系的目的结点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public List<SNNode> GetOutgoingDestinations(SNNode node, params string[] types )
        {
            if (node == null)
                return new List<SNNode>();

            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
            {
                if (types.Contains( e.Rational.Rational))
                    nodes.Add(e.Destination);
            }

            return nodes;

        }

        public SNNode GetOutgoingDestination(SNNode node, string type,string label)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
            {
                if (e.Rational.Rational == type && e.Rational.Label==label)
                    return e.Destination;
            }

            return null;

        }
        public SNNode GetOutgoingDestination(SNNode node, string type)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
            {
                if (e.Rational.Rational == type )
                    return e.Destination;
            }

            return null;

        }
        /// <summary>
        /// 获取从node发出的type的终点节点，该终点节点不是notOther
        /// </summary>
        /// <param name="node"></param>
        /// <param name="notOther"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public SNNode GetOutgoingOtherDest(SNNode node,SNNode notOther,string type)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> nodes = new List<SNNode>();
            foreach (var e in edges)
            {
                if (e.Rational.Rational == type && e.Destination!=notOther)
                    return e.Destination;
            }

            return null;
        }

        public SNNode GetOutgoingDestination(List<SNNode> nodes, params string[] types)
        {
            foreach (var node in nodes)
            {
                List<SNEdge> edges = GetOutgoingEdges(node);  
                foreach(var edge in edges)
                {
                    if (types.Contains(edge.Rational.Rational))
                        return edge.Destination;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取公式语义网中的常量节点
        /// </summary> 
        /// <returns></returns>
        public List<SNNode> GetConstNodes()
        {
            List<SNNode> nodes = GetNodes(ITSStrings.Const);
            List<SNNode> results = new List<SNNode>();
            foreach (var node in nodes)
            {
                results.AddRange(GetIncomingSources(node, SNRational.ISA,SNRational.IS));
            }
            return results;
        }

        /// <summary>
        /// 获取语义网中的常变量节点
        /// </summary> 
        /// <returns></returns>
        public List<SNNode> GetConstVarNodes()
        {
            List<SNNode> nodes = GetNodes(ITSStrings.ConstVar);
            List<SNNode> results = new List<SNNode>();
            foreach (var node in nodes)
            {
                results.AddRange(GetIncomingSources(node, SNRational.ISA,SNRational.IS));
            }
            return results;
        }

        /// <summary>
        /// 获取语义网中的变量节点
        /// </summary> 
        /// <returns></returns>
        public List<SNNode> GetVariableNodes()
        {
            List<SNNode> nodes = GetNodes(ITSStrings.Variable);
            List<SNNode> results = new List<SNNode>();
            foreach (var node in nodes)
            {
                results.AddRange(GetIncomingSources(node, SNRational.ISA,SNRational.IS));
            }
            return results;
        }
        #endregion

        #region Common Semantic Connection
        public SNNode GetASSOCNode(SNNode node)
        {
            List<SNEdge> edges = GetOutgoingEdges(node); 
            foreach (var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ASSOC)
                    return edge.Destination;
            }

            return null;
        }
        public SNNode GetASSOCNode(SNNode node,string label)
        {
            List<SNEdge> edges = GetOutgoingEdges(node); 
            foreach (var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ASSOC &&
                    edge.Rational.Label==label)
                    return edge.Destination;
            }

            return null;
        }


        public List<SNNode> GetATTNodes(SNNode node)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> subNodes = new List<SNNode>();
            foreach (var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ATT)
                    subNodes.Add(edge.Destination);
            }
            return subNodes;
        }
        public List<SNNode> GetATTNodes(SNNode node, string label)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> subNodes = new List<SNNode>();
            foreach (var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ATT && edge.Rational.Label == label)
                    subNodes.Add(edge.Destination);
            }
            return subNodes;
        }
        public List<SNEdge> GetATTEdges(SNNode node)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNEdge> results = new List<SNEdge>();
            foreach (var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ATT)
                    results.Add(edge);
            }
            return results;
        }
        public List<SNEdge> GetATTEdges(SNNode node, string label)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNEdge> results = new List<SNEdge>();
            foreach (var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ATT && edge.Rational.Label == label)
                    results.Add(edge);
            }
            return results;
        }

        public SNNode GetATTNode(SNNode node,string label)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> subNodes = new List<SNNode>();
            foreach (var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ATT && edge.Rational.Label == label)
                    return edge.Destination;
            }

            return null;
        }

        public SNNode GetATTNode(SNNode node)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            List<SNNode> subNodes = new List<SNNode>();
            foreach (var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ATT)
                    return edge.Destination;
            }

            return null;
        }

        /// <summary>
        /// 查找名字为name的结点的ISA结点。
        /// 比如，查找名称为name的结点的定义结点,语义解释为，name的"定义"是什么
        /// SNNode node=net.FindNode("定义");
        /// GetISANode(net,node,name);//找到name的定义
        /// 同样的，如果要查找某个物理量的单位，其过程如下：
        /// SNNode node=net.FindNode("单位");
        /// GetISANode(net,node,name);
        /// </summary>
        /// <param name="defNode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public SNNode GetISANode(SNNode typeNode, string name)
        {
            SNNode node = FastGetNode(name);
            return GetISANode(typeNode, node);
        }
        public SNNode GetISANode(SNNode typeNode,SNNode node)
        {
            if (node == null)
                return null;
            List<SNNode> nodes = GetIncomingSources(node, SNRational.ASSOC);

            foreach (var n in nodes)
            {
                if (IsISARational(n,typeNode))
                    return n;
            }
            return null;            
        }


        public SNEdge GetOutgoingEdge(SNNode node,string type)
        {
            List<SNEdge> edges = GetOutgoingEdges(node);
            foreach (var edge in edges)
            {
                ///注意，一个结点只能有一个IS父节点
                if (edge.Rational.Rational == type)
                    return edge;
            }
            return null;
        }

        public new SNEdge GetEdge(SNNode from,SNNode to)
        {
            List<SNEdge> edges = GetOutgoingEdges(from);
            foreach(var edge in edges)
            {
                if(edge.Destination==to)
                {
                    return edge;
                }
            }
            return null;
        }

        public SNEdge GetEdge(SNNode from,SNNode to,string type)
        {
            List<SNEdge> edges = GetOutgoingEdges(from);
            foreach(var edge in edges)
            {
                if(edge.Destination==to && edge.Rational.Rational==type)
                {
                    return edge;
                }
            }
            return null;
        }
        #endregion

    }
}
