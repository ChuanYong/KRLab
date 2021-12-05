using KRLab.Core.Algorithms.Common;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utilities;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 解析语义网中的ARGV连接
    /// </summary>
    public class ARGVParseInfo:ParseInfo
    {
        protected SNNode _mainNode;
        protected List<SNNode> _argNodes;

        protected Dictionary<string, System.Tuple<double, double>> _argMinMax;
        protected List<string> _args; 

        public List<string> ARGVs
        {
            get
            {
                return _args;
            }
        }

        /// <summary>
        ///要确保node!=null,确保node是一个问题的文字描述部分
        /// </summary>
        /// <param name="node">具有发出ARGV连接的节点</param>
        /// <param name="net"></param>
        /// <param name="projectType"></param>
        public ARGVParseInfo(SNNode node,SemanticNet net):base(net)
        {
            _mainNode = node;
            if(node!=null)
            {
                SNNode nd = _net.GetOutgoingDestination(node, SNRational.ISA);
                if(nd==null || nd.Name!="文字描述")
                {
                    throw new NetException("必须是问题的文字描述");
                }
            }

            _argNodes = _net.GetOutgoingDestinations(node, SNRational.ARGV);

            _args = new List<string>();
            _argMinMax = new Dictionary<string, System.Tuple<double, double>>();

            Parse();
        }

        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            throw new NotImplementedException();
        }

        protected void Parse()
        {
            foreach(var node in _argNodes)
            {
                _args.Add(node.Name);
                SNNode nd = _net.GetOutgoingDestination(node, SNRational.VAL);
                if(nd!=null)
                {
                    double x;
                    double.TryParse(nd.Name, out x);
                    _argMinMax[node.Name] = new System.Tuple<double, double>(x, x);
                }
                nd = _net.GetOutgoingDestination(node, SNRational.VALR);
                if(nd!=null)
                {
                    SNNode minNode = _net.GetOutgoingDestination(nd, SNRational.MIN);
                    SNNode maxNode = _net.GetOutgoingDestination(nd, SNRational.MAJ);
                    double xmin, xmax;
                    double.TryParse(minNode.Name, out xmin);
                    double.TryParse(maxNode.Name, out xmax);
                    _argMinMax[node.Name] = new System.Tuple<double, double>(xmin, xmax);
                }
            }

        }

        /// <summary>
        /// 随机获取参数的一个值
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public Dictionary<string,double> ARGRandValues
        {
            get
            {
                Dictionary<string, double> dic = new Dictionary<string, double>();
                foreach(var  d in _argMinMax)
                {
                    if(d.Value.Item1==d.Value.Item2)
                    {
                        dic[d.Key] = d.Value.Item1;
                    }
                    else//d.Value.Item1<d.Value.Item2
                    {
                        dic[d.Key] = Math.Round(Rand.Random(d.Value.Item1, d.Value.Item2),1);
                    }
                }
                return dic;
            }

        }
    }
}
