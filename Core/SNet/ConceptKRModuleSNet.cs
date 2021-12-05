using KRLab.Core.Algorithms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
 
using Utilities;

namespace KRLab.Core.SNet
{
    /// <summary> 
    /// 必须建立与概念相关的知识，比如内涵和外延。
    /// 特征是一个或多个客体特性的抽象结果，而概念是对特征的独特组合而形成的知识单元。
    /// （1）个别概念，对应于一个客体，比如地球，
    /// （2）一般概念，对应于具有共性的多个客体，比如，行星，
    /// （3）一个概念可以有其区别概念和近似概念作为属性，
    /// （4）一个概念可以有关联知识类型，比如一个概念关联一个公式，或关联一个结论
    /// （5）一个概念的内涵可以分成多个部分进行描述，
    /// 概念的外延定义：列举根据同一准则划分出的全部下位概念来描述一个概念。
    /// 概念的内涵定义：用上位概念和区别特征描述概念内涵的定义。 
    /// </summary>
    public class ConceptKRModuleSNet:KRModuleSNet
    {
        protected SNNode _intensionNode;//内涵结点
        protected SNNode _extensionNode;//外延结点
        protected SNNode _distinctiveNode;//区别特征
        protected SNNode _similarNode;//近似概念

        public override string KRType => ProjectType.conceptsn; 

        /// <summary>
        /// 获取学习课题的定义节点，以及它们之间连接边的标签
        /// </summary>
        public SNNode DefinitionNode
        {
            get
            {
                SNNode node = Net.GetOutgoingDestination(_intensionNode,SNRational.VAL);
                return node;
            }
        } 

        public List<SNNode> DefAssocedNodes
        {
            get
            {
                SNNode defNode = DefinitionNode;
                List<SNNode> nodes = Net.GetOutgoingDestinations(defNode, SNRational.ASSOC);
                return nodes;
            }
        }

        /// <summary>
        /// 概念的区别概念
        /// </summary>
        public List<SNNode> DistinctiveNodes
        {
            get 
            {
                return Net.GetOutgoingDestinations(_distinctiveNode,SNRational.VAL);
            }
        } 

        //上位概念
        public System.Tuple<string, SNNode> SuperConceptNode
        {
            get
            {
                List<SNNode> nodes=Net.GetOutgoingDestinations(_topicNode, SNRational.IS);//主题节点IS的终止节点
                SNNode node = null;
                foreach(var nd in nodes)
                {
                    if(Net.HasConnection(nd,_krNode,SNRational.IS))
                    {
                        node = nd;
                        break;
                    }
                }
                SNEdge edge = Net.GetEdge(_topicNode, node); 
                return new System.Tuple<string, SNNode>(edge.Rational.Label, node);
            }
        }
        
        /// <summary>
        /// 概念内涵的特征节点
        /// </summary>
        public List<SNNode> CharacteristicsNodes
        {
            get
            {
                SNNode node = Net.GetOutgoingDestination(_intensionNode, SNRational.ATT);
                List<SNNode> nodes = Net.GetOutgoingDestinations(node, SNRational.VAL);
                return nodes;
            }
        }

        /// <summary>
        /// 概念的外延节点
        /// </summary>
        public List<System.Tuple<string,SNNode>> ExtensionNodes
        {
            get
            {
                List<System.Tuple<string, SNNode>> nodes = new List<System.Tuple<string, SNNode>>();
                List<SNNode> tmps= Net.GetOutgoingDestinations(_extensionNode, SNRational.VAL);
                foreach(var nd in tmps)
                {
                    SNEdge edge = Net.GetEdge(_extensionNode,nd);
                    nodes.Add(new System.Tuple<string, SNNode>(edge.Rational.Label, nd));
                }
                return nodes;
            }
        }

        public ConceptKRModuleSNet(SemanticNet net):base(net,KCNames.Concept)
        {

        }

        public List<SNNode> GetSimilarNodes()
        {
            return Net.GetOutgoingDestinations(_similarNode, SNRational.VAL);
        }

        public override void CheckAndInit()
        {
            base.CheckAndInit(); 

            ///概念的外延不一定有
            List<SNNode> nodes = Net.GetOutgoingDestinations(_krNode, SNRational.ATT);
            foreach(var node in nodes)
            {
                if (node.Name == "内涵")
                    _intensionNode = node;
                else if (node.Name == "外延")
                    _extensionNode = node;
                else if (node.Name == "区别特征")
                    _distinctiveNode = node;
                else if (node.Name == "近似概念")
                    _similarNode = node;
            } 

        }
       

        public new static void Check(List<IEntity> entities, List<Relationship> relations, Action<bool, string> callback)
        { 

            IEntity topNode = null;
            IEntity intentNode = null;
            IEntity extentNode = null;
            IEntity distinctiveNode = null;
            foreach (var node in entities)
            {
                if (node.Name == KCNames.Concept) 
                    topNode = node; 
                if(node.Name=="内涵") 
                    intentNode = node; 
                if (node.Name == "外延")
                    extentNode = node;
                if (node.Name == "区别特征")
                    distinctiveNode = node;
            }

            if (topNode == null)
            {
                callback(false, "没有" + KCNames.Concept + "结点");
                return;
            }
            if(intentNode==null)
            {
                callback(false, "概念应该有内涵结点，并以ATT与之连接！");
                return;
            }

            IEntity topicNode = KRModuleSNet.GetTopicNode(topNode,entities,relations);
            if (topicNode == null)
            {
                callback(false, "必须有学习课题结点以" + SNRational.KTYPE + "'关系与缺少与<" + KCNames.Concept + ">结点相连！");
                return;
            }


            List<IEntity> defNodes = new List<IEntity>();//概念定义结点  
            bool isOk = false;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;  
                if (snr.SNRelationshipType.ToString()==SNRational.VAL && snr.First == intentNode && 
                    (snr.Label != "图片描述"))
                    defNodes.Add(snr.First);
                if (snr.SNRelationshipType.ToString() == SNRational.KTYPE && snr.First == topicNode &&//
                    snr.Second == topNode && snr.Label == "主题")
                    isOk = true;
            }
            if(!isOk)
            {
                callback(false, "必须指明概念的主题节点，用KTYPE连接，从<" + topicNode.Name + ">指向<" + topNode.Name + ">节点，连接标签为‘主题’");
                return;
            }
            if(defNodes.Count==0)
            {
                callback(false, "必须指定<内涵>结点的VAL节点，以说明内涵的具体内容！");
                return;
            } 

            IEntity superNode = null;//上位概念结点 
             
            IEntity distCharatNode = null;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                 
                if (distinctiveNode != null && snr.SNRelationshipType.ToString() == SNRational.VAL && snr.First == distinctiveNode)
                    distCharatNode = snr.Second;
                if(snr.SNRelationshipType.ToString()==SNRational.IS && snr.Second !=topNode && 
                    snr.First==topicNode)
                {
                    superNode = snr.First;
                }
            } 
 
            if(distinctiveNode!=null && distCharatNode==null)
            {
                callback(false, "必须以<VAL>关系指明<区别特征>的子节点！");
                return;
            }

            List<IEntity> assocedNodes = new List<IEntity>();
            List<IEntity> extentedNodes = new List<IEntity>();
            foreach (var rl in relations)    
            {
                SNRelationship snr = (SNRelationship)rl; 
                if (distCharatNode != null && snr.SNRelationshipType.ToString() == SNRational.ASSOC && 
                    snr.Second == distCharatNode)
                    assocedNodes.Add(snr.First);
                if (extentNode != null && snr.SNRelationshipType.ToString() == SNRational.VAL &&
                    snr.First == extentNode)
                    extentedNodes.Add(snr.First);
            }
            if(distCharatNode!=null && assocedNodes.Count<2)
            {
                callback(false, "必须指明<" + distCharatNode.Name + ">结点相关的两个结点！");
                return;
            }
            if(extentNode!=null && extentedNodes.Count<2)
            {
                callback(false, "必须以<VAL>关系指明外延的构成结点，并且只是两个或两个以上！");
                return;
            }


        }

    }
}
