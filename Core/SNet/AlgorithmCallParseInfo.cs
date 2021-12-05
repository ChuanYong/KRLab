using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 算法或函数调用的语义解析，要处理算法和函数的输入及输出，
    /// 输入是用ARGV，ARGV2，ARGV3等连接指明，输出是用RESULT连接指明。
    /// </summary>
    public class AlgorithmCallParseInfo:ParseInfo
    { 
        public AlgorithmCallParseInfo(SNNode node,SemanticNet net)
            :base(net)
        {

        }

        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            throw new NotImplementedException();
        }
    }
}
