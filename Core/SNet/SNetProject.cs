using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

using KRLab.Core;
using KRLab.Translations;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 一般语义网
    /// </summary>
    public class SNetProject
    {  
        protected string _projectFileName;

        //当前语义网，相当于semantic network设计软件中的一个diagram
        //在设计语义网时，相当于每章的内容。
        protected string _currentNetName;
        protected SemanticNet _currentSNet;

        //在计算题的语义网中，包含了一个问题子网和一个公式子网
        //故事子网中包含有“故事”结点，公式子网包含有“计算方法”结点
        protected List<SemanticNet> _storySubNets;
        protected SemanticNet _equationSubNet;

        protected SNNode _currentNode;
        protected SNNode _startNode;
        protected SNEdge _startEdge;

        protected List<SemanticNet> _NetList;
 
        public List<SemanticNet> NetList
        {
            get { return _NetList; }
        }

        public SemanticNet CurrentSNet
        {
            get { return _currentSNet; }
        }

        public SNetProject()
        {
            _projectFileName = string.Empty;
            _currentNetName = string.Empty;
            _NetList = new List<SemanticNet>(); 
        }


        public void CheckNet()
        {
            //List<SNNode> knowledgeNodes = Net.GetIncomingSources(_topicNode, SNRational.ISA);
            //if (knowledgeNodes.Count < 1)
            //{
            //    throw new Exception("名为" + Topic + "的语义网错误，没有列出单位！");
            //}

            //_unitNodeDict = new Dictionary<string, SNNode>();
            //foreach (var node in knowledgeNodes)
            //{
            //    _unitNodeDict.Add(node.Name, node);
            //}
            //_unitSymbolDict = new Dictionary<string, string>();
            //foreach (var d in _unitNodeDict)
            //{
            //    SNNode sNode = Net.GetATTNode(d.Value, ITSStrings.Symbol);
            //    if (sNode != null)
            //        _unitSymbolDict.Add(d.Key, sNode.Name);

            //}
        }


        /// <summary>
        /// 先读入项目文件，从项目中多个语义网中查找相关的语义网
        /// </summary>
        /// <param name="topic">语义网名称，比如，运动与速度</param>
        /// <returns></returns>
        public SemanticNet GetSNet(string topic)
        { 
            foreach (SemanticNet net in NetList)
            {
                if (net.Topic.Contains(topic) || topic.Contains(net.Topic))
                {
                    _currentSNet = net;
                    return net;
                }
            }
            return null;
        }

        /// <summary>
        /// //语义项目名称，比如，初中物理
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void LoadFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException(Strings.ErrorBlankFilename, "fileName");

            if (!File.Exists(fileName))
                throw new FileNotFoundException(Strings.ErrorFileNotFound);

            if (_projectFileName == fileName)
                return;

            _projectFileName = fileName;

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(fileName);
            }
            catch (Exception ex)
            {
                throw new IOException(Strings.ErrorCouldNotLoadFile, ex);
            }

            XmlElement root = document["Project"];

            try
            {
                Deserialize(root);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(Strings.ErrorCorruptSaveFile, ex);
            }

        }

        public SemanticNet FindSNetWithNodeName(string nodeName)
        {
            foreach (SemanticNet net in NetList)
            {
                if (net.GetNode(nodeName) != null)
                {
                    return net;
                }
            }

            return null;
        }

        protected virtual void Deserialize(XmlElement projectNode)
        {
            //KRLab中项目文件的名称
            XmlElement projectNameElement = projectNode["Name"];
            if (projectNameElement == null)
            {
                throw new InvalidDataException("Project's name cannot be empty");
            }
            string projectName = projectNameElement.InnerText;

            foreach (XmlElement item in projectNode.GetElementsByTagName("ProjectItem"))
            {
                SemanticNet net;
                DeserializeProjectItem(item, out net);
                NetList.Add(net);
            }

            _currentSNet = NetList[0];
        }

        protected void DeserializeProjectItem(XmlElement projectItemNode,out SemanticNet net)
        {
            XmlElement diagramNameItem = projectItemNode["Name"];
            string diagramName = diagramNameItem.InnerText;
            net = new SemanticNet(diagramName);

            XmlNodeList elmList = projectItemNode.SelectNodes("Entities/Entity");
            List<SNNode> nodeList = new List<SNNode>();

            foreach (XmlNode elm in elmList)
            {
                try
                {
                    string nodeName = elm["Name"].InnerText;
                    SNNode snNode = new SNNode(nodeName);

                    net.AddNode(snNode);
                    nodeList.Add(snNode);

                }
                catch (BadSyntaxException ex)
                {
                    throw new InvalidDataException("Invalid entity", ex);
                }
            }

            XmlNodeList relationList = projectItemNode.SelectNodes("Relationships/Relationship");
            foreach (XmlElement elm in relationList)
            {
                string rational = elm["SNRelationshipType"].InnerText;
                string first = elm.GetAttribute("first");
                string second = elm.GetAttribute("second");
                string label = elm["Label"].InnerText;

                string startMulti = string.Empty;
                XmlElement smNode = elm["StartMultiplicity"];
                if (smNode != null)
                    startMulti = smNode.InnerText;
                string endMulti = string.Empty;
                XmlElement emNode = elm["EndMultiplicity"];
                if (emNode != null)
                    endMulti = emNode.InnerText;
                string startRole = string.Empty;
                XmlElement srNode = elm["StartRole"];
                if (srNode != null)
                    startRole = srNode.InnerText;
                string endRole = string.Empty;
                XmlElement erNode = elm["EndRole"];
                if (erNode != null)
                    endRole = erNode.InnerText;

                int firstIndex, secondIndex;
                if (!int.TryParse(first, out firstIndex) ||
                    !int.TryParse(second, out secondIndex))
                {
                    throw new InvalidDataException("not correct SemanticNet's format");
                }

                if (firstIndex < 0 || firstIndex >= nodeList.Count ||
                    secondIndex < 0 || secondIndex >= nodeList.Count)
                {
                    throw new InvalidDataException("not correct SemanticNet's format");
                }                 

                net.AddEdge(nodeList[firstIndex], nodeList[secondIndex],
                    new SNRational(rational, label,startMulti,endMulti,startRole,endRole));
            }
        }

        public List<string> FindDiagramFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException(Strings.ErrorBlankFilename, "fileName");

            if (!File.Exists(fileName))
                throw new FileNotFoundException(Strings.ErrorFileNotFound);

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(fileName);
            }
            catch (Exception ex)
            {
                throw new IOException(Strings.ErrorCouldNotLoadFile, ex);
            }

            XmlElement root = document["Project"];

            XmlElement nameElement = root["Name"];
            if (nameElement == null || nameElement.InnerText == "")
                throw new InvalidDataException("Project's name cannot be empty.");
            string projectName = nameElement.InnerText;

            List<string> names = new List<string>();
            foreach (XmlElement item in root.GetElementsByTagName("ProjectItem"))
            {
                names.Add(item["Name"].InnerText);
            }

            return names;
        }
    }
}
