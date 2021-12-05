using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    public class PhenomenaKRModuleSNet : KRModuleSNet
    {
        public override string KRType => ProjectType.phensn;

        protected SNNode _contNode=null;

        public PhenomenaKRModuleSNet(SemanticNet net) : base(net,KCNames.Phenomena)
        {
        }
        public override void CheckAndInit()
        {
            base.CheckAndInit();
        }

        public SNNode ContentNode
        {
            get
            {      
                if(_contNode==null)
                {
                    ATTParseInfo att = new ATTParseInfo(_topicNode, Net);
                    _contNode = att.GetAttValueNode("内容");
                    return _contNode; 
                }
                return _contNode;
            }
        }

        public List<SNNode> GetAssocNodesOfContent()
        {
            return Net.GetOutgoingDestinations(ContentNode, SNRational.ASSOC);
        }
         
        /// <summary>
        /// 用于检查ACT的语法，在KRLab编辑器的代码中调用
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="relations"></param>
        /// <param name="callback"></param>
        public new static void Check(List<IEntity> entities, List<Relationship> relations, Action<bool, string> callback)
        {
            IEntity topNode = null;
            IEntity contentNode = null;
            foreach (var node in entities)
            {
                if (node.Name == KCNames.Phenomena)
                    topNode = node;
                else if (node.Name == "内容")
                    contentNode = node;
            }

            if (topNode == null)
            {
                callback(false, "没有<" + KCNames.Phenomena + ">结点");
                return;
            }
            if (contentNode == null)
            {
                callback(false, "没有<内容>结点");
                return;
            }

            IEntity topicNode = null;
            List<Relationship> rels = new List<Relationship>();
            bool isOk1 = false;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if ((snr.SNRelationshipType.ToString() == SNRational.IS || snr.SNRelationshipType.ToString() == SNRational.ISA)
                    && snr.Second == topNode)
                    topicNode = snr.First;
                if (snr.SNRelationshipType.ToString() == SNRational.ISA && snr.Second == contentNode)
                {
                    rels.Add(rl);
                }
                if (snr.SNRelationshipType.ToString() == SNRational.ATT && snr.First == topNode &&
                    snr.Second == contentNode)
                    isOk1 = true;

            }
            if (topicNode == null)
            {
                callback(false, "必须有子节点与<" + topNode.Name + ">结点以IS相连！");
                return;
            }
            if (!isOk1)
            {
                callback(false, "<内容>结点必须以ATT关系与<" + topNode.Name + ">相连！");
                return;
            }
            if (rels.Count == 0)
            {
                callback(false, "必须给出<" + contentNode.Name + ">的ISA子节点，用以给出现象的具体内容！");
                return;
            }

            IEntity concNode = null;
            if (rels.Count == 1)
            {
                concNode = rels[0].First;
            }
            else if (rels.Count > 1)
            {
                foreach (var rel in rels)
                {
                    if (rel.Label.Contains("文字描述"))
                    {
                        concNode = rel.First;
                        break;
                    }
                }
            }
            if (concNode == null)
            {
                callback(false, "有多个<" + contentNode.Name + ">的ISA子节点，必须在ISA连接上用'文字描述’指明内容的文字描述节点！");
                return;
            }

            List<IEntity> contKeyNodes = new List<IEntity>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.SNRelationshipType.ToString() == SNRational.ASSOC &&
                    snr.First == concNode)
                    contKeyNodes.Add(snr.Second);
            }
            if (contKeyNodes.Count == 0)
            {
                callback(false, "必须以ASSOC关系指出结论内容的关键词!");
                return;
            }

            ACTParseInfo.Check(entities, relations, callback);

        }
    }
}
