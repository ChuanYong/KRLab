using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitKRModuleSNet:KRModuleSNet
    {
        //学习课题结点下面的单位结点
        protected Dictionary<string, SNNode> _unitNodeDict;
        protected Dictionary<string, string> _unitSymbolDict;

        static List<string> operators = new List<string>() {"*", "/", "^" };

        public static readonly Dictionary<string,string> BasicUnits = new Dictionary<string,string>()
        {
            { "长度","米" },{"质量","千克" },{"时间","秒" },{"电流","安培" },{"温度","开尔文"},{"物质的量","摩尔" },{"发光强度","坎德拉" }
        };

        public override string KRType => ProjectType.untsn;

        public Dictionary<string,SNNode> UnitNodeDict
        {
            get { return _unitNodeDict; }
        } 
        public Dictionary<string,string> UnitSymbolDict
        {
            get { return _unitSymbolDict; }
        } 

        public SNNode GetISUnitNode()
        {
            SNNode isNode = Net.GetOutgoingDestination(_krNode, SNRational.ATT);
            if (isNode != null && isNode.Name=="国际制")
                return Net.GetOutgoingDestination(isNode, SNRational.VAL);

            return null;
        }

        public UnitKRModuleSNet(SemanticNet net):base(net,KCNames.Unit)
        { 
        } 

        public override void CheckAndInit()
        {
            base.CheckAndInit();

            _storyNode = GetStory(_topicNode);
            List<SNNode> knowledgeNodes = Net.GetIncomingSources(_topicNode, SNRational.ISA);

            _unitNodeDict = new Dictionary<string, SNNode>();
            foreach (var node in knowledgeNodes)
            {
                _unitNodeDict[node.Name]=node;
            }

            _unitSymbolDict = new Dictionary<string, string>();
            foreach (var d in _unitNodeDict)
            {
                SNNode symNode = Net.GetOutgoingDestination(d.Value, SNRational.SYMB);
                _unitSymbolDict.Add(d.Key, symNode.Name);

            } 
        }

        /// <summary>
        /// 获取单位的定义节点
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public SNNode GetDefinitionNode(SNNode unitNode)
        {
            if (unitNode == null)
                return null;

            ATTParseInfo att = new ATTParseInfo(unitNode, Net);
            List<SNNode> nodes=att.GetAttValueNodes("定义");
            if (nodes.Count > 0)
                return nodes[0];
            else
                return null;
        }


        /// <summary>
        /// 检查创建的语义网是否满足该类型知识的要求，在GUI项目中的Workspace类的CheckNet函数中
        /// 调用。
        /// </summary>
        /// <param name="netRoot">语义网的xml根节点</param>
        /// <param name="callback"></param>
        public new static void Check(List<IEntity> entities, List<Relationship> relations,
            Action<bool,string> callback)
        {
            IEntity topNode=null;
            foreach(var node in entities)
            {
                if (node.Name == KCNames.Unit)
                {
                    topNode = node; 
                } 
            }               

            if (topNode==null)
            {
                callback(false, "没有<" + KCNames.Unit + ">结点");
                return;
            }

            IEntity topicNode = null;
            IEntity symbolNode = null;
            IEntity isNode = null;
            List<IEntity> unitNodes = new List<IEntity>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl; 
                if(snr.SNRelationshipType.ToString()==SNRational.ATT && snr.First==topNode)
                {
                    symbolNode = snr.Second;
                } 
                if (snr.SNRelationshipType.ToString() == SNRational.ISA && snr.Second == topNode)
                {
                    unitNodes.Add(snr.First);
                } 
                if(snr.SNRelationshipType.ToString()==SNRational.ATT && snr.Second==topNode)
                {
                    topicNode = snr.First;
                }
                if(snr.SNRelationshipType.ToString()==SNRational.ATT && snr.First==topNode && snr.Second.Name=="国际制")
                {
                    isNode = snr.Second;
                }
            }

            if (topicNode == null)
            {
                callback(false, "缺少与<" + KCNames.Unit + ">节点以ATT连接的节点，用以指明是什么物理量的单位。");
                return;
            }
            if (symbolNode==null)
            {
                callback(false, "<" + topNode.Name+">结点必须有<符号表示>的属性结点！以ATT相连！");
                return;
            } 
            if (unitNodes.Count==0)
            {
                callback(false, "需要添加与<" + topicNode.Name + ">结点以<" + SNRational.ISA + ">关系相连的结点，以指明具体的<"+
                    topicNode.Name+">的单位");
                return;
            }
            if (isNode == null)
            {
                callback(false, "没有创建<国际制>节点，并用ATT连接从<单位>节点指向<国际制>节点！");
                return;
            }

            List<IEntity> symbolNodes = new List<IEntity>();
            foreach (var nd in unitNodes)
            {
                bool isOk = false;
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.Second == nd)
                    {
                        isOk = true;
                        symbolNodes.Add(snr.First);
                        break;
                    }

                }

                if (!isOk)
                {
                    callback(false, "每个单位必须与符号以ASSOC关系连接！从符号指向单位。");
                    return;
                }
            }

            foreach(var node in symbolNodes)
            {
                bool isOk = false;
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if (snr.First == node && snr.SNRelationshipType.ToString() == SNRational.ISA)
                    {
                        isOk = true;
                        break;
                    }

                }

                if (!isOk)
                {
                    callback(false, "<"+node.Name+">节点必须以ISA关系与<符号表示>节点相连!");
                    return;
                }
            }

          
            //整个语义网中，只有这个地方适用COMP连接，所以直接查询即可
            List<SNRelationship> snrls = new List<SNRelationship>();
            IEntity subIsNode = null;
            foreach(var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if(snr.SNRelationshipType.ToString()==SNRational.COMP && unitNodes.Contains(rl.First))
                {
                    snrls.Add(snr);
                }
                if(snr.SNRelationshipType.ToString()==SNRational.ISA && snr.Second==isNode)///国际制单位
                {
                    subIsNode = snr.First;
                }
            }
            if(snrls.Count<(unitNodes.Count-1))
            {
                callback(false, "两个单位结点之间必须用COMP关系连接起来");
                return;
            }
            if(subIsNode==null)
            {
                callback(false, "没有指明具体的国际单位！");
                return;
            }

            foreach(var rl in snrls)
            {
                if(rl.StartMultiplicity==null || rl.EndMultiplicity==null)
                {
                    callback(false, "<"+rl.First.Name+">到<"+rl.Second.Name+">的连接线数据标注不规范！应该标注单位换算的值。");
                }
                if (rl.Label != "=")
                    callback(false, "<" + rl.First.Name + ">到<" + rl.Second.Name + ">的连接标签应该是=");
                float x,y;
                if(!float.TryParse(rl.StartMultiplicity,out x))
                {
                    callback(false, "<" + rl.First.Name + ">到<" + rl.Second.Name + ">的连接中<" + rl.First.Name + ">输入的不是数值");
                    return;
                }
                if(!float.TryParse(rl.EndMultiplicity,out y))
                {
                    callback(false, "<" + rl.First.Name + ">到<" + rl.Second.Name + ">的连接中<" + rl.First.Name + ">输入的不是数值");
                    return;
                }
            }

            ///如果不是基本单位，要进行导出单位的演算
            bool ok = false;
            if(!BasicUnits.ContainsKey(topicNode.Name))
            {
                IEntity firstOperator = null;
                foreach(var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if(snr.SNRelationshipType.ToString()==SNRational.MRESULT && snr.Label=="=" && snr.Second==topicNode)
                    {
                        ok = true;
                        firstOperator = snr.First;
                    }
                }
                if(!ok)
                {
                    callback(false, "<" + topicNode.Name + ">不是基本单位，而是导出单位，需要给出其单位演算过程！");
                    return;
                }

                //记录参与运算的基本物理量
                List<string> quantNames = new List<string>();
                foreach(var node in entities)
                {
                    if(node.Name=="/" || node.Name=="^" || node.Name=="*")
                    {
                        CheckOperatorNode(node, relations, callback,ref quantNames); 
                    } 
                }

                ///去除相同的
                quantNames=quantNames.Distinct().ToList();

                foreach(var nd in unitNodes)
                {
                    int i = 0;
                    foreach (var rl in relations)
                    {
                        SNRelationship snr = (SNRelationship)rl;
                        if (snr.First == nd && snr.SNRelationshipType.ToString() == SNRational.ASSOC)
                        {
                            i++;
                        }
                    }
                    if(i!=quantNames.Count)
                    {
                        callback(false, "<" + topicNode.Name + ">演算中涉及"+ quantNames.Count+"个不同的物理量，<" 
                            + nd.Name + ">必须指明" + quantNames.Count + "个对应关联的单位");
                        return;
                    }
                }
            }
        }

        private static void CheckOperatorNode(IEntity node, List<Relationship> relations, Action<bool, string> callback,
            ref List<string> varNames)
        {
            SNRelationship act = null;
            SNRelationship acted = null;
             
            List<SNRelationship> rels = new List<SNRelationship>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if(node.Name=="/" || node.Name=="^")
                {
                    if (snr.First == node && (snr.SNRelationshipType.ToString() == SNRational.ACT || 
                        snr.SNRelationshipType.ToString() == SNRational.ACTED))
                        rels.Add(snr);

                    if (snr.First == node && snr.SNRelationshipType.ToString() == SNRational.ACT)
                        act = snr;
                    if (snr.First == node && snr.SNRelationshipType.ToString() == SNRational.ACTED)
                        acted = snr;
                }
                else if(node.Name=="*")
                {
                    if (snr.First == node && snr.SNRelationshipType.ToString() == SNRational.ACT)
                        rels.Add(snr);

                    if (snr.First == node && snr.SNRelationshipType.ToString() != SNRational.ACT &&
                        snr.SNRelationshipType.ToString() != SNRational.MRESULT)
                    {
                        callback(false, "<*>节点与<" + snr.Second.Name + ">节点只能是ACT连接！");
                        return;
                    }
                }

                if (snr.First == node && (snr.SNRelationshipType.ToString()==SNRational.ACT || snr.SNRelationshipType.ToString()==SNRational.ACTED))
                {
                    if(CheckPhysNode(snr.Second,relations))///是一个物理量节点
                        varNames.Add(snr.Second.Name);
                }
            }

            if (node.Name == "/" || node.Name == "^")
            {
                if (rels.Count != 2)
                {
                    callback(false, "<" + node.Name + ">节点只能有一个ACT和一个ACTED连接！");
                    return;
                }
                if (act == null)
                {
                    callback(false, "<" + node.Name + ">节点必须要一个ACT连接！");
                    return;
                }
                if (acted == null)
                {
                    callback(false, "<" + node.Name + ">节点必须要一个ACTED连接！");
                    return;
                }

            }
            else if(node.Name=="*")
            {
                if (rels.Count < 2)
                {
                    callback(false, "<" + node.Name + ">节点至少要有两个ACT连接！");
                    return;
                }
            } 
        }
 

        /// <summary>
        /// node是连接运算符的一个节点，确认其是物理量还是数值常量
        /// </summary>
        /// <param name="node"></param>
        /// <param name="relations"></param>
        /// <param name="callback"></param>
        private static bool CheckPhysNode(IEntity node, List<Relationship> relations)
        {
            bool isNumerical = false;
            bool isOperator = false;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if(snr.First==node && snr.SNRelationshipType.ToString()==SNRational.IS && snr.Second.Name=="数值")
                {
                    isNumerical = true;
                }
                else if((snr.First == node && (snr.SNRelationshipType.ToString() == SNRational.ACT || snr.SNRelationshipType.ToString() == SNRational.ACTED)))
                    isOperator = true;
            }
            if (isNumerical || isOperator)
            {
                return false;
            }
            else
            {
                return true;
            }
        } 
    }
}
