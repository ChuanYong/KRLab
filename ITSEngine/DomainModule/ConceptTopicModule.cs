using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class ConceptTopicModule:TopicModule
    {
        public ConceptKRModuleSNet SNet
        {
            get { return (ConceptKRModuleSNet)_sNet; }
        }

        public ConceptTopicModule(string course,KRModuleSNet net):base(course,net)
        {

        }

        public ConceptTopicModule(ConceptKRModule krModule,string topic):
            base(krModule, topic)
        { 
        }

        public override string Parse()
        {
            base.Parse();

            string info = string.Empty;
            info += Definition+","+ SNet.SuperConceptNode.Item1+ SNet.SuperConceptNode.Item2+"。";
            if(Extensions.Count!=0)
            {
                info += Topic+ExtensionLabel+"：\n";
                foreach (var str in Extensions)
                    info += str + " "; 
            }

            return info;
        }

        public override string Parse(string name)
        {
            return base.Parse(name);
        }

        public string Definition
        {
            get { return SNet.DefinitionNode.Name; }
        }

        /// <summary>
        /// 获取概念内涵的特征
        /// </summary>
        /// <returns></returns>
        public List<string> Characteristics
        {
            get
            {
                List<string> strs = new List<string>();
                List<SNNode> nodes = SNet.CharacteristicsNodes;
                foreach (var nd in nodes)
                {
                    strs.Add(nd.Name);
                }
                return strs;
            }

        }

        public List<string> Extensions
        {
            get
            {
                List<string> strs = new List<string>();
                List<System.Tuple<string,SNNode>> nodes = SNet.ExtensionNodes;
                foreach (var node in nodes)
                {
                    strs.Add(node.Item2.Name);
                }
                return strs;
            }
        }

        public string ExtensionLabel
        {
            get
            {
                if (SNet.ExtensionNodes.Count > 0)
                    return SNet.ExtensionNodes[0].Item1;
                else
                    return "";
            }
        }

        public List<string> GetDefAssocedInfo()
        {
            List<string> info = new List<string>();
            List<SNNode> nodes = SNet.DefAssocedNodes;
            foreach(var node in nodes)
            {
                info.Add(node.Name);
            }
            return info;
        }

        public string GetSuperConcept()
        {
            if (SNet.SuperConceptNode != null)
            {
                return SNet.SuperConceptNode.Item2.Name;
            }
            else
                return null;
        }

        public List<string> GetDistintiveConcepts()
        {
            List<string> concepts = new List<string>();
            List<SNNode> nodes = SNet.DistinctiveNodes;
            foreach(var node in nodes)
            {
                concepts.Add(node.Name);
            }
            return concepts;
        }

        public List<string> GetSimilarConcepts()
        {
            List<string> concepts = new List<string>();
            List<SNNode> nodes = SNet.GetSimilarNodes();
            foreach(var node in nodes)
            {
                concepts.Add(node.Name);
            }

            return concepts;
        }
    }
}
