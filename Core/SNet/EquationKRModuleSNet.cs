using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
 
using Utilities;
using KRLab.Core.DataStructures.Lists;
using KRLab.Core.DataStructures.Graphs;
using KRLab.Core.Algorithms.Graphs;
using KRLab.Core.SNet;
using System.Reflection;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 必须建立与公式相关的所有知识，比如变量和输出。
    /// EquationKRModuleSNet对应一个公式语义网文件，该文件中
    /// 可以有多个公式。
    /// </summary>
    public class EquationKRModuleSNet:KRModuleSNet
    {
        public override string KRType => ProjectType.equsn;

        public static List<string> operators = new List<string>() { "+", "-", "*", "/", "^" };
        public static List<string> funs=new List<string>(){"sin","cos","tan","ctan","ln","log","sqrt","abs","sinh","cosh"}; 

        public EquationKRModuleSNet(SemanticNet net):base(net,KCNames.Equation)
        {

        }

        public override void CheckAndInit()
        {
            base.CheckAndInit();
        }

        public bool HasEquName(string name)
        {
            return Net.GetNode(name)!=null;
        }

        /// <summary>
        /// 获取公式节点equNode的算法
        /// </summary>
        /// <param name="equNode"></param>
        /// <returns></returns>
        public List<SNNode> GetAlgorithmNodes(SNNode equNode)
        {
            ATTParseInfo att = new ATTParseInfo(equNode, Net);
            return att.GetAttValueNodes("算法");
        }

        /// <summary>
        /// 获取公式语义网中所有的公式节点
        /// </summary>
        /// <returns></returns>
        public List<SNNode> GetEquNodes()
        {
            List<SNNode> nodes = new List<SNNode>();
            foreach (var node in Net.Vertices)
            {
                List<SNNode> nds = Net.GetOutgoingDestinations(node, SNRational.ATT);
                if (nds.Count != 2)
                    continue;

                if (nds[0].Name == "左边" && nds[1].Name == "右边")
                {
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        public static List<SNNode> GetEquNodes(SemanticNet net)
        {
            List<SNNode> nodes = new List<SNNode>();
            foreach (var node in net.Vertices)
            {
                List<SNNode> nds = net.GetOutgoingDestinations(node, SNRational.ATT);
                if (nds.Count != 2)
                    continue;

                if (nds[0].Name == "左边" && nds[1].Name == "右边")
                {
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        /// <summary>
        /// 查找公式语义网中的等式
        /// </summary>
        /// <returns></returns>
        public List<EquElem> GetEquElems()
        {
            List<SNNode> equNodes = GetEquNodes();

            List<EquElem> equs = new List<EquElem>(); 
            foreach (var node in equNodes)
            {
                SemanticNet net = Net.CreateSubNetWithNeighbors(node);
                EquElem elm = new EquElem(node, Net);
                equs.Add(elm);
            }
            return equs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="net">对应一个语义图</param>
        /// <returns></returns>
        public static List<EquElem> GetEquElems(SemanticNet net)
        {
            List<SNNode> equNodes = EquationKRModuleSNet.GetEquNodes(net);

            List<EquElem> equs = new List<EquElem>();
            foreach (var node in equNodes)
            {
                SemanticNet subNet = net.CreateSubNetWithNeighbors(node);
                EquElem elm = new EquElem(node, net);
                equs.Add(elm);
            }
            return equs;
        }


        /// <summary>
        ///以网络中一个公式的中间结点source开始进行遍历
        /// </summary>
        public static List<FormulaElement> TraverseFromNode(SNNode source,SemanticNet net)
        {
            if (source == null)
            {
                throw new Exception("没有发现该方程！");
            }

            //List<FormulaElement> elements = new List<FormulaElement>();
            List<FormulaElement> operatorElements = new List<FormulaElement>();

            var visited = new HashSet<SNNode>();
            var queue = new DataStructures.Lists.Queue<SNNode>();
            int i = 0;
            visited.Add(source);
            queue.Enqueue(source);

            while (!queue.IsEmpty)
            {
                var current = queue.Dequeue();
                List<SNEdge> outs = net.GetOutgoingEdges(current);
                List<SNEdge> ins = net.GetIncomingEdges(current);
                FormulaElement element = new FormulaElement(current, i, outs, ins);
                //elements.Add(element);
                i++;
                if (SemanticNet.IsOperationNode(current) || net.IsAssignedNode(current))
                    operatorElements.Add(element);

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
            return operatorElements; 
        }
         

        /// <summary>
        /// 从source结点开始向前查询运算符结点，得到从
        /// source开始的表达式
        /// </summary>
        /// <param name="source"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static string CreateFormulaString(SNNode source, List<FormulaElement> elements,
            SemanticNet net)
        {
            string str = string.Empty;
            if (SemanticNet.IsOperationNode(source) || net.IsAssignedNode(source))
            {
                DLinkedList<string> strList = CreateExpr(source, elements); 
                foreach (var sr in strList)
                    str += sr;
            }
            else
            {
                str = source.Name;
            }

            return str;
        }

        public static DLinkedList<string> CreateExpr(SNNode source, List<FormulaElement> elements)
        {
            Dictionary<SNNode, FormulaElement> dict = new Dictionary<SNNode, FormulaElement>();
            foreach (var elm in elements)
            {
                dict.Add(elm.Node, elm);
            }

            DLinkedList<string> eqr = new DLinkedList<string>();
            eqr.Append("?0?");

            InsertSymbleForElement(elements[0], elements[0], ref eqr);
            for (int i = 0; i < elements.Count; i++)
            {
                ParseAnElement(elements[i], ref eqr,dict);
            }

            return eqr;
        }

        /// <summary>
        /// 将一个FormulaElement解析为字符串，添加到strList中
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="formula"></param>
        public static void ParseAnElement(FormulaElement elem, ref DLinkedList<string> strList,
            Dictionary<SNNode, FormulaElement> dict)
        {
            List<SNEdge> edges = elem.GetArrayedOutEdges();
            foreach (var edge in edges)
            {
                if (SemanticNet.IsOperationNode(edge.Destination))
                {
                    InsertSymbleForElement(elem, dict[edge.Destination], ref strList);
                }
                else
                {
                    int i = strList.IndexOf("?" + elem.Index.ToString() + "?");
                    strList.InsertAt(edge.Destination.Name, i);
                    strList.RemoveAt(++i);
                }
            }
        }

        private static void InsertSymbleForElement(FormulaElement curr, FormulaElement next, ref DLinkedList<string> strList)
        {
            int idx = strList.IndexOf("?" + curr.Index.ToString() + "?");
            List<SNEdge> edges = next.GetArrayedOutEdges();

            strList.InsertAt("(", idx);
            for (int i = 0; i < edges.Count - 1; i++)
            {
                strList.InsertAt("?" + next.Index.ToString() + "?", ++idx);
                strList.InsertAt(next.Node.Name, ++idx);
            }
            strList.InsertAt("?" + next.Index.ToString() + "?", ++idx);
            strList.InsertAt(")", ++idx);
            strList.RemoveAt(++idx);
        }

        /// <summary>
        /// 从net中的start开始建立一个子语义网
        /// </summary>
        /// <param name="start"></param>
        /// <param name="net"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SemanticNet CreateEquSNet(SNNode start,SemanticNet net,string name="")
        {
            SemanticNet subNet = net.CreateSubNetWithNeighbors(start);
            return subNet;
        }

        /// <summary>
        /// 根据指定的FormulaElement创建一个新的语义网
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static SemanticNet CreateEquSNet(List<FormulaElement> elements, string topic = "")
        {
            SemanticNet equNet = new SemanticNet(topic);

            foreach (var e in elements)
            {
                if (!equNet.HasVertex(e.Node))
                    equNet.AddNode(e.Node);
                foreach (var edge in e.OutEdges)
                {
                    if (!equNet.HasEdge(edge.Source, edge.Destination))
                        equNet.AddEdge(edge.Source, edge.Destination, edge.Rational);
                    if (!equNet.HasVertex(edge.Destination))
                        equNet.AddNode(edge.Destination);
                }
                foreach (var edge in e.InEdges)
                {
                    if (!equNet.HasEdge(edge.Source, edge.Destination))
                        equNet.AddEdge(edge.Source, edge.Destination, edge.Rational);
                    if (!equNet.HasVertex(edge.Destination))
                        equNet.AddNode(edge.Destination);
                }
            }

            return equNet;
        }


        /// <summary>
        /// node是一个运算符节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="relations"></param>
        /// <param name="callback"></param>
        private static void CheckTwoConnNode(IEntity node, List<Relationship> relations, Action<bool, string> callback)
        {
            SNRelationship act = null;
            SNRelationship acted = null;
            List<SNRelationship> rels = new List<SNRelationship>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.First == node && (snr.SNRelationshipType.ToString()==SNRational.OPRND || 
                    snr.SNRelationshipType.ToString()==SNRational.OPRED))
                    rels.Add(snr);

                if(snr.First==node && snr.SNRelationshipType.ToString()==SNRational.OPRND) 
                    act = snr; 
                if (snr.First == node && snr.SNRelationshipType.ToString() == SNRational.OPRED)
                    acted = snr; 
            }
            if(rels.Count!=2)
            {
                callback(false, "<" + node.Name + ">节点只能有一个OPRND和一个OPRED连接！");
                return;
            }
            if(act==null)
            {
                callback(false, "<" + node.Name + ">节点必须要一个OPRND连接！");
                return;
            }
            if (acted == null)
            {
                callback(false, "<" + node.Name + ">节点必须要一个OPRED连接！");
                return;
            }
        } 

        public static void CheckLeftRight(IEntity topNode,List<IEntity> entities, List<Relationship> relations
            , Action<bool, string> callback)
        {
            bool isOk = false;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.Second==topNode && snr.SNRelationshipType.ToString() == SNRational.IS)
                {
                    isOk = true;
                    CheckLeftRight(snr.First, entities, relations,callback);
                }
            }
            if(!isOk)
            { 
                IEntity left = null;
                IEntity right = null;
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if (snr.First == topNode && snr.SNRelationshipType.ToString() == SNRational.ATT &&
                        snr.Second.Name == "左边")
                    {
                        left = snr.Second;
                    }
                    if (snr.First == topNode && snr.SNRelationshipType.ToString() == SNRational.ATT &&
                        snr.Second.Name == "右边")
                    {
                        right = snr.Second;
                    }
                }

                if (left==null || right==null)
                {
                    callback(false, "没有确定方程的左右边");
                    return;
                }

                bool isOk1 = false;
                bool isOk2 = false;
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if (snr.Second == left && snr.SNRelationshipType.ToString() == SNRational.ISA)
                    {
                        isOk1 = true;
                    }
                    if (snr.Second==right && snr.SNRelationshipType.ToString() == SNRational.ISA)
                    {
                        isOk2 = true;
                    }
                }

                if(!isOk1 || !isOk2)
                {
                    callback(false, "必须用ISA指明公式的左右边的表达式！");
                    return;
                }

            } 

        }

        public static void CheckOperationNode(IEntity opNode,List<Relationship> relations, Action<bool, string> callback)
        {
            if (opNode.Name == "-" || opNode.Name == "/" || opNode.Name == "^")
            {
                CheckTwoConnNode(opNode, relations, callback);
            }
            else if (opNode.Name == "+" || opNode.Name == "*")
            {
                List<SNRelationship> rels = new List<SNRelationship>();
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if (snr.First == opNode && snr.SNRelationshipType.ToString() == SNRational.OPRND)
                        rels.Add(snr);
                }
                if (rels.Count < 2)
                {
                    callback(false, "<" + opNode.Name + ">节点至少要有两个OPRND连接！");
                    return;
                }
            }

        }

        public new static void Check(List<IEntity> entities, List<Relationship> relations, Action<bool, string> callback)
        {
            IEntity varNode = null;
            IEntity conVarNode = null;
            IEntity constNode = null;
            foreach(var node in entities)
            {
                if (node.Name == "公式")
                {
                    CheckLeftRight(node, entities, relations, callback);
                }
                if (node.Name=="变量")
                {
                    varNode = node;
                }
                else if(node.Name=="常变量")
                {
                    conVarNode = node;
                }
                else if(node.Name=="常量")
                {
                    constNode = node;
                }
                else if(operators.Contains(node.Name))
                {
                    CheckOperationNode(node, relations, callback);
                }
                else if(funs.Contains(node.Name))
                {
                    List<SNRelationship> rels = new List<SNRelationship>();
                    foreach (var rl in relations)
                    {
                        SNRelationship snr = (SNRelationship)rl;
                        if (snr.First == node)
                            rels.Add(snr); 
                    }
                    if(rels.Count!=1 || rels[0].SNRelationshipType.ToString()!=SNRational.OPRED)
                    {
                        callback(false, "<" + node.Name + ">节点表示一个函数，输入参数只有一个，必须用OPRED连接");
                        return;
                    }
                }
            }

            if (varNode == null)
            {
                callback(false, "必须添加<变量>节点");
                return;
            }

            List<IEntity> varNodes = new List<IEntity>();
            List<IEntity> conVarNodes = new List<IEntity>();
            List<IEntity> constNodes = new List<IEntity>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl; 
                if(snr.Second==varNode&& snr.SNRelationshipType.ToString()!=SNRational.IS)
                {
                    callback(false, "从<"+snr.First.Name+">到达<变量>节点的必须是IS连接！");
                    return;
                }

                if(snr.SNRelationshipType.ToString()==SNRational.IS && snr.Second==varNode)
                {
                    varNodes.Add(snr.First);
                }
                if(snr.SNRelationshipType.ToString()==SNRational.IS && snr.Second==conVarNode)
                {
                    conVarNodes.Add(snr.First);
                }
                if(constNode!=null && snr.SNRelationshipType.ToString()==SNRational.IS && snr.Second==constNode)
                {
                    constNodes.Add(snr.First);
                }

            }
            if(varNodes.Count<1)
            {
                callback(false, "必须指明<变量>节点的IS子节点，表示公式中的各个变量，一般来说，这样的变量应该有多个。");
                return;
            }
            if (conVarNode != null && conVarNodes.Count == 0)
            {
                callback(false, "有<常变量>节点，但没有添加其IS子节点，表示公式中的常变量。");
                return;
            }
            if (constNode!=null && constNodes.Count==0)
            {
                callback(false, "有<常量>节点，但没有添加其IS子节点，表示公式中的常量。");
                return;
            }

            //foreach(var node in varNodes)
            //{
            //    bool ok = false;
            //    foreach (var rl in relations)
            //    {
            //        SNRelationship snr = (SNRelationship)rl;
            //        if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.First == node)
            //        {
            //            ok = true;
            //            break;
            //        }
            //    }
            //    if(!ok)
            //    {
            //        callback(false, "这是一个公式，必须用ASSOC连接指明<"+node.Name+">节点所表示的名称");
            //        return;
            //    }
            //}

            //foreach (var node in conVarNodes)
            //{
            //    bool ok = false;
            //    foreach (var rl in relations)
            //    {
            //        SNRelationship snr = (SNRelationship)rl;
            //        if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.First == node)
            //        {
            //            ok = true;
            //            break;
            //        }
            //    }
            //    if (!ok)
            //    {
            //        callback(false, "这是一个公式，必须用ASSOC连接指明<" + node.Name + ">节点所表示的名称");
            //        return;
            //    }
            //}

        }
    }
}
