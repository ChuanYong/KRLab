using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using KRLab.Core.SNet;
using KRLab.Core;

namespace ITS.DomainModule
{
    public class PhenomenaKRModule : KRModule
    {    

        public override object Project
        {
            get
            {
                if (_project == null)
                {
                    string path = FileManager.GetKRSNProjectPath(_course, ProjectType.phensn);

                    if (path == null || !File.Exists(path))
                        return null;
                    KRSNetProject<PhenomenaKRModuleSNet> project = new KRSNetProject<PhenomenaKRModuleSNet>();
                    project.LoadFromFile(path);
                    _project = project;
                }
                return _project;
            }
        }

        public override List<string> KRAttributes => new List<string>();

        public override List<SemanticNet> SNets
        {
            get
            {
                List<SemanticNet> list = new List<SemanticNet>();
                List<PhenomenaKRModuleSNet> nets = ((KRSNetProject<PhenomenaKRModuleSNet>)Project).NetList;
                foreach (var net in nets)
                {
                    list.Add(net.Net);
                }
                return list;
            }
        }

        public PhenomenaKRModule(string course) : base(course,KCNames.Phenomena)
        {
        }         

        public override KRModuleSNet GetKRModuleSNet(string netName)
        {
            if (Project == null)
                return null;
            KRSNetProject<PhenomenaKRModuleSNet> project = (KRSNetProject<PhenomenaKRModuleSNet>)Project;
            SemanticNet net = project.GetSNet(netName);
            if (net == null)
                return null;

            return new PhenomenaKRModuleSNet(net);
        }

        public override TopicModule CreateTopicModule(string netName)
        {
            PhenomenaTopicModule topicModule = new PhenomenaTopicModule(this, netName);
            return topicModule;
        }

    }
}
