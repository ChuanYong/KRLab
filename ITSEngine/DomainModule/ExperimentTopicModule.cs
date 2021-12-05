using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class ExperimentTopicModule:TopicModule
    {
        public ExperimentKRModuleSNet SNet
        {
            get { return (ExperimentKRModuleSNet)_sNet; }
        }
        
        public ExperimentTopicModule(string course,KRModuleSNet net):base(course,net)
        {

        }

        public ExperimentTopicModule(ExperimentKRModule krModule,string topic):
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

        public string GetPrinciple()
        {
            return SNet.GetPrincipleNode().Name;
        }

        public List<string> GetMethods()
        {
            List<string> methods = new List<string>();
            List<SNNode> nodes = SNet.GetMethodNodes();
            foreach(var node in nodes)
            {
                methods.Add(node.Name);
            }
            return methods;
        }

        public List<string> GetInstruments()
        {
            List<string> names = new List<string>();
            List<SNNode> nodes = SNet.GetInstrumentNodes();
            foreach(var node in nodes)
            {
                names.Add(node.Name);
            }

            return names;
        }

        public List<string> GetPurposes()
        {
            List<string> purposes = new List<string>();
            List<SNNode> nodes = SNet.GetPurposeNodes();
            foreach(var node in nodes)
            {
                purposes.Add(node.Name);
            }
            return purposes;
        }
    }
}
