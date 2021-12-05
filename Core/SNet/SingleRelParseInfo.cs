using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 单个连接的语义解析。
    /// </summary>
    public class SingleRelParseInfo:ParseInfo
    {
        protected SNNode _source;
        protected SNNode _dest;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">RESULT连接的发出节点</param>
        /// <param name="net"></param> 
        public SingleRelParseInfo(SNNode source,SNNode dest,SemanticNet net):base(net)
        {
            _source = source;
            _dest = dest;
        }

        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            qas = new List<System.Tuple<string, string[]>>();
        }

        /// <summary>
        /// 解析起点与目的点之间的语义
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public string ParseInfo()
        {
            string info = string.Empty;

            return info;
        } 
    }
}
