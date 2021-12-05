using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class PhenomenaTopicModule:TopicModule
    {
        public PhenomenaKRModuleSNet SNet
        {
            get { return (PhenomenaKRModuleSNet)_sNet; }
        } 

        public PhenomenaTopicModule(string course,KRModuleSNet net):base(course,net)
        {

        }

        public PhenomenaTopicModule(PhenomenaKRModule krModule,string topic):
            base(krModule,topic)
        {


        }

        public override string Parse()
        {
            return base.Parse();
        }

        public override string Parse(string name)
        {
            return base.Parse(name);
        }

        public string GetContent()
        {
            return SNet.ContentNode.Name;
        }

        public List<string> GetAssocConcepts()
        {
            List<SNNode> nodes = SNet.GetAssocNodesOfContent();
            List<string> concepts = new List<string>();
            foreach(var node in nodes)
            {
                concepts.Add(node.Name);
            }
            return concepts;
        }
    }
}
