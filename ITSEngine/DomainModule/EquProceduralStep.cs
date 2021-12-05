using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    /// <summary>
    /// 每个算法步骤用GRANU连接指明该步骤的操作，各操作如果有执行顺序
    ///    用ANTE连接指明
    /// </summary>
    public class EquProceduralStep:ProceduralStep
    {
        /// <summary>
        /// 算法步骤对应的子语义网
        /// </summary>
        protected SemanticNet _net; 
        public SemanticNet Net
        {
            get { return _net; }
        }

        protected CONDParseInfo _condInfo;
        public CONDParseInfo CondInfo
        {
            get { return _condInfo; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepNode">算法步骤节点</param>
        /// <param name="net">整个算法语义网</param>
        public EquProceduralStep(SNNode stepNode,SemanticNet net):base(stepNode)
        {
            _net = net.CreateSubNetWithNeighbors(stepNode);
            SNNode cond = net.GetOutgoingDestination(stepNode, SNRational.COND);
            if (cond != null)
                _condInfo = new CONDParseInfo(cond, net);
            else
                _condInfo = null;
        }

        public List<SNNode> GetAllOperatorNodes()
        {
            return _net.GetOutgoingDestinations(_stepNode, SNRational.GRANU);
        }
    }
}
