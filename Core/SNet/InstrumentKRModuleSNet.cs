using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace KRLab.Core.SNet
{
    public class InstrumentKRModuleSNet:KRModuleSNet
    {
        public override string KRType => ProjectType.inssn;
        protected SNNode _instruNode;
        protected SNNode _topNode;

        protected ATTParseInfo _attInfo = null;

        public SNNode InstrumentNode
        {
            get { return _instruNode; }
        }

        public InstrumentKRModuleSNet(SemanticNet net):base(net,KCNames.Instrument)
        {
        }

        public override void CheckAndInit()
        {
            base.CheckAndInit();
            _topicNode = Net.FastGetNode("器材");
            _instruNode = Net.GetIncomingSource(_topicNode, SNRational.IS);
            if (_instruNode == null)
            {
                throw new NetException("没有'仪器'结点");
            }
            _attInfo = new ATTParseInfo(_topicNode, Net);    
        }  

        public List<SNNode> GetImageNodes()
        { 
            return _attInfo.GetAttValueNodes("图片显示");
        }

        public List<SNNode> GetFuncNodes()
        {
            return _attInfo.GetAttValueNodes("功能");
        }

        public List<SNNode> GetNoticNodes()
        {
            return _attInfo.GetAttValueNodes("注意事项");
        }

        public List<SNNode> GetSuitableNodes()
        {
            return _attInfo.GetAttValueNodes("适用场所");
        }

        public new static void Check(List<IEntity> entities,List<Relationship> relations,
            Action<bool,string>callback)
        {
            IEntity topNode = null;
            IEntity imageNode = null;
            IEntity noticNode = null;
            IEntity caseNode = null;
            IEntity funNode = null;

            foreach (var node in entities)
            {
                if (node.Name == KCNames.Instrument)
                {
                    topNode = node;
                }
                else if (node.Name == "适用场所")
                    caseNode = node;
                else if (node.Name == "注意事项")
                    noticNode = node;
                else if (node.Name == "图片显示")
                    imageNode = node;
                else if (node.Name == "功能")
                    funNode = node;
            }
            if(topNode==null)
            {
                callback(false, "必须有<器材>结点！");
                return;
            }
            if(caseNode==null)
            {
                callback(false, "必须添加器材的<适用场所>结点");
                return;
            }
            if(noticNode==null)
            {
                callback(false, "必须添加器材的<注意事项>结点");
                return;
            }
            if(imageNode==null)
            {
                callback(false, "必须添加器材的<图片显示>结点");
                return;
            }
            if(funNode==null)
            {
                callback(false, "必须添加器材的<功能>结点");
                return;
            }

            IEntity topicNode = KRModuleSNet.GetTopicNode(topNode, entities, relations);
            if (topicNode == null)
            {
                callback(false, "缺少与'" + KCNames.Instrument + "'结点以'" + SNRational.IS + "'关系连接的结点！");
                return;
            } 

            List<IEntity> noticNodes = new List<IEntity>();
            List<IEntity> funNodes = new List<IEntity>();
            List<IEntity> caseNodes = new List<IEntity>();
            IEntity subImageNode = null; 

            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.First == topNode && snr.SNRelationshipType.ToString() != SNRational.ATT)
                {
                    callback(false, "<器材>与<" + snr.Second.Name + ">节点之间只能以ATT相连！");
                    return;
                }
                if (snr.SNRelationshipType.ToString() == SNRational.IS)
                {
                    if (snr.Second == noticNode)
                    {
                        noticNodes.Add(snr.First);
                    }
                    else if (snr.Second == imageNode)
                    {
                        subImageNode = snr.First;
                    }
                    else if (snr.Second == funNode)
                    {
                        funNodes.Add(snr.First);
                    }
                    else if(snr.Second==caseNode)
                    {
                        caseNodes.Add(snr.First);
                    }

                }
                if (subImageNode==null)
                {            
                    callback(false, "请用IS连接指明<图片显示>节点的子节点，指明图片的文件名称！");
                    return; 
                }
                if (noticNodes.Count==0)
                {
                    callback(false, "请用IS连接指明器材的注意事项！");
                    return;
                }
                if(funNodes.Count==0)
                {
                    callback(false, "请用IS连接指明器材的功能！");
                    return;
                }
                if(caseNodes.Count==0)
                {
                    callback(false, "请用IS连接指明器材的适用场所！");
                    return;
                }
            }
        }
    }
}
