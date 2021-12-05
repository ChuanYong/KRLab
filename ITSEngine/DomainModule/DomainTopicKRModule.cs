using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using KRLab.Core;
using KRLab.Core.Algorithms.Graphs;
using KRLab.Core.SNet;
using ITS.StudentModule;
using ITSText;

using Utilities; 

namespace ITS.DomainModule
{
    /// <summary>
    /// DomainTopicsKRModule 用以描述一门课程的章节顺序，
    /// is a hierarchical structure of a course
    /// stored in the system. The root of the tree keeps the name of
    /// the course, and is called course node. Subsequently, the different
    /// sections/subsections and topics of the course are kept in the 
    /// lower parts of the tree. The leaf nodes of the tree are called topics
    /// and are the atomic teachable units. Topic nodes are also associated with
    /// different features like concepts(sub-topics), prerequisite topics, 
    /// threshold score, difficulty, and importance.
    /// 在FVCTutor的ITSEngine系统中，一门课程中的学习主题和相关的学习内容作为一个KRLab项目Project做在一起。
    /// </summary>
    public class DomainTopicKRModule:KRModule
    { 

        protected SemanticNet _catalogueNet;
        //课程语义网的目录结点
        protected SNNode _catalogueNode;
        //章结点
        protected Dictionary<string, SNNode> _chapterNodes;
        //小结结点
        protected Dictionary<string, SNNode> _sectionNodes;

        protected Dictionary<string, SemanticNet> _chaptNets; 

        public override object Project
        {
            get
            {
                if(_project==null)
                {
                    string path = FileManager.GetKRSNProjectPath(_course, ProjectType.topicsn);//ProjectType.topicsn
                    if (path == null || !File.Exists(path))
                        return null;
                  
                    KRSNetProject<DomainTopicKRModuleSNet> project = new KRSNetProject<DomainTopicKRModuleSNet>();
                    project.LoadFromFile(path);
                    _project = project;
                   
                }
                return _project;
            }
        }


        public override List<SemanticNet> SNets
        {
            get
            {
                List<SemanticNet> nets = new List<SemanticNet>();
                List<DomainTopicKRModuleSNet> conceptNets = ((KRSNetProject<DomainTopicKRModuleSNet>)Project).NetList;
                foreach (var n in conceptNets)
                    nets.Add(n.Net);
                return nets;
            }
        }


        public SemanticNet CatalogueNet
        {
            get
            {
                if(_catalogueNet==null)
                    _catalogueNet=((KRSNetProject<DomainTopicKRModuleSNet>)Project).GetSNet("目录");
                return _catalogueNet;
            }
        }

        /// <summary>
        /// 指定章名称的语义网，章名称为全名，如，第一章 机械运动
        /// </summary>
        public Dictionary<string,SemanticNet> ChapterNets
        {
            get
            {
                if(_chaptNets==null)
                {
                    _chaptNets = new Dictionary<string, SemanticNet>();
                    List<DomainTopicKRModuleSNet> nets = ((KRSNetProject<DomainTopicKRModuleSNet>)Project).NetList;
                    foreach (var krNet in nets)
                    {
                        if (krNet.Topic != "目录")
                            _chaptNets.Add(krNet.Topic, krNet.Net);
                    }
                }
                return _chaptNets;
            }
        }


        protected SNNode CatalogueNode
        {
            get
            {
                if(_catalogueNode==null)
                    _catalogueNode=CatalogueNet.FastGetNode("目录");
                return _catalogueNode;
            }
        } 

        public int ChapterNumber 
        {
            get { return ChapterNodes.Count;} 
        }


        /// <summary>
        /// 获取目录下面所有的章序号和章名称
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> Chapters
        {
            get
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (var d in ChapterNodes)
                {
                    dict.Add(d.Key, d.Value.Name);
                }
                return dict;

            }

        }

        public List<System.Tuple<string, string>> ListedChapters
        {
            get
            {
                int nb = ChapterNumber;
                List<System.Tuple<string, string>> list = new List<System.Tuple<string, string>>();
                for(int i=1;i<=nb;i++)
                {
                    foreach(var node in ChapterNodes)
                    {
                        if (node.Key.Contains(i.ToString()))
                            list.Add(new System.Tuple<string, string>(node.Key, node.Value.Name));
                    }

                }
                return list;
            }
        } 
        public override List<string> KRAttributes
        {
            get { return new List<string>(); }
        }
         

        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        public DomainTopicKRModule(string course):base(course,KCNames.Catalogue)
        {  
        }

        protected string SectIndex(int i, int j)
        {
            return i.ToString() + "." + j.ToString();
        }

        public SemanticNet GetSNet(string index)
        {
            foreach (var dict in ChapterNets)
            {
                if (dict.Key.Contains(index))
                    return dict.Value;
            }
            return null;
        }

        public SemanticNet GetSNet(int i, int j)
        {
            string index = SectIndex(i, j);
            return GetSNet(index);
        }
        /// <summary>
        /// 知识查询和解析
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="callback"></param>
        public void Parse(string inputStr, Action<bool, string> callback)
        {
            if (inputStr.Contains("课程"))
            {
                string info = Course + "：\n";
                foreach (var tuple in ListedChapters)
                {
                    info += tuple.Item1 + " " + tuple.Item2 + "\n";
                }
                callback(true, info);
                return;
            }
            else if (inputStr.Contains("第") && inputStr.Contains("章") && inputStr.Contains("节"))
            {
                string[] arr = inputStr.Split(new char[] { ' ', '，' });
                string str1 = TextProcessor.StrBetween(arr, "第", "章");
                str1.Trim();
                string str2 = TextProcessor.StrBetween(arr, "第", "节");
                str2.Trim();
                int k1;
                bool ok = int.TryParse(str1, out k1);
                if (!ok || k1 > ChapterNumber)
                {
                    callback(false, "输入的章节有误");
                    return;
                }
                int k2;
                ok = int.TryParse(str2, out k2);
                int nb = GetSectionNumber(k1);
                if (!ok || k2 > nb)
                {
                    callback(false, "第" + k1 + "章只有" + nb + "节!");
                    return;
                }
                string info = "本小节包括的学习课题主要为：\n";
                List<string> topics = GetTopics(k1, k2);
                foreach (var topic in topics)
                {
                    info += topic + "\n";
                }
                callback(true, info);
                return;
            }
            else if (inputStr.Contains("第") && inputStr.Contains("章"))
            {
                int i = inputStr.IndexOf("第");
                string str = inputStr.Substring(i + 1, 1);
                int k;
                bool ok = int.TryParse(str, out k);
                if (!ok || k > ChapterNumber)
                {
                    callback(false, "输入的章节有误");
                    return;
                }

                Dictionary<string, string> sects = GetSections(int.Parse(str));
                string info = "本章包括如下的小节：\n";
                foreach (var dict in sects)
                {
                    info += dict.Key + " " + dict.Value + "\n";
                }
                callback(true, info);
                return;
            }

            Dictionary<string, List<SNNode>> nodeDict;
            ParseChapters(inputStr, out nodeDict);
            List<string> chaptNames = nodeDict.Keys.ToList();

            if(chaptNames.Count>0)
            {
                string info = "查询到的相关结果为：\n";
                if(chaptNames.Count==1 && nodeDict[chaptNames[0]].Count==1)
                {
                    SNNode node = nodeDict[chaptNames[0]][0]; 
                    info += ParseANode(chaptNames[0],node);
                    callback(true, info);
                    return;
                }
                foreach(var dic in nodeDict)
                {
                    info += dic.Key + "：";
                    foreach(var node in dic.Value)
                    {
                        info += node.Name + " ";
                    }
                    info += "\n";
                }
                info += "请再次输入，查找更为详细结果。";
                callback(true, info);
                return;
            }
            else
            {
                callback(false, "你现在学习的课程是<" + Course + ">，请输入较为详细的查询信息。");
            }
        } 

        public void ParseChapters(string inputStr,out Dictionary<string,List<SNNode>> nodes)
        {
            nodes = new Dictionary<string, List<SNNode>>();

            foreach(var net in ChapterNets)
            {
                List<SNNode> tmp= net.Value.SearchNodes(inputStr);
                if (tmp.Count > 0)
                    nodes[net.Key] = tmp; 
            } 
        }

        public string ParseANode(string netName,SNNode node)
        {
            if (node == null)
                return string.Empty;

            string info = string.Empty;

            SemanticNet chaptNet = _chaptNets[netName];
            List<SNNode> nodes = chaptNet.GetOutgoingDestinations(node, SNRational.IS);
            if(nodes.Count>0)//node是一个完整建模的知识点
            {
                List<string> types = new List<string>();
                foreach (var nd in nodes)
                {
                    types.Add(nd.Name);
                }
                foreach (var type in types)
                {
                    string projectType = ProjectType.KCNameToProjectType(type);
                    string path = FileManager.GetKRSNProjectPath(_course, projectType);
                    if (path == null || !File.Exists(path))
                        throw new NetException("没有"+type+"类型的语义项目");

                    info += ParseForDiffProjTypes(type,node.Name)+"\n"; 
                }
                return info;
            }

            SNNode ispNode = chaptNet.GetOutgoingDestination(node, SNRational.ISP);
            if(ispNode!=null)//node是一个没有完整建模的知识点，在其唯一的宿主节点建模了
            {
                List<string> types = new List<string>();
                List<SNNode> tmp = chaptNet.GetOutgoingDestinations(ispNode, SNRational.IS);
                foreach (var t in tmp)
                {
                    if (!types.Contains(t.Name))
                        types.Add(t.Name);
                }
                foreach (var type in types)
                {
                    string path = FileManager.GetKRSNProjectPath(_course, type);
                    if (path == null || !File.Exists(path))
                        throw new NetException("没有" + type + "类型的语义项目");
                    info += ParseForDiffProjTypes(type, ispNode.Name, node.Name) + "\n"; 
                }
                return info;
            }

            return info;
        } 

        protected string ParseForDiffProjTypes(string type,string topicName,string name="")
        { 
            TopicModule topicModule = null;
            type = ProjectType.KCNameToProjectType(type);

            if (type == ProjectType.conceptsn)
            { 
                topicModule = new ConceptTopicModule(new ConceptKRModule(Course),topicName);
            }
            else if(type==ProjectType.consn)
            { 
                topicModule = new ConclusionTopicModule(new ConclusionKRModule(Course),topicName);
            }
            else if(type==ProjectType.equsn)
            {
                throw new NotImplementedException();
                //topicModule = new Equations.SYYCEqus(new EquationKRModule(Course), topicName);
            }
            else if(type==ProjectType.expsn)
            { 
                topicModule = new ExperimentTopicModule(new ExperimentKRModule(Course), topicName);
            }
            else if(type==ProjectType.inssn)
            { 
                topicModule = new InstrumentTopicModule(new InstrumentKRModule(Course), topicName);
            } 
            else if(type==ProjectType.phensn)
            { 
                topicModule = new PhenomenaTopicModule(new PhenomenaKRModule(Course), topicName);
            }
            else if(type==ProjectType.procesn)
            { 
                topicModule = new ProceduralTopicModule(new ProceduralKRModule(Course), topicName);
            }
            else if(type==ProjectType.topicsn)
            { 
                topicModule = new DomainTopicModule(new DomainTopicKRModule(Course), topicName);
            }
            else if(type==ProjectType.untsn)
            { 
                topicModule = new UnitTopicModule(new UnitKRModule(Course), topicName);
            }
            else
            {
                throw new NetException("没有这种类型的语义项目！");
            }

            if (name == "")
                return topicModule.Parse();
            else
                return topicModule.Parse(name);
        }

        /// <summary>
        /// 在每个语义项目中，每个语义网Net的netName或名称都是其文件名。
        /// </summary>
        /// <param name="netName"></param>
        /// <returns></returns>
        public override KRModuleSNet GetKRModuleSNet(string netName)
        {
            if (Project == null)
                return null;

            SemanticNet net =((KRSNetProject<DomainTopicKRModuleSNet>) Project).GetSNet(netName);
            if (net == null)
                return null;
            return new DomainTopicKRModuleSNet(net);
        }

        public override TopicModule CreateTopicModule(string netName)
        { 
            DomainTopicModule topicModule = new DomainTopicModule(this, netName);
            return topicModule;
        }

        #region Topics 

        /// <summary>
        /// 在章节语义网中分为5类节点，一个是最顶端的小节节点，
        /// 小节名称的节点，知识类型节点，学习课题节点,和知识点节点
        /// </summary>
        /// <param name="net"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public int NodeType(SemanticNet net,SNNode node)
        {
            if (node.Name == "小节")
                return 0;
            SNNode topNode = net.FastGetNode("小节");
            SNNode sectNode = net.GetIncomingSource(topNode, SNRational.ISA);
            if (net.HasConnection(node, sectNode, SNRational.ISP))
                return 3;//学习课题节点
            if (sectNode.Name == node.Name)
                return 1;//小节名称的节点
            if (KCNames.Names.Contains(node.Name))
                return 2;//知识类型节点
            return 4; //知识点节点

        }

        protected LearningTopic CreateLearningTopic(SemanticNet net,int i,int j,SNNode topicNode)
        {
            List<string> krTypes = GetKRTypes(net, topicNode);
            Dictionary<int, int> chaptWeights = GetChapterWeights();
            Dictionary<int, int> sectWeights = GetSectWeights(i);
            Dictionary<string, int> topicWeights = GetTopicWeights(i, j);

            SNNode chaptNode = GetChapterNode("第" + i.ToString() + "章");
            ChapterItem chaptItem = GetChapterItem(chaptNode);
            SNNode sectNode = GetSectionNode("第" + i.ToString() + "章", "第" + j.ToString() + "节");
            SectionItem sectItem = GetSectionItem(sectNode);

            Tuple<int, int, int> weights = new Tuple<int, int, int>(chaptWeights[i], sectWeights[j], topicWeights[topicNode.Name]);
            LearningTopic lt = new LearningTopic(Course, chaptItem, sectItem, topicNode.Name, krTypes, weights);
            return lt;
        }

        /// <summary>
        /// 获取某章，某节，某个课题的前续课题,包括ANTE关系和ASSOC关系
        /// </summary>
        /// <param name="i">章序号</param>
        /// <param name="j">小节序号</param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public List<LearningTopic> GetPrequelTopics(int i,int j,string topic)
        {
            SemanticNet net = GetSNet(i, j); 
            if (net == null)
                return new List<LearningTopic>();

            SNNode topicNode = net.FastGetNode(topic);
            if (topicNode == null)
                return new List<LearningTopic>();

            List<SNNode> preNodes = net.GetIncomingSources(topicNode, SNRational.ANTE);
            List<LearningTopic> topics = new List<LearningTopic>();
            foreach (var node in preNodes)
            {
                LearningTopic lt = CreateLearningTopic(net, i, j, node);
                topics.Add(lt);
            } 
            return topics;
        } 



        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public List<LearningTopic> GetPrequelTopics(LearningTopic topic)
        { 
            return GetPrequelTopics(topic.ChaptItem.NumberIndex,topic.SectItem.NumberIndex, topic.Topic);
        } 
       
        public List<LearningTopic> GetTIMEFollowUpTopics(int i,int j, string topic)
        {
            SNNode topicNode = GetTopicNode(i, j, topic);
            if (topicNode == null)
                return null;
            SemanticNet net = GetSNet(i, j);
            List<SNNode> nextNodes = net.GetOutgoingDestinations(topicNode, SNRational.ANTE);
            if (nextNodes.Count == 0)
                return null;

            List<LearningTopic> topics = new List<LearningTopic>();
            foreach (var node in nextNodes)
            {
                LearningTopic lt = CreateLearningTopic(net, i, j, node);
                topics.Add(lt);
            }
            return topics;
        }

        protected SNNode GetChaptNode(int chapter)
        {
            SNNode node = CatalogueNet.FastGetNode("目录");
            List<SNEdge> edges = CatalogueNet.GetIncomingEdges(node, SNRational.ISP);
            foreach(var edge in edges)
            {
                if (TextProcessor.GetNumber(edge.Rational.Label) == chapter)
                    return edge.Source;
            }
            return null;
        }


        protected Dictionary<string, int> GetTopicWeights(int chapter, int sect)
        {
            string index = SectIndex(chapter, sect);
            SemanticNet net = GetSNet(index);
            SNNode node = net.FastGetNode("小节");
            node = net.GetIncomingSource(node, SNRational.ISA);

            List<SNEdge> edges = net.GetIncomingEdges(node, SNRational.ISP);
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (var edge in edges)
            {
                dict[edge.Source.Name] = int.Parse(edge.Rational.Label);
            }
            return dict;
        }
        /// <summary>
        /// 获取某章下面的所有小节的权重
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        protected Dictionary<int,int> GetSectWeights(int chapter)
        {
            SNNode chaptNode = GetChaptNode(chapter);

            Dictionary<int, int> dict = new Dictionary<int, int>();
            List<SNEdge> edges = CatalogueNet.GetIncomingEdges(chaptNode, SNRational.ISP);
            foreach(var edge in edges)
            {
                int index = TextProcessor.GetNumber(edge.Rational.Label);
                dict[index] = int.Parse(edge.Rational.StartMulti);
            }
            return dict;
        }

        /// <summary>
        /// 获取所有章节的权重。
        /// </summary>
        /// <returns>字典的key是章序号，从1开始</returns>
        protected Dictionary<int,int> GetChapterWeights()
        {
            SNNode node = CatalogueNet.FastGetNode("目录");
            List<SNEdge> edges = CatalogueNet.GetIncomingEdges(node, SNRational.ISP);
            Dictionary<int, int> dict = new Dictionary<int, int>();
            foreach(var edge in edges)
            {
                int index = TextProcessor.GetNumber(edge.Rational.Label);
                dict[index] = int.Parse(edge.Rational.StartMulti);
            }
            return dict;
        }

        /// <summary>
        /// 获取sectName节点所在章节的所有小节
        /// </summary>
        /// <param name="sectName"></param>
        /// <returns></returns>
        protected List<SNNode> GetSectNodes(string sectName)
        {
            SNNode node = CatalogueNet.FastGetNode(sectName);
            node = CatalogueNet.GetOutgoingDestination(node, SNRational.ISP);
            List<SNNode> nodes = CatalogueNet.GetIncomingSources(node,SNRational.ISP);
            return nodes;
        }

        public List<LearningTopic> GetTopicsAtSection(int chapter,int sect)
        {
            string sectIndex = SectIndex(chapter, sect);
            SemanticNet net = GetSNet(sectIndex);
            SNNode topNode = net.FastGetNode("小节");
            SNNode sectNode = net.GetIncomingSource(topNode, SNRational.ISA);
            List<SNNode> nodes = net.GetIncomingSources(sectNode,SNRational.ISP);

            Dictionary<int, int> sectWeights = GetSectWeights(chapter);
            Dictionary<int, int> chaptWeights = GetChapterWeights();

            List<LearningTopic> topics = new List<LearningTopic>();
            foreach(var node in nodes)
            {
                LearningTopic lt = CreateLearningTopic(net, chapter, sect, node);
                topics.Add(lt);
            }

            return topics;
        }


        /// <summary>
        /// 获取指定章节的所有学习课题，不包括知识点
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public List<string> GetTopics(int chapter,int sect)
        {
            List<string> topics = new List<string>();
            List<SNNode> nodes = GetTopicNodes(chapter, sect);
            foreach (var n in nodes)
            {
                topics.Add(n.Name);
            }
            return topics;
        }

        /// <summary>
        /// 获取章节下的所有知识类型节点
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        protected List<SNNode> GetKRTypeNodes(int i,int j)
        {
            SemanticNet net = GetSNet(i, j);
            List<SNNode> nodes = new List<SNNode>();
            foreach(var nd in net.Vertices)
            {
                if (KCNames.Names.Contains(nd.Name))
                    nodes.Add(nd);
            }
            return nodes;
        }

        /// <summary>
        /// 获取某个章节下的所有知识点和学习课题的节点
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        protected List<SNNode> GetAllKnowledgeTopics(int i,int j)
        {
            SemanticNet net = GetSNet(i, j);
            List<SNNode> nodes = new List<SNNode>();
            List<SNNode> krNodes = GetKRTypeNodes(i,j);
            foreach(var nd in krNodes)
            {
                List<SNNode> topicNodes = net.GetIncomingSources(nd, SNRational.IS);
                foreach(var td in topicNodes)
                {
                    if (!nodes.Contains(td))
                        nodes.Add(td);
                }
            }

            return nodes;
        }

        /// <summary>
        /// 获取某章，某小节下的无前续课题的课题，即本小节的开始课题
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public List<string> GetStartTopics(int i,int j)
        {
            List<string> results = new List<string>();
            List<string> topics = GetTopics(i,j); 
            foreach (var topic in topics)
            {
                if (GetPrequelTopics(i,j, topic).Count == 0)
                    results.Add(topic);
            }
            return results;
        }

        /// <summary>
        /// 查找章节下面无前续学习课题的结点
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public LearningTopic GetStartTopic(int i,int j)
        {
            List<LearningTopic> topics = GetStartLearningTopics(i,j);
            if (topics.Count == 0)
                return null;
            return topics[0];
        }


        /// <summary>
        /// 查找章节下面无前续学习课题的结点
        /// </summary>
        /// <param name="i">章序号</param>
        /// <param name="j">小节序号</param>
        /// <returns></returns>
        public List<LearningTopic> GetStartLearningTopics(int i,int j)
        {
            List<LearningTopic> results = new List<LearningTopic>(); 
            List<SNNode> topics = GetTopicNodes(i,j);
            SemanticNet net = GetSNet(i, j);
            foreach (var node in topics)
            {
                if (GetPrequelTopics(i,j, node.Name).Count == 0)
                {
                    LearningTopic lt = CreateLearningTopic(net, i, j, node);
                     results.Add(lt);
                }
            }
            return results;
        }

        #endregion


        /// <summary>
        /// 获取某章中的课题以及相关联知识点的名称、知识类型名称、语义网名称。
        /// 获取学习课题的类型及其类型下的子问题。语义网设计中，按知识类型建立语义项目，项目名称
        /// 也就是类型名称，下面包含不同名称的语义网，
        /// 类型下的子问题对应语义项目下的语义网。
        /// </summary>
        /// <param name="chaptIndex"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public List<System.Tuple<string,string>> GetKRTypeAndNameOfTopic(int i,int j,string topic)
        {
            SemanticNet chaptNet = GetSNet(i, j);
            if (chaptNet == null)
            {
                throw new NetException($"在语义项目'{((KRSNetProject<DomainTopicKRModuleSNet>) Project)}'中没有找到名称为{topic}的语义网！");
  
            }

            SNNode node = GetLearningTopicNode(chaptNet, topic);
            if(node==null)
            {
                string index = i.ToString() + "." + j.ToString();
                throw new NetException("在<" +index + ">中没有找到<" + topic + ">的结点！，语义网错误！");
            } 

            IEnumerable<System.Tuple<string,string>> tuples= GetKRModuleName(chaptNet, node);
            return tuples.ToList();
        }

        /// <summary>
        /// 查找名称为topic的学习课题节点，不是小节节点，
        /// 因为有学习课题节点有可能与小节节点同名。
        /// </summary>
        /// <param name="net">对应学习小节的语义网络</param>
        /// <param name="topic"></param>
        /// <returns></returns>
        private SNNode GetLearningTopicNode(SemanticNet net,string topic)
        {
            List<SNNode> nodes = net.GetNodes(topic);
            foreach(var node in nodes)
            {
                SNNode nd = net.GetOutgoingDestination(node, SNRational.ISA);//
                if(nd!=null && nd.Name=="小节")
                {
                    continue;
                }
                else
                {
                    return node;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取学习课题的知识类型，这样可以到相应的类型语义项目中查找
        /// 详细的内容。某个知识点有可能是作为另外一个知识点的一部分进行定义的，没有单独建立
        /// 相应的语义网
        /// </summary>
        /// <param name="net">章节语义网</param>
        /// <param name="topicNode"></param>
        /// <returns>知识类型和语义网名称的列表</returns>
        private List<System.Tuple<string,string>> GetKRModuleName(SemanticNet net, SNNode topicNode)
        {
            if (topicNode == null)
                throw new Exception("没有指定学习课题！"); 

            List<SNEdge> edges = net.GetOutgoingEdges(topicNode, SNRational.IS,SNRational.ISP);
            List<System.Tuple<string, string>> tuples = new List<System.Tuple<string, string>>();
            foreach(var edge in edges)
            {
                if (edge.Rational.Rational == SNRational.ISP &&
                    net.GetOutgoingEdges(edge.Destination,SNRational.IS).Count>0)
                {
                    tuples.AddRange(_GetKRModuleName(net, edge.Destination));
                }
                else if(edge.Rational.Rational==SNRational.IS)
                {
                    System.Tuple<string, string> tp = new System.Tuple<string, string>(edge.Destination.Name,
                    topicNode.Name);
                    tuples.Add(tp);
                }

            }
            return tuples;
        }

        private List<System.Tuple<string, string>> _GetKRModuleName(SemanticNet net, SNNode topicNode)
        { 
            List<SNEdge> edges = net.GetOutgoingEdges(topicNode, SNRational.IS);
            List<System.Tuple<string, string>> tuples = new List<System.Tuple<string, string>>();
            foreach (var edge in edges)
            {
                System.Tuple<string, string> tp = new System.Tuple<string, string>(edge.Destination.Name,
                topicNode.Name);
                tuples.Add(tp);
            }
            return tuples;
        }

        /// <summary>
        /// 获取章节的名称
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <param name="sectionIndex"></param>
        /// <returns></returns>
        public System.Tuple<string, string> GetSectionName(int chapterIndex, int sectionIndex)
        {
            if (chapterIndex <= 0 || sectionIndex <= 0)
                throw new ArgumentException("章节序号应该从1开始");
            chapterIndex -= 1;

            string chaptName = "第" + chapterIndex.ToString() + "章";
            string sectName = "第" + sectionIndex.ToString() + "节";
            return GetSectionName(chaptName, sectName);
        }


        public int GetSectionNumber(int chapterIndex)
        {
            return GetSections(chapterIndex).Count;
        } 

        public int GetSectionNumber(string chapterIndex)
        {
            return GetSections(chapterIndex).Count;
        }

        public string GetChapterName(int index)
        {
            if (index <= 0)
                throw new ArgumentException("输入的章节序号从1开始");
             
            string name = "第" + index.ToString() + "章";
            return GetChapterName(name);
        }

        /// <summary>
        /// 获取指定章序号的章名称
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <returns></returns>
        public string GetChapterName(string chapterIndex)
        {
            SNNode node;
            if (!ChapterNodes.TryGetValue(chapterIndex, out node))
            {
                return string.Empty;
            }
            return node.Name;
        }

        public System.Tuple<string,string> GetSectionName(string chapterIndex,string sectionIndex)
        {
            Dictionary<string,SNNode> dict= GetSectionNodes(ChapterNodes[chapterIndex]);
            if (!dict.Keys.Contains(sectionIndex))
                return new System.Tuple<string,string>(ChapterNodes[chapterIndex].Name,string.Empty);
            else
                return new System.Tuple<string, string>(ChapterNodes[chapterIndex].Name,
                    dict[sectionIndex].Name) ;
        }

        public Dictionary<string, string> GetSections(int chapterIndex)
        {
            if (chapterIndex <= 0)
                throw new ArgumentException("输入的章节序号从1开始");
             
            string name = "第" + chapterIndex.ToString() + "章";
            return GetSections(name);
        }

        /// <summary>
        /// 获取某章下面的所有小节序号和名称
        /// </summary>
        /// <param name="chapterIndex">第一章，第二章，....</param>
        /// <returns></returns>
        public Dictionary<string,string> GetSections(string chapterIndex)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Dictionary<string, SNNode> secNodes = GetSectionNodes(ChapterNodes[chapterIndex]);
            foreach(var s in secNodes)
            {
                dict.Add(s.Key, s.Value.Name);
            }
            return dict;
        }


        /// <summary>
        ///  获取本课程的下一章
        /// </summary>
        /// <param name="chapter">第1章、第2章、....</param>
        /// <returns></returns>
        public ChapterItem GetNextChapter(ChapterItem chapter)
        {
            int i = chapter.NumberIndex;
            if (ChapterNumber > i)
            {
                string index = "第" + (i + 1).ToString() + "章";
                SNNode chaptNode = ChapterNodes[index];
                ChapterItem item = new ChapterItem(index, chaptNode.Name);
                return item;
            }
            else
                return null; 
        }

        public SectionItem GetSection(ChapterItem chapter,string sectIndex)
        {
            SNNode node = GetSectionNode(chapter.Index, sectIndex);
            return new SectionItem(sectIndex, node.Name);
        }

        /// <summary>
        /// 获取某章，section的下一小节
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public SectionItem GetNextSection(ChapterItem chapter,SectionItem section)
        {
            int nb = GetSectionNumber(chapter.Index);
            List<string> names = DomainTopicKRModuleSNet.SectionNames;

            if (names.Count < nb)
                throw new NetException("小节数目大于既定的小节名称！");

            int index = names.IndexOf(section.Index);
            if (index < nb - 1)
            {
                string indexName = names[index + 1];
                SNNode node = GetSectionNode(chapter.Index, indexName);
                return new SectionItem(indexName, node.Name);
            }
            else
                return null; 
        }

        public ChapterItem GetChapterItem(SNNode chapterNode)
        {
            SNEdge edge = CatalogueNet.GetOutgoingEdges(chapterNode, SNRational.ISP)[0];
            return new ChapterItem(edge.Rational.Label, chapterNode.Name);
        }


        #region Chapter node
     
        /// <summary>
        /// 获取目录结点下的所有章结点,string不是章的全名，而是章序号：第1章等 
        /// </summary>
        protected Dictionary<string,SNNode> ChapterNodes
        {
            get
            {
                if (_chapterNodes != null)
                    return _chapterNodes;
                else
                {
                    List<SNEdge> edges = CatalogueNet.GetIncomingEdges(CatalogueNode);
                    _chapterNodes = new Dictionary<string, SNNode>();
                    foreach (var tp in edges)
                    {
                        if (tp.Rational.Rational == SNRational.ISP)
                            _chapterNodes.Add(tp.Rational.Label, tp.Source);
                    }
                    return _chapterNodes;

                }
            }
        }

        /// <summary>
        /// 获取某个小节结点的章结点
        /// </summary>
        /// <param name="sectNode"></param>
        /// <returns></returns>
        protected SNNode GetChapterNodeOfSect(SNNode sectNode)
        {
            if (sectNode == null)
                return null;

            List<SNNode> nodes = CatalogueNet.GetOutgoingDestinations(sectNode, SNRational.ISP);
            if (nodes.Count == 0)
                return null;
            return nodes[0];
        }

        protected SNNode GetChapterNode(ChapterItem chapter)
        {
            SNNode node;
            if (!ChapterNodes.TryGetValue(chapter.Index, out node))
                return null;
            else
                return node;
        }
        protected SNNode GetChapterNode(string chapterIndex)
        {
            SNNode node;
            if (!ChapterNodes.TryGetValue(chapterIndex, out node))
                return null;
            else
                return node;
        }
        #endregion

        #region Section Nodes

        protected SNNode GetSectNode(SemanticNet net)
        {
            SNNode topNode = net.FastGetNode("小节");
            SNNode sectNode = net.GetIncomingSource(topNode, SNRational.ISA);
            return sectNode;
        }

        protected SNNode GetSectNode(int i,int j)
        {
            string index = i.ToString() + "." + j.ToString();
            SemanticNet net = GetSNet(index);
            return GetSectNode(net);
        }

        public SectionItem GetStartSectionItem(ChapterItem chapter)
        {
            int i = chapter.NumberIndex;
            string index = i.ToString() + ".1";
            SemanticNet net = GetSNet(index);
            if (net == null)
                return null;

            SNNode chaptNode = GetChapterNode(chapter);
            if (chaptNode == null)
                return null;
            List<SNEdge> edges = CatalogueNet.GetIncomingEdges(chaptNode, SNRational.ISP);
            foreach(var edge in edges)
            {
                if(edge.Rational.Label=="第1节")
                {
                    return new SectionItem(edge.Rational.Label, edge.Source.Name);
                }
            }
            return null;
        }

        protected SectionItem GetSectionItem(SNNode sectNode)
        {
            SNEdge edge = CatalogueNet.GetOutgoingEdges(sectNode, SNRational.ISP)[0];
            return new SectionItem(edge.Rational.Label, sectNode.Name);
        } 


        protected SectionItem GetSectionItem(string chaptIndex,string sectItem)
        {
            SNNode node = GetSectionNode(chaptIndex, sectItem);
            return GetSectionItem(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chaptIndex">格式为，第1章</param>
        /// <param name="sectIndex">格式为，第1节</param>
        /// <returns></returns>
        protected SNNode GetSectionNode(string chaptIndex,string sectIndex)
        {
            SNNode chaptNode = ChapterNodes[chaptIndex];
            Dictionary<string, SNNode> nodes = GetSectionNodes(chaptNode);
            if (nodes == null)
                return null;

            return nodes[sectIndex];
        }
        protected SNNode GetSectionNode(ChapterItem chapter, SectionItem sect)
        {
            SNNode chaptNode = ChapterNodes[chapter.Index];
            Dictionary<string, SNNode> nodes = GetSectionNodes(chaptNode);
            if (nodes == null)
                return null;

            return nodes[sect.Index];
        }

        /// <summary>
        /// 查询某个章结点下的所有小节结点
        /// </summary>
        /// <param name="chapter">章结点</param> 
        /// <returns>小节序号+小节名称</returns>
        protected Dictionary<string,SNNode> GetSectionNodes(SNNode chapNode)
        {
            if (chapNode == null)
                return null;

            List<SNEdge> edges = CatalogueNet.GetIncomingEdges(chapNode);
            Dictionary<string, SNNode> sectionNodes = new Dictionary<string, SNNode>();
            foreach (var tp in edges)
            {
                if (tp.Rational.Rational == SNRational.ISP)
                    sectionNodes[tp.Rational.Label]=tp.Source;
            }

            return sectionNodes;
        }
        #endregion


        #region Topic nodes
         

        /// <summary>
        /// 获取学习课题节点topicNode的知识类型
        /// </summary>
        /// <param name="topicNode"></param>
        /// <returns></returns>
        protected List<string> GetKRTypes(SemanticNet net, SNNode topicNode)
        {
            List<string> types = new List<string>();  

            List<SNNode> nodes = net.GetOutgoingDestinations(topicNode, SNRational.IS);
            foreach(var node in nodes)
            {
                types.Add(node.Name);
            }

            return types;
        }

        protected List<SNNode> AllNeighboursInSection(SemanticNet net,SNNode node)
        {
            if (NodeType(net,node) != 3)
            {
                return null;
            }
            else
                return net.AllNeighbours(node).ToList();
        }
         

        public LearningTopic FindFirstMatch(int chapter,int sect,Func<LearningTopic,bool> match)
        {
            SemanticNet net = GetSNet(chapter, sect);

            List<LearningTopic> topics = GetTopicsAtSection(chapter, sect);            

            LearningTopic startTopic = topics[0];
            SNNode startNode = net.FastGetNode(startTopic.Topic);
            if (startNode == null)
                return null; 

            int level = 0;													// keeps track of levels
            var frontiers = new List<SNNode>();									// keeps track of previous levels, i - 1
            var levels = new Dictionary<SNNode, int>(net.VerticesCount);		// keeps track of visited nodes and their distances
            var parents = new Dictionary<SNNode, object>(net.VerticesCount);	// keeps track of tree-nodes

            frontiers.Add(startNode);
            levels.Add(startNode, 0);
            parents.Add(startNode, null);

            // BFS VISIT CURRENT NODE
            if (match(startTopic))
                return startTopic;

            // TRAVERSE GRAPH
            while (frontiers.Count > 0)
            {
                var next = new List<SNNode>();									// keeps track of the current level, i

                foreach (var node in frontiers)
                {
                    List<SNNode> neighbours = AllNeighboursInSection(net,node);
                    if (neighbours == null) continue;
                    foreach (var adjacent in neighbours)
                    {
                        if (!levels.ContainsKey(adjacent)) 				// not visited yet
                        {
                            // 如果结点是学习课题结点
                            if (NodeType(net,adjacent) == 3)
                            { 
                                LearningTopic newTopic = CreateLearningTopic(net,chapter,sect,adjacent);
                                if (match(newTopic))
                                    return newTopic;
                            }

                            levels.Add(adjacent, level);					// level[node] + 1
                            parents.Add(adjacent, node);
                            next.Add(adjacent);
                        }
                    }
                }

                frontiers = next;
                level = level + 1;
            }
            return null;
        }
        
        protected List<SNNode> GetFollowUpNodes(SNNode topicNode)
        {
            if (topicNode == null)
                return new List<SNNode>();
            List<SNNode> nodes = CatalogueNet.GetOutgoingDestinations(topicNode, SNRational.ANTE);
            return nodes;
        }

        protected SNNode GetTopicNode(int i,int j,string topic)
        {
            string sectIndex = SectIndex(i, j);
            SemanticNet net = GetSNet(sectIndex);
            if (net == null)
                return null;
            SNNode topNode = net.FastGetNode("小节");
            SNNode sectNode = net.GetIncomingSource(topNode, SNRational.ISA);
            List<SNNode> nodes = net.GetIncomingSources(sectNode, SNRational.ISP);
            SNNode node = nodes.Find(target => target.Name == topic);
            return node; 
        }

        protected SNNode GetTopicNode(LearningTopic topic)
        {
            return GetTopicNode(topic.ChaptItem.NumberIndex, topic.SectItem.NumberIndex, topic.Topic);
        } 

        /// <summary>
        /// 获取某个小节下的所有知识点
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        protected List<SNNode> GetTopicNodes(int i,int j)
        { 
            string sectIndex = SectIndex(i, j);
            SemanticNet net = GetSNet(sectIndex);
            SNNode topNode = net.FastGetNode("小节");
            SNNode topicNode = net.GetIncomingSource(topNode,SNRational.ISA);
            List<SNNode> nodes = net.GetIncomingSources(topicNode,
                SNRational.ISP);

            return nodes;
        }

        #endregion

        #region Utilities
        protected string IntToStrChapter(int i)
        {
            if (i <= 0)
                throw new ArgumentException("输入的章节序号从1开始");
             
            string chapterName = "第" + i.ToString() + "章";

            return chapterName;

        }
        protected string IntToStrSection(int i)
        {
            if (i <= 0)
                throw new ArgumentException("输入的章节序号从1开始");

            string name = "第" + i.ToString() + "节";

            return name;

        }

        protected System.Tuple<string, string> IntToStr(int chaptIndex, int sectIndex)
        {

            if (chaptIndex <= 0 || sectIndex <= 0)
                throw new ArgumentException("输入的章节序号从1开始");
             
            string chapterName = "第" + chaptIndex.ToString() + "章";
            string name = "第" + sectIndex.ToString() + "节";

            return new System.Tuple<string, string>(chapterName, name);
        }

        #endregion
    }
}
