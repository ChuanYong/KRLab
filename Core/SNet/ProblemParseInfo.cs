using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exversion;
using ITSText;
using Utilities;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 用于处理语义网中的“提问”模块，“提问”模块可以是在一个单独
    /// 的语义网文件，也可以作为一部分，嵌入其它语义网文件中，比如嵌入
    /// 概念、结论、现象知识语义网中。
    /// </summary>
    public class ProblemParseInfo:ParseInfo
    {
        protected SNNode _problemNode; 
        protected List<SNNode> _quesNodes;
        protected List<SNNode> _answNodes;
 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">"提问"节点</param>
        /// <param name="net"></param>
        public ProblemParseInfo(SNNode node,SemanticNet net):base(net)
        {
            List<SNNode> atts = net.GetOutgoingDestinations(node, SNRational.ATT);
            foreach(var nd in atts)
            {
                if (nd.Name.Contains("文字描述"))
                {
                    _problemNode = net.GetOutgoingDestination(nd, SNRational.VAL);
                }
                else if (nd.Name.Contains("提问"))
                {
                    _quesNodes = net.GetOutgoingDestinations(nd,SNRational.VAL);
                    SNNode tm = net.GetOutgoingDestination(nd, SNRational.ATT);
                    _answNodes = net.GetOutgoingDestinations(tm, SNRational.VAL);
                }
            }   
        }
        protected int _idx=0;
        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            qas = new List<System.Tuple<string, string[]>>(); 
            qas.Add(CreateQAInfo());
        }

        /// <summary>
        /// 创建新的问题内容
        /// </summary>
        /// <returns></returns>
        public System.Tuple<string,string[]> CreateQAInfo()
        {
            if (_idx < _quesNodes.Count)
            { 
                ARGVParseInfo parser = new ARGVParseInfo(_problemNode, Net);
                string probContent=TextProcessor.ReplaceWords(_problemNode.Name, parser.ARGRandValues);

                parser = new ARGVParseInfo(_quesNodes[_idx], Net);
                string quesContent = TextProcessor.ReplaceWords(_quesNodes[_idx].Name, parser.ARGRandValues);

                string ans = GetAnswer(_quesNodes[_idx]);
                _idx++; 

                return new System.Tuple<string, string[]>(probContent + "，" + quesContent,
                    new[] { ans });
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取问题的对应答案
        /// </summary>
        /// <returns></returns>
        public string GetAnswer(SNNode quesNode)
        {
            string ans = string.Empty;
            if(_quesNodes.Count==1 && _answNodes.Count==1)
            {
                ARGVParseInfo parser = new ARGVParseInfo(_answNodes[0], Net);
                ans = TextProcessor.ReplaceWords(_answNodes[0].Name, parser.ARGRandValues);                
            }
            else///如果有多个提问，就有多个答案，答案与相应的提问用ASSOC连接关联
            {
                SNNode ansNode = Net.GetIncomingSource(quesNode, SNRational.ASSOC);
                ARGVParseInfo parser = new ARGVParseInfo(ansNode, Net);
                ans = TextProcessor.ReplaceWords(ansNode.Name, parser.ARGRandValues);
            }

            return ans;
        } 
    }
}
