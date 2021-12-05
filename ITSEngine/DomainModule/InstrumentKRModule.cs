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
    public class InstrumentKRModule:KRModule
    {  

        public override object Project
        {
            get
            {
                if (_project == null)
                {
                    string path = FileManager.GetKRSNProjectPath(_course, ProjectType.untsn);

                    if (path == null || !File.Exists(path))
                        return null;
                    KRSNetProject<InstrumentKRModuleSNet> project = new KRSNetProject<InstrumentKRModuleSNet>();
                    project.LoadFromFile(path);

                    _project = project;
                }
                return _project;
            }
        }

        public override List<string> KRAttributes
        {
            get { return new List<string>() { }; } 
        }

        public override List<SemanticNet> SNets
        {
            get
            {
                List<SemanticNet> nets = new List<SemanticNet>();
                List<InstrumentKRModuleSNet> conceptNets = ((KRSNetProject<InstrumentKRModuleSNet>)Project).NetList;
                foreach (var n in conceptNets)
                    nets.Add(n.Net);
                return nets;
            }
        }

        public InstrumentKRModule(string course):base(course,KCNames.Instrument)
        {

        } 

        public override KRModuleSNet GetKRModuleSNet(string netName)
        {
            if (Project == null)
                return null;

            SemanticNet net = ((KRSNetProject<InstrumentKRModuleSNet>)Project).GetSNet(netName);
            if (net == null)
                return null;

            return new InstrumentKRModuleSNet(net);
        }

        public override TopicModule CreateTopicModule(string netName)
        {
            InstrumentTopicModule topicModule = new InstrumentTopicModule(this, netName);
            return topicModule;
        }
    }
}
