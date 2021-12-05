using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
 
using Utilities;
using ITSText;

namespace KRLab.Core.SNet
{
    /*
     *该类对应课程语义项目中的目录语义网和章节语义网
     * （1）目录语义网标注一门课程的目录结构，以及相关的学习课题，如下图所示
     *                     
     *                     第1章             第2章                 第2节
     *       【chapter1】-------->【目录】<---------【chapter2】<---------【小节2】
     *                      ISP                         /|\         ISP 
     *                                                   |第1节  
     *                                                   |       
     *                                                   |          
     *                                               【 小节1 】                                                        
     *                                                          
     *  (a)各章结点与目录用ISP连接，并在连接线上标注第?章，比如第1章，第2章，
     *  (b)各小节结点与章结点以ISP连接，并在连接线上标注第？节，比如第1节，第2节，
     *  (c)目录结点与课程名称结点以ASSOC连接，课程名称结点以IS与课程类别结点连接。
     *     
     * （2）章节语义网标注一门课程中某一章所有学习课题的知识类型，如下图所示
     *           
     *                    【 小节1 】<--------【课题1】------>【概念】
     *                        /|\    \       /
     *                         |ISP   \     /
     *                         |       \   /      
     *                      【课题2】-->【课题3】
     * 
     *         【知识点1】------>【概念】<-------【知识点2】
     *                     IS      /|\     IS
     *                              |IS
     *                              | 
     *                          【知识点3】
     *                           
     *          【知识点4】----->【结论】<-------【知识点5】
     *                     IS      /|\     IS
     *                              |IS
     *                              | 
     *        【知识点6】 ------->【知识点7】<-------【知识点8】                         
     *                   ISP        /|\   \   ISP
     *                               |ISP _\|
     *                               |    【概念】
     *                            【知识点9】
     *   
     *  (a)每个小节下的学习课题与小节结点以ISP关系连接，连接线的标签注明学习课题的权重，
     *     一个小节下的所有权重加起来等于100。
     *  (b)各学习课题结点之间以ANTE关系连接，以表示其学习顺序，箭头指向的为后续课题。 
     *  (c)目前，知识类型主要分为概念、单位、测量、结论、公式、算法，知识类型结点下的
     *      结点必须以IS关系连接。
     *  (d)每个课题进行建模的时候，可以指明依赖的知识点，知识点与学习课题不同。学习课题
     *     是某个小节下一级的标题，而知识点是标题下面内容所涉及的各个相对独立的知识。
     *     依赖的知识点必须在相应的知识类型项目中进行了建模。这些知识点应该是属于本门课程的，
     *     不应该是其它课程的。此外，这些知识点应该是该学习课题之前的知识点，不是后续的知识点。
     * （e）每个学习课题或知识点，可以是属于多个知识类型，比如上面的【课题4】是结论，也是概念。
     *  (f)为了查询方便，学习课题结点下可以指明涉及的新知识点（前面章节没有出现的），
     *     比如上面的【知识点1】，【知识点2】和【知识点3】是【课题4】的一部分，
     *     而【课题4】的知识类型是一个结论。这些新知识点也必须进行建模。
     */

    public class DomainTopicKRModuleSNet : KRModuleSNet
    {
        public static List<string> ChapterNames = new List<string>()
        {
            "第1章","第2章","第3章","第4章","第5章","第6章","第7章","第8章","第9章","第10章","第11章","第12章",
            "第13章","第14章","第15章","第16章","第17章","第18章","第19章","第20章","第21章","第22章","第23章",
            "第24章","第25章","第26章","第27章","第28章"
        };
        public static List<string> SectionNames = new List<string>()
        {
            "第1节","第2节","第3节","第4节","第5节","第6节","第7节","第8节","第9节","第10节","第11节","第12节",
            "第13节","第14节","第15节","第16节","第17节","第18节","第19节","第20节","第21节","第22节","第23节",
            "第24节","第25节","第26节"
        };
        
        public DomainTopicKRModuleSNet(SemanticNet net) : base(net,KCNames.Catalogue)
        { 
        }
        public override string KRType => ProjectType.topicsn;

        public override void CheckAndInit()
        {
            if (Topic == "目录")
            {
                SNNode krNode = Net.FastGetNode("目录");
                if (krNode == null)
                    throw new NetException("没有'目录'节点");
                //各章的结点
                List<SNNode> nodes = Net.GetIncomingSources(krNode, SNRational.ISP);
                if (nodes.Count == 0)
                    throw new Exception("没有课程章结点！"); 
            }
     


            //List<SNNode> knowledgeNodes = Net.GetIncomingSources(_topicNode, SNRational.ISA);
            //if (knowledgeNodes.Count < 1)
            //{
            //    throw new Exception("没有相应的知识节点！");
            //}
        }

        /// <summary>
        /// 某个章节下的所有学习课题
        /// </summary> 
        ///必须先建立目录语义网 
        public static void Check(string netName, List<IEntity> entities, List<Relationship> relations, Action<bool, string> callback)
        {
            CheckCatalogue(netName, entities, relations, callback);

            CheckSection(netName, entities, relations, callback);
        }

        protected static void CheckSection(string netName,List<IEntity> entities, List<Relationship> relations,
            Action<bool, string> callback)
        {
            if (netName == "目录")
                return;              
             
            IEntity topNode = null;
            foreach (var node in entities)
            {
                if (node.Name == "小节")
                {
                    topNode = node;
                    break;
                }
            }
            if(topNode==null)
            {
                callback(false, "<"+netName+">语义网中没有<小节>节点！");
                return;
            }

            IEntity sectNode = null;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.Second == topNode && snr.SNRelationshipType.ToString() != SNRational.ISA)
                {
                    callback(false, "到达<" + topNode.Name + ">结点的必须是ISA连接，因为其是小节结点！");
                    return;
                }
                if (snr.Second == topNode && snr.SNRelationshipType.ToString() == SNRational.ISA)
                {
                    sectNode = snr.First;
                    break;
                }
            }

            int x = 0;
            List<IEntity> topicNodes = new List<IEntity>();
            foreach (var rl in relations)
            { 
                SNRelationship snr = (SNRelationship)rl;
                if (snr.Second == sectNode && snr.SNRelationshipType.ToString() != SNRational.ISP)
                {
                    callback(false, "到达<" + sectNode.Name + ">结点的必须是ISP连接，因为其是小节结点！");
                    return;
                } 
                if (snr.Second == sectNode && snr.SNRelationshipType.ToString() == SNRational.ISP)
                {
                    topicNodes.Add(snr.First);
                }
                if (snr.Second == sectNode && snr.SNRelationshipType.ToString() == SNRational.ISP)
                {
                    int i;
                    if (!int.TryParse(rl.Label, out i))
                    {
                        callback(false, "<" + sectNode.Name + ">下面的连接必须标注数字，表示小节中学习课题的权重！");
                        return;
                    }
                    x += i;
                }
            }
            if (x != 100)
            {
                callback(false, "<" + sectNode.Name + ">小节下面的学习课题的权重加起来必须等于100！");
                return;
            }

            ///检查学习课题之间是否用ANTE关系连接
            foreach(var nd in topicNodes)
            {
                bool isOk = false;
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if (snr.SNRelationshipType.ToString() == SNRational.ANTE && (snr.First==nd || snr.Second==nd))
                    {
                        isOk = true;
                    }
                }
                if(!isOk && topicNodes.Count>1)
                { 
                    callback(false, "<" + nd.Name + ">结点必须有ANTE关系连接其它学习课题节点，表示学习的次序！");
                    return;
                }
            }

            List<IEntity> nodes = new List<IEntity>();
            foreach (var node in entities)
            {
                if (!KCNames.Names.Contains(node.Name) && node != topNode && node != sectNode)
                    nodes.Add(node);
            }

            foreach(var node in nodes)
            {
                bool ok = false;
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if (snr.First == node && ((KCNames.Names.Contains(snr.Second.Name) && snr.SNRelationshipType.ToString() == SNRational.IS)
                        || snr.SNRelationshipType.ToString()==SNRational.ISP))
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    callback(false, "<" + node.Name + ">结点必须有以IS关系连接知识类型结点,\n 或以ISP连接其它节点！");
                    return;
                }
            }


            ////章节语义网中出现的知识点或学习课题必须在某个类型语义项目中进行语义网建模  
            _topicDict = new Dictionary<string, List<IEntity>>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if(KCNames.Names.Contains(snr.Second.Name) && snr.SNRelationshipType.ToString()==SNRational.IS)
                {
                    if(!_topicDict.ContainsKey(snr.Second.Name))
                    {
                        _topicDict[snr.Second.Name] = new List<IEntity>() { snr.First };
                    }
                    else
                    {
                        _topicDict[snr.Second.Name].Add(snr.First);
                    }
                }
            } 

            ///检查_topicDict中的学习课题和知识点是否是另外一个知识点的一部分，
            ///如果是，则该知识点不需要额外构建一个单独的语义网进行定义。不加入到_topicDict中
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;  
                if (snr.SNRelationshipType.ToString() == SNRational.ISP && snr.Second!= sectNode)
                {
                    foreach(var dic in _topicDict)
                    {
                        dic.Value.Remove(snr.First);
                    }
                }
            }
        }

        static Dictionary<string, List<IEntity>> _topicDict;

        public static Dictionary<string, List<IEntity>> TopicDict
        {
            get { return _topicDict; }
        }


        static Dictionary<int, Dictionary<int, IEntity>> _sects=new Dictionary<int, Dictionary<int, IEntity>>();

        public static List<string> GetSectionts()
        {
            List<string> names = new List<string>();
            foreach (var dict in _sects)
            {
                foreach (var sect in dict.Value)
                {
                    names.Add(dict.Key.ToString() + "." + sect.Key.ToString() + "-" + sect.Value.Name);
                }
            }
            return names;
        }

        /// <summary>
        ///课程语义项目中目录语义网的语法要求 
        /// </summary>
        /// <param name="netName"></param>
        /// <param name="entities"></param>
        /// <param name="relations"></param>
        /// <param name="topicDict">每个章节下所有的学习课题</param>
        /// <param name="callback"></param>
        protected static void CheckCatalogue(string netName, List<IEntity> entities, List<Relationship> relations,
            Action<bool, string> callback)
        {
            if (netName != "目录")
                return;

            BasicSemanticNode catalogueNode = null;
            foreach (var node in entities)
            {
                if (node.Name == KCNames.Catalogue)
                {
                    catalogueNode = (BasicSemanticNode)node;
                }
            }

            if (catalogueNode == null)
            {
                callback(false, "没有" + KCNames.Catalogue + "结点");
                return;
            }

            BasicSemanticNode courseNode = null;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;
                if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.First == catalogueNode &&
                    snr.Label=="课程名称")
                    courseNode = (BasicSemanticNode)snr.Second;
            }
            if (courseNode == null)
            {
                callback(false, "目录结点应该用一个标有<课程名称>的<ASSOC>的连接与课程结点关联起来！");
                return;
            }

            BasicSemanticNode subjectNode = null;
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;

                if (snr.SNRelationshipType.ToString() == SNRational.IS && snr.First == courseNode)
                {
                    subjectNode = (BasicSemanticNode)snr.Second;
                }
            }
            if (subjectNode == null)
            {
                callback(false, "课程结点应该与一个标有学科名称的'学科结点'相连接：\n用'IS'关系连接");
                return;
            }

            int cx = 0;

            Dictionary<int,IEntity> chaptNodes = new Dictionary<int, IEntity>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;

                if (snr.Second == catalogueNode && snr.SNRelationshipType.ToString() != SNRational.ISP)
                {
                    callback(false, snr.First.Name + "结点指向目录结点的连接只能是ISP关系！");
                    return;
                }

                if (snr.SNRelationshipType.ToString() == SNRational.ISP)
                {
                    if (snr.First == catalogueNode)
                    {
                        callback(false, snr.Second.Name + "结点与目录结点的连接方向错误：\n从章结点以ISP关系指向目录结点！");
                        return;
                    }
                    if (snr.Second == catalogueNode)
                    {
                        if (!DomainTopicKRModuleSNet.ChapterNames.Contains(snr.Label))
                        {
                            callback(false, "章结点'" + snr.First.Name + "'与目录结点之间的连接应该严格标注为：\n第1章、第2章、第3章...");
                            return;
                        }
                        int nb = TextProcessor.GetNumber(snr.Label);
                        chaptNodes[nb]=snr.First;

                    }
                    if (snr.Second == catalogueNode && snr.SNRelationshipType.ToString() == SNRational.ISP)
                    {
                        int i;
                        if (!int.TryParse(snr.StartMultiplicity, out i))
                        {
                            callback(false, "<" + catalogueNode.Name + ">目录下面的连接必须标注章权重的数值！");
                            return;
                        }
                        cx += i;
                    }

                }
            }
            if (cx != 100)
            {
                callback(false, "<" + catalogueNode.Name + ">下面的章权重加起来必须等于100！");
                return;
            }
            if (chaptNodes.Count == 0)
            {
                callback(false, "缺少与'目录结点'用ISP关系连接的章结点！");
                return;
            }
             
            ///蛮力遍历！！！！
            foreach (var node in chaptNodes)
            {
                int x = 0;
                Dictionary<int, IEntity> sectNodes = new Dictionary<int, IEntity>();
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;
                    if (snr.Second == node.Value && snr.SNRelationshipType.ToString() != SNRational.ISP)
                    {
                        callback(false, "<" + snr.First.Name + ">结点指向'" + node.Key + "'结点的连接不是ISP关系!");
                        return;
                    }
                    if (snr.Second == node.Value && !DomainTopicKRModuleSNet.SectionNames.Contains(snr.Label))
                    {
                        callback(false, "小节结点<" + snr.First.Name + ">与章结点之间的连接应该严格标注为：\n第1节、第2节、第3节...");
                        return;
                    }
                    if (snr.First == node.Value && snr.Second != catalogueNode)
                    {
                        callback(false, node.Key + "结点除了指向目录结点不指向其它结点！");
                        return;
                    }
                    if (snr.Second == node.Value && snr.SNRelationshipType.ToString() == SNRational.ISP)
                    {
                        int nb = TextProcessor.GetNumber(snr.Label);
                        sectNodes[nb]=snr.First;
                    }
                    if (snr.Second == node.Value && snr.SNRelationshipType.ToString() == SNRational.ISP)
                    { 
                        int i;
                        if (!int.TryParse(snr.StartMultiplicity, out i))
                        {
                            callback(false, "<" + node.Key + ">下面的连接必须标注小节的权重数值！");
                            return;
                        }
                        x += i;
                    }
                }
                _sects[node.Key] = sectNodes;

                if (x != 100)
                {                 
                    callback(false, "<第" + node.Key + "章>下面的小节权重加起来必须等于100！");
                    return;
                }
                if (sectNodes.Count == 0)
                {
                    callback(false, "第<"+node.Key + "章>下面没有指定相应的小节");
                    return;
                }
            }
        }      
    }
}
