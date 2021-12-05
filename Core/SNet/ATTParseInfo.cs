using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 解析语义网中的属性语义。
    /// （1）一个结点的属性包括了自身具有的属性值，以及所有的父节点的属性值。
    /// （2）如果属性值没有用ASSOC关系与某个结点连接起来，则所有属性值都是结点的属性。否则，
    /// 只有某个特定的属性值是属于结点的。
    /// 
    ///                           
    ///                            ATT                  ISA
    ///              【结点0】------------>【属性0】<--------【属性值01】
    ///                /|\                    /|\
    ///                 | IS                   | ISA 
    ///                 |     【属性值10】     |
    ///             【结点1】  /    /|\     【属性值02】 
    ///                /|\   \/      | 
    ///                 | IS / \     |
    ///                 |   /   \    |
    ///             【结点2】   【属性1】------>【属性值11】       
    /// </summary>
    public class ATTParseInfo:ParseInfo
    {
        /// <summary>
        /// _node是分析对象，判断_node的属性及其属性值
        /// </summary>
        protected SNNode _node;
        //表述_node的属性持有关系，比如，物体-(的-质量-是),中间的字符串
        //就是属性名称，属性结点的名称。
        protected List<Tuple<string,string,string>> _actAttTuples;
        //记录属性值节点，一个属性可以能有多个独立的值，或一个值范围
        protected Dictionary<string,List<SNNode>> _attValueNodeDict;

        protected List<SNNode> _fathers;

        protected bool _isOk = true;

        public Dictionary<string, List<SNNode>> ATTValueNodeDict
        {
            get { return _attValueNodeDict; }
        }

        public bool IsATT
        {
            get { return _isOk; }
        }

        public List<SNNode> AttNodes 
        {
            get
            {
                if (!_isOk)
                {
                    return null;
                }

                List<SNNode> nodes = new List<SNNode>();
                nodes.AddRange(_net.GetOutgoingDestinations(_node, SNRational.ATT));
                foreach (var node in _fathers)
                {
                    List<SNNode> attNodes = _net.GetOutgoingDestinations(node, SNRational.ATT);
                    nodes.AddRange(attNodes);
                }

                return nodes;

            }

        } 

        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            qas = new List<System.Tuple<string, string[]>>();
            System.Tuple<string, string[]> qa = new System.Tuple<string, string[]>(_node.Name + "有哪些属性？",
                _attValueNodeDict.Keys.ToArray());
            qas.Add(qa);
             
            List<string> tmp = _attValueNodeDict.Keys.ToList();
            foreach(var att in tmp)
            {
                string str = GetAttValue(att);
                qa = new System.Tuple<string, string[]>(_node.Name+"的"+att + "是什么？",new[] { str });
                qas.Add(qa);
            }
        }

        /// <summary>
        /// 确保node!=null
        /// </summary>
        /// <param name="node">查询这个节点的属性</param>
        /// <param name="net"></param>
        /// <param name="projectType"></param>
        public ATTParseInfo(SNNode node,SemanticNet net):base(net)
        { 
            _node = node;
            _fathers = _net.GetFatherNodes(_node);
            _actAttTuples = new List<Tuple<string, string,string>>();
            _attValueNodeDict = new Dictionary<string, List<SNNode>>();

            Parse();
        }         

        protected Tuple<string,string,string> GetTuple(string attString)
        {
            foreach(var t in _actAttTuples)
            {
                if (t.Item2 == attString)
                    return t;
            }

            return null;
        }

        /// <summary>
        /// 解析一句完整的句子，描述物体的属性
        /// </summary>
        /// <param name="attName">属性名称</param>
        /// <returns></returns>
        public string Parse(string attName)
        {
            if (!_isOk)
            {
                return _node.Name + "没有属性";
            }

            List<SNNode> attValueNodes = _attValueNodeDict[attName];
            Tuple<string, string, string> t = GetTuple(attName);
            string str = _node.Name + t.Item1 + t.Item2 + t.Item3;
            foreach(var node in attValueNodes)
            {
                str += node.Name+"和";
            }
            string str1=str.Remove(str.Length - 1,1);

            return str1;
        }

        /// <summary>
        /// 获取属性值，在语义网建模中，对于属性，不一定要给出属性值
        /// </summary>
        /// <param name="attName"></param>
        /// <returns></returns>
        public string GetAttValue(string attName)
        {
            if(!_isOk)
            {
                return _node.Name + "没有属性";
            }

            List<SNNode> attValueNodes = _attValueNodeDict[attName];
            string str = string.Empty;
            foreach (var node in attValueNodes)
            {
                str += node.Name + "，";
            }
            string str1 = str.Remove(str.Length - 1, 1);

            return str1;
        }


        /// <summary>
        /// 获取某个属性结点的属性值，也就是其VAL子节点
        /// </summary>
        /// <param name="attNode"></param>
        /// <returns></returns>
        public List<SNNode> GetAttValueNodes(SNNode attNode)
        {
            return _net.GetOutgoingDestinations(attNode, SNRational.VAL);
        }

        /// <summary>
        /// 获取指定属性名称的属性值节点
        /// </summary>
        /// <param name="attName"></param>
        /// <returns></returns>
        public List<SNNode> GetAttValueNodes(string attName)
        {
            if (!_isOk)
            {
                return null;
            }

            return _attValueNodeDict[attName]; 
        }

        public SNNode GetAttValueNode(string attName)
        {
            if (!_isOk)
                return null;

            return _attValueNodeDict[attName][0];
        }



        /// <summary>
        /// 解析结点node的属性，包括其自身和所有的父节点的属性结点。
        /// </summary>
        /// <param name="node"></param>
        /// <returns>属性名称+属性值</returns>
        public static Dictionary<string, List<SNNode>> ParseAttribute(SNNode node,SemanticNet net)
        {
            var visited = new HashSet<SNNode>();
            var stack = new Stack<SNNode>(net.VerticesCount);

            stack.Push(node);

            ///查找node的父节点,包括node本身
            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Contains(current))
                {
                    // DFS VISIT NODE STEP 
                    visited.Add(current);

                    // 获取当前节点的所有父节点
                    List<SNNode> fathers = net.GetOutgoingDestinations(current, SNRational.IS, SNRational.ISA);
                    foreach (var adjacent in fathers)
                    {
                        if (!visited.Contains(adjacent))
                            stack.Push(adjacent);
                    }
                }
            }

            Dictionary<string, List<SNNode>> attNodes = new Dictionary<string, List<SNNode>>();
            ///对每一个父节点
            foreach (var fNode in visited)
            {
                ///父节点具有的所有属性结点
                List<SNNode> tmpAttNodes = net.GetOutgoingDestinations(fNode, SNRational.ATT);
                foreach (var nd in tmpAttNodes)
                {
                    ///查找属性结点下所有的子节点
                    List<SNNode> valNodes = net.GetOutgoingDestinations(nd, SNRational.VAL);
                    if (valNodes.Count == 0)
                    {
                        attNodes[nd.Name] = new List<SNNode>();
                        continue;
                    }

                    foreach (var isNode in valNodes)
                    {
                        foreach (var fn in visited)
                        {
                            //如果有ASSOC与某个属性连接，表示属性下面的子节点不是
                            //这个fn结点都有的，
                            if (net.HasConnection(isNode, fn, SNRational.ASSOC))
                                attNodes[nd.Name] = new List<SNNode>() { isNode };
                            //如果属性结点与fn有ASSOC连接，则属性结点下的子节点都是fn的属性
                            if (net.HasConnection(nd, fn, SNRational.ASSOC))
                            {
                                attNodes[nd.Name] = valNodes;
                            }

                            else//如果没有ASSOC连接，表示属性结点下的所有子节点都是，
                                attNodes[nd.Name] = valNodes;
                        }
                    }
                }
            }

            return attNodes;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="attNode"></param>
        protected void Parse()
        {
            List<SNNode> attNodes = AttNodes;
            //如果没有属性结点，则返回
            if (attNodes.Count == 0)
            {
                _isOk = false;
                return;
            }

            foreach(var attNode in attNodes)
            {
                List<SNEdge> edges = _net.GetIncomingEdges(attNode, SNRational.ATT);
                SNEdge edge1 = edges[0];
                List<SNNode> attValueNodes = _net.GetOutgoingDestinations(attNode,SNRational.VAL);

                SNNode vNode;
                bool b = IsASSOCConnected(attValueNodes,out vNode);
                if (!b)
                {
                    _attValueNodeDict[attNode.Name] = attValueNodes;
                }
                else
                    _attValueNodeDict[attNode.Name] = new List<SNNode>() { vNode };
                SNEdge edge2 = _net.GetEdge(attNode,attValueNodes[0]);
                Tuple<string, string,string> tuple = new Tuple<string,string, string>(edge1.Rational.Label, attNode.Name,edge2.Rational.Label);
                _actAttTuples.Add(tuple);
            } 
        }

        protected bool IsASSOCConnected(List<SNNode> attValueNodes,out SNNode node)
        {
            foreach(var fn in _fathers)
            {
                foreach(var vNode in attValueNodes)
                {
                    SNRational snr = _net.Rational(vNode, fn);
                    if (snr != null && snr.Rational == SNRational.ASSOC)
                    {
                        node = vNode;
                        return true;
                    }
                }
            }
            node = null;
            return false;
        }
    }
}
