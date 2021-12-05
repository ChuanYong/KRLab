using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using KRLab.Core.SNet;
using KRLab.Core;
using Utilities;

namespace ITS.DomainModule
{
     public class ConceptKRModule:KRModule
    {
        public  List<string> _nameString;
        public List<double> _similaList;
        public List<SNNode> _nodeList;
        public Dictionary<int[], double> _dic2;//存放两两之间的相似性


        public override object Project
        {
            get
            {
                if(_project==null)
                {
                    string path = FileManager.GetKRSNProjectPath(_course, ProjectType.conceptsn);
                    if (path==null || !File.Exists(path))
                        return null;
                    KRSNetProject<ConceptKRModuleSNet> project = new KRSNetProject<ConceptKRModuleSNet>();
                    project.LoadFromFile(path);

                    _project = project;
                }
                //获取当前目录所有的概念
                string conceptPath = FileManager.GetKRSNProjectPath(_course, ProjectType.conceptsn);
                _nameString = new List<string>();
                _nodeList = new List<SNNode>();
                if (conceptPath != null && File.Exists(conceptPath))
                {
                    //先加载概念语义网
                    KRSNetProject<ConceptKRModuleSNet> conpPro = new KRSNetProject<ConceptKRModuleSNet>();                   
                    conpPro.LoadFromFile(conceptPath);                   
                    foreach (var net1 in conpPro.NetList)
                    {
                        if (net1.Topic == "数")
                        {
                            //KRModuleSNet kr = new KRModuleSNet(conpPro.GetSNet(SNetNames[i]));
                            ConceptKRModule krm = new ConceptKRModule(_course);
                            TopicModule tm = new TopicModule(_course, net1);//krm.GetKRModuleSNet(names[i])
                            SemanticNet net = tm.KRModuleSNet.Net;

                            foreach (var e in net.Edges)
                            {
                                SNEdge edge = (SNEdge)e;
                                SNNode firstNode = edge.Source;
                                SNNode secondNode = edge.Destination;
                                string first = edge.Source.Name;
                                string second = edge.Destination.Name;
                                string rational = edge.Rational.Rational;
                                string label = edge.Rational.Label;                              
                                
                                //该部分是数的选择题的
                                if (rational == SNRational.IS && second == "数")//类型是is并且目的节点是数
                                {
                                    _nameString.Add(first);//将起始节点存入namestring
                                    //List<SNNode> nodeList = new List<SNNode>();
                                    _nodeList.Add(firstNode);//临时变量存放和数is相连的节点
                                    int length = _nameString.Count;
                                    Random rand = new Random();
                                    int randIdx = 0;
                                    for (int j = 0; j < length; j++)
                                    {
                                        randIdx = rand.Next(j, length);
                                        string temp = string.Empty;
                                        temp = _nameString[j];
                                        _nameString[j] = _nameString[randIdx];
                                        _nameString[randIdx] = temp;
                                    }
                                }
                                //计算两两之间的相似性存入列表，所有选择题通用
                                List<int> edgeNum = new List<int>();//存放两两之间的最短路径，有关系是1，无关系是2
                                Dictionary<int[], string> dic = new Dictionary<int[], string>();//使用一个字典存放两两对应的关系
                                Dictionary<int[], int> dic1 = new Dictionary<int[], int>();//存放两两之间的最短路径
                                _dic2 = new Dictionary<int[], double>();//存放两两之间的相似性

                                for (int i = 0; i < _nodeList.Count-1; i++)
                                {
                                    for(int j = 0; j < _nodeList.Count; j++)
                                    {
                                        int[] betw ={ i,j};
                                        //if (firstNode == _nodeList[i] && secondNode == _nodeList[j])
                                        //{
                                            if(edge.Rational.Label == "SYMM"||edge.Rational.Label=="COMPL"|| 
                                                edge.Rational.Label=="ISP"|| edge.Rational.Label=="ANLG")
                                            {
                                                string edgeRelation = edge.Rational.Label;
                                                dic.Add(betw, edgeRelation);
                                                dic1.Add(betw, 1);
                                                double simiOn = Math.Log10(4 / 1);//计算最短路径为1的相似度
                                                
                                                if(edge.Rational.Label=="SYMM")//给已经计算出的相似度根据连接关系，加上不同的权重
                                                {
                                                    double weightHad = simiOn*1.4;
                                                    _dic2.Add(betw,weightHad);
                                                }
                                                if (edge.Rational.Label == "COMPL")
                                                {
                                                    double weightHad = simiOn * 1.5;
                                                    _dic2.Add(betw, weightHad);
                                                }
                                                if (edge.Rational.Label == "ISP")
                                                {
                                                    double weightHad = simiOn * 1.2;
                                                    _dic2.Add(betw, weightHad);
                                                }
                                                if (edge.Rational.Label == "ANLG")
                                                {
                                                    double weightHad = simiOn * 1.6;
                                                    _dic2.Add(betw, weightHad);
                                                }
                                                
                                            }   
                                            else//如果之间没有直接连接
                                            {
                                                dic1.Add(betw, 2);
                                                double simiTw = Math.Log10(4 / 2);//计算最短路径为2的相似度
                                                _dic2.Add(betw, simiTw);
                                            }
                                       // }
                                    }
                                }
                                
                            }
                        }
                        
                    }
                    
                    
                    
                    
                    //for (int i = 0; i < length / 2; i++)
                    //{
                    //    randIdx = rand.Next(0, length - i);
                    //    string temp = string.Empty;
                    //    temp = _nameString[length - i];
                    //    _nameString[length - i] = _nameString[randIdx];
                    //    _nameString[randIdx] = temp;
                    //}
                }
                return _project;
            }
        } 
        public  List<string> NameString//安全性
        {
            get
            {
                if (this.Project != null)
                {
                     return _nameString;
                }
                return null;
            }
        }
        public List<double> Similarity
        {
            get
            {
                if (this.Project != null)
                {
                    return _similaList;
                }
                return null;
            }
        }
        public Dictionary<int[], double> Dic2
        {
            get
            {
                if (this.Project != null)
                {
                    return _dic2;
                }
                return null;
            }
        }
        //static ConceptKRModule()
        //{
        //    return _nameString = new nameString();
        //}
        //private ConceptKRModule(_course)
        //{

        //}

        public override List<string> KRAttributes
        {
            get { return new List<string>() {"内涵","外延","区别特征" }; }
        }

        public override List<SemanticNet> SNets
        {
            get 
            {
                List<SemanticNet> nets = new List<SemanticNet>();
                List<ConceptKRModuleSNet> conceptNets = ((KRSNetProject<ConceptKRModuleSNet>)Project).NetList;
                foreach (var n in conceptNets)
                    nets.Add(n.Net);//在nets中添加语义网
                return nets;
            }
        }

        public ConceptKRModule(string course):base(course,KCNames.Concept)//
        {

        } 
         

        public override KRModuleSNet GetKRModuleSNet(string netName)
        {
            if(Project== null)
            {
                return null;
            } 
            SemanticNet net = ((KRSNetProject<ConceptKRModuleSNet>)Project).GetSNet(netName);
            if (net == null)
                return null;

            return new ConceptKRModuleSNet(net);            
        }

        public override TopicModule CreateTopicModule(string netName)
        { 
            ConceptTopicModule topicModule = new ConceptTopicModule(this, netName);
            return topicModule;
        }

        public string GetDefinition()
        {
            return string.Empty;
        }
        public string GetExtention()
        {
            return string.Empty;
        }
        public string GetDistinctive()
        {
            return string.Empty;
        }
    }
}
