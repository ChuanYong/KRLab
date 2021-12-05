using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class InstrumentTopicModule:TopicModule
    {
        public InstrumentKRModuleSNet SNet
        {
            get { return (InstrumentKRModuleSNet)_sNet; }
        }

        public string InstrumentName
        {
            get { return SNet.InstrumentNode.Name; }
        }

        public InstrumentTopicModule(string course,KRModuleSNet net):base(course,net)
        {

        }

        public InstrumentTopicModule(InstrumentKRModule krModule,string topic):
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
        public List<string> GetNoticPoints()
        {
            List<string> notices = new List<string>();
            List<SNNode> nodes = SNet.GetNoticNodes();
            foreach(var node in nodes)
            {
                notices.Add(node.Name);
            }
            return notices;
        }

        public List<string> GetImageNames()
        {
            List<string> names = new List<string>();
            List<SNNode> nodes = SNet.GetImageNodes();
            foreach(var node in nodes)
            {
                names.Add(node.Name);
            }
            return names;
        }

        public List<string> GetFunctionalities()
        {
            List<string> funcs = new List<string>();
            List<SNNode> nodes = SNet.GetFuncNodes();
            foreach(var node in nodes)
            {
                funcs.Add(node.Name);
            }
            return funcs;
        }

        public List<string> GetSuitableCases()
        {
            List<string> cases = new List<string>();
            List<SNNode> nodes = SNet.GetSuitableNodes();
            foreach(var node in nodes)
            {
                cases.Add(node.Name);
            }
            return cases;
        }
    }
}
