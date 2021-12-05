using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace KRLab.Core.SNet
{
    public class CONDParseInfo:ParseInfo
    {
        protected SNNode _condNode;

        public CONDParseInfo(SNNode node,SemanticNet net):base(net)
        {
            _condNode = node;
        }

        public string CondInfo
        {
            get { return _condNode.Name; }
        }

        /// <summary>
        /// 产生提问的一部分，一般与ACT相结合
        /// </summary>
        /// <param name="qas"></param>
        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            qas = new List<System.Tuple<string, string[]>>();
            List<List<SNNode>> set;
            if(GetAndORConds(out set))
            {
                if(set.Count==1)
                {
                    int i = Rand.Random(set[0].Count);
                    string tmp = set[0][i].Name;

                    set[0].Remove(set[0][i]);
                    string str = string.Empty;
                    foreach(var nd in set[0])
                    {
                        str += nd.Name + "、";
                    }
                    str.Remove(str.Length - 1, 1);

                    string q = "在所有列出的必须同时满足的条件中"+str+"，还缺少哪个？";
                    string a = tmp;
                    qas.Add(new System.Tuple<string, string[]>(q, new[] { a }));
                }
                if(set.Count>1)
                {
                    int i = Rand.Random(set[0].Count);
                    List<SNNode> removeNodes = set[i];
                    string a = string.Empty;
                    foreach (var node in removeNodes)
                    {
                        a += node.Name + "和";
                    }
                    a.Remove(a.Length - 1, 1);

                    set.Remove(set[i]);
                    string str = string.Empty;
                    foreach (var list in set)
                    {
                        string tmpStr = string.Empty;
                        foreach(var n in list)
                        {
                            tmpStr += n.Name+"和";
                        }
                        tmpStr.Remove(tmpStr.Length - 1, 1);
                        str += tmpStr + "、";
                    }
                    str.Remove(str.Length - 1, 1);

                    string q = "在所有列出的可选条件中"+str+"，还缺少哪个？";
                    qas.Add(new System.Tuple<string, string[]>(q, new[] { a }));
                }
            }

            List<SNNode> nodes;
            if(GetGranuNodes(out nodes))
            {
                COMPParseInfo comp = new COMPParseInfo(nodes[0], nodes[1], Net);
                System.Tuple<string, string, string> tp = comp.ComRelation();
                if(tp!=null)
                {
                    string q = tp.Item1 + "在什么条件下";
                    string a = tp.Item1 + tp.Item2 + tp.Item3;
                    qas.Add(new System.Tuple<string, string[]>(q, new[] { a }));

                    q = tp.Item3 + "与" + tp.Item1 + "在什么条件下";
                    a=tp.Item1 + tp.Item2 + tp.Item3;
                    qas.Add(new System.Tuple<string, string[]>(q, new[] { a }));
                }
            }
        }

        public bool GetAndORConds(out List<List<SNNode>> set)
        {
            if (Net.IsAndOrNode(_condNode))
            { 
                set=Net.FindAndOrSets(_condNode);
                return true;
            }
            else
            {
                set = null;
                return false;
            }
        }

        public bool GetGranuNodes(out List<SNNode> nodes)
        {
            nodes=Net.GetOutgoingDestinations(_condNode, SNRational.GRANU);
            if (nodes.Count == 0)
                return false;
            else
                return true;
        }  
    }
}
