using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// (1)每个算法步骤用GRANU连接指明该步骤的操作，各操作如果有执行顺序
    ///    用ANTE连接指明
    /// </summary>
    public class ProceduralKRModuleSNet : KRModuleSNet
    { 
        protected SNNode _startNode;
        protected List<SNNode> _stepNodes;

        public List<SNNode> StepNodes
        {
            get { return _stepNodes; }
        }
        public override string KRType => ProjectType.procesn; 

        public ProceduralKRModuleSNet(SemanticNet net) : base(net,KCNames.Procedural)
        {
            ATTParseInfo att = new ATTParseInfo(GetTopicNode(), net);
            net.WalkForANTENodes(att.GetAttValueNodes("开始")[0],out _stepNodes);            
        }

        public SNNode GetNode()
        {
            SNNode topNode = Net.FastGetNode(KCNames.Procedural);
            return Net.GetIncomingSource(topNode, SNRational.IS);
        }

        public override void CheckAndInit()
        {
            base.CheckAndInit();
        } 

        public new static void Check(List<IEntity> entities, List<Relationship> relations, Action<bool, string> callback)
        {
            IEntity topNode = null; 
            foreach (var node in entities)
            {
                if (KCNames.Procedural.Contains(node.Name))
                    topNode = node; 
            }
            if(topNode==null)
            {
                callback(false, "必须给出一个标有<算法>或<流程>的节点");
                return;
            }
            IEntity topicNode = null;
            IEntity startNode = null;
            IEntity endNode = null;
            IEntity realStartNode = null;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.SNRelationshipType.ToString() == SNRational.IS && snr.Second == topNode)
                    topicNode=snr.First;

                if (snr.SNRelationshipType.ToString() == SNRational.ATT && snr.First == topNode
                    && snr.Second.Name == "开始")
                    startNode = snr.Second;
                if (snr.SNRelationshipType.ToString() == SNRational.ATT && snr.First == topNode
                    && snr.Second.Name == "结束")
                    endNode = snr.Second;

                if(startNode!=null)
                {
                    if (snr.SNRelationshipType.ToString() == SNRational.VAL && snr.First == startNode)
                        realStartNode = snr.Second;
                }
            }
            if(topicNode==null)
            {
                callback(false, "必须用IS连接指明<" + topNode.Name + ">节点的课题节点");
                return;
            }
            if(startNode==null || endNode==null)
            {
                callback(false, "必须指明<" + topNode.Name + ">的属性节点：<开始>和<结束>节点");
                return;
            }
            if(realStartNode==null)
            {
                callback(false, "必须用VAL连接指明算法的<开始>属性节点的值节点，也就是算法的开始！");
                return;
            }

            CheckAStep(realStartNode, endNode, entities, relations, callback);

        }

        protected static void CheckAStep(IEntity stepNode,IEntity endNode,
            List<IEntity> entities, List<Relationship> relations, Action<bool, string> callback)
        {
            IEntity realEndNode = null;
            IEntity nextNode = null;
            IEntity granuNode = null;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.SNRelationshipType.ToString() == SNRational.ANTE && snr.First == stepNode)
                    nextNode = snr.Second;
                if (snr.SNRelationshipType.ToString() == SNRational.GRANU && snr.First == stepNode)
                    granuNode = snr.Second;

                if (snr.SNRelationshipType.ToString() == SNRational.ATT && snr.First == endNode)
                    realEndNode = snr.Second;
            }
            if((realEndNode==null && nextNode==null) || (realEndNode!=null && nextNode!=realEndNode && nextNode==null))
            {
                callback(false, "必须用ANTE添加<" + stepNode.Name + ">节点下一个节点，表示算法或流程的下一步");
                return;
            }
            if(granuNode==null)
            {
                callback(false, "必须用GRANU连接指明<" + stepNode.Name + ">的具体操作");
            }

            ///检查算法中的运算
            foreach(var node in entities)
            {
                if(EquationKRModuleSNet.operators.Contains(node.Name))
                    EquationKRModuleSNet.CheckOperationNode(node, relations, callback);
            }
        }

    }
}
