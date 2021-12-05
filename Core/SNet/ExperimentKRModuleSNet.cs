using KRLab.Core.BDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using Utilities;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 必须建立与测量相关的所有知识，比如误差。
    /// </summary>
    public class ExperimentKRModuleSNet:KRModuleSNet
    {
        public override string KRType => ProjectType.expsn;

        protected ATTParseInfo _attInfo = null;

        public ExperimentKRModuleSNet(SemanticNet net):base(net,KCNames.Experiment)
        {

        }


        /// <summary>
        /// 这部分是否可以放到MeasureSQA里面做？？？
        /// </summary>
        public override void CheckAndInit()
        {
            base.CheckAndInit();
            _attInfo = new ATTParseInfo(_topicNode, Net);
        }

        public List<SNNode> GetMethodNodes()
        {
            return _attInfo.GetAttValueNodes("方法");
        }

        
        public List<SNNode> GetInstrumentNodes()
        {
            SNNode insNode = Net.GetOutgoingDestination(_topicNode, SNRational.DEPT);
            Debug.Assert(insNode != null);
            List<SNNode> results = Net.GetIncomingSources(insNode, SNRational.ISA);
            return results;
        }

        /// <summary>
        /// 实验原理只有一个，所以只用一个节点表示实验原理
        /// </summary>
        /// <returns></returns>
        public SNNode GetPrincipleNode()
        {
            return _attInfo.GetAttValueNode("原理");
        }

        public List<SNNode> GetPurposeNodes()
        {
            return _attInfo.GetAttValueNodes("目的");
        }


        /// <summary>
        /// 检查创建的语义网是否满足该类型知识的要求，在GUI项目中的Workspace类的CheckNet函数中
        /// 调用。
        /// </summary>
        /// <param name="entities">语义网的结点</param>
        /// <param name="relations">语义网的连接</param>
        /// <param name="callback">消息反馈</param>
        public new static void Check(List<IEntity> entities, List<Relationship> relations,
            Action<bool, string> callback)
        {
            Dictionary<string, IEntity> topNodes = new Dictionary<string, IEntity>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.SNRelationshipType.ToString() == SNRational.ATT && snr.First.Name == "实验")
                {
                    topNodes[snr.Second.Name] = snr.Second;
                    topNodes[snr.First.Name] = snr.First;
                }
            }

            if (!topNodes.ContainsKey("实验"))
            {
                callback(false, "缺少<实验>节点！");
                return;
            }
            if (!topNodes.ContainsKey("方法"))
            {
                callback(false, "缺少<方法>节点！");
                return;
            }
            if (!topNodes.ContainsKey("器材"))
            {
                callback(false, "缺少<器材>节点！");
                return;
            }
            //if (!topNodes.ContainsKey("结论"))
            //{
            //    callback(false, "缺少<结论>节点！");
            //    return;
            //}

            IEntity topicNode = KRModuleSNet.GetTopicNode(topNodes["实验"], entities, relations);
            if (topicNode == null)
            {
                callback(false, "缺少与'" + KCNames.Experiment + "'结点以'" + SNRational.IS + "'关系连接的结点，作为学习课题结点！");
                return;
            }

            List<IEntity> toolNodes = new List<IEntity>();
            List<IEntity> methodNodes = new List<IEntity>();
            IEntity prinNode = null;
            IEntity goalNode = null;
            IEntity conNode = null;

            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if(snr.First==topNodes["实验"] && snr.SNRelationshipType.ToString()!=SNRational.ATT)
                {
                    callback(false, "<实验>与<" + snr.Second.Name + ">节点之间只能以ATT相连！");
                    return;
                }
                if (snr.SNRelationshipType.ToString() == SNRational.IS)
                {
                    if (topNodes.ContainsKey("原理") && snr.Second == topNodes["原理"])
                    {
                        prinNode = snr.First;
                    }
                    else if (topNodes.ContainsKey("目的") && snr.Second == topNodes["目的"])
                    {
                        goalNode = snr.First;
                    }
                    else if (topNodes.ContainsKey("结论") && snr.Second == topNodes["结论"])
                    {
                        conNode = snr.First;
                    }

                }
                else if (topNodes.ContainsKey("器材") && snr.Second == topNodes["器材"])
                {
                    if (snr.SNRelationshipType.ToString() != SNRational.IS && snr.SNRelationshipType.ToString()!=SNRational.ATT)
                    {
                        callback(false, "请用IS连接指明实验所需的器材！");
                        return;
                    }
                    toolNodes.Add(snr.First);
                }
                else if (topNodes.ContainsKey("方法") && snr.Second == topNodes["方法"])
                {
                    if (snr.SNRelationshipType.ToString() != SNRational.ISP && snr.SNRelationshipType.ToString()!=SNRational.ATT)
                    {
                        callback(false, "请用ISP连接指明实验方法或步骤！");
                        return;
                    }

                    int i;
                    if (snr.SNRelationshipType.ToString()==SNRational.ISP && !int.TryParse(snr.Label, out i))
                    {
                        callback(false, "指明方法的ISP连接的标签上必须注明数字1.2.3...表示步骤顺序！");
                        return;
                    }
                    methodNodes.Add(snr.First);
                }
            }
            if (toolNodes.Count == 0)
            {
                callback(false, "没有与<器材>节点以IS相连的节点，用以指明实验所需的所有器材！");
                return;
            }
            if (methodNodes.Count == 0)
            {
                callback(false, "没有与<方法>节点以ISP相连的节点，用以指明实验方法或步骤，并在连接标签上注明数字1.2.3...表示步骤顺序！");
                return;
            }
            if (topNodes.ContainsKey("结论") && conNode == null)
            {
                callback(false, "没有与<结论>节点以IS相连的节点，用以指明实验结论！");
                return;
            }
            if(topNodes.ContainsKey("目的") && goalNode==null)
            {
                callback(false, "没有与<目的>节点以IS相连的节点，用以指明实验目的！");
                return;
            }
            if(topNodes.ContainsKey("原理") && prinNode==null)
            {
                callback(false, "没有与<原理>节点以IS相连的节点，用以指明实验原理！");
                return;                
            }

            List<IEntity> prinNodes = new List<IEntity>();
            List<IEntity> goalNodes = new List<IEntity>();
            List<IEntity> conNodes = new List<IEntity>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.First == prinNode)
                    prinNodes.Add(snr.Second);
                if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.First == goalNode)
                    goalNodes.Add(snr.Second);
                if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.First == conNode)
                    conNodes.Add(snr.Second);
            }
            if (prinNode != null && prinNodes.Count == 0)
            {
                callback(false, "必须对实验原理进行语义建模，请参见结论语义网的方法！");
                return;
            }
            if (conNode!=null && conNodes.Count == 0)
            {
                callback(false, "必须对实验结论进行语义建模，请参见结论语义网的方法！");
                return;
            }
            if (goalNode != null && goalNodes.Count == 0)
            {
                callback(false, "必须对实验目的进行语义建模，请参见结论语义网的方法！");
                return;
            }
        }
             
    }
}
