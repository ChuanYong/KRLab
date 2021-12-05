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
    public class ProceduralKRModule : KRModule
    {
        KRSNetProject<ProceduralKRModuleSNet> _project;

        public override object Project
        {
            get
            {
                if (_project == null)
                {
                    string path = FileManager.GetKRSNProjectPath(_course, ProjectType.procesn);

                    if (path == null || !File.Exists(path))
                        return null;
                    _project = new KRSNetProject<ProceduralKRModuleSNet>();
                    _project.LoadFromFile(path);
                }
                return _project;
            }
        } 

        public override List<string> KRAttributes
        {
            get 
            {
                return new List<string>()
                {
                    "步骤",
                };
            }
        }

        public override List<SemanticNet> SNets
        {
            get
            {
                List<SemanticNet> list = new List<SemanticNet>();
                List<ProceduralKRModuleSNet> nets= ((KRSNetProject<ProceduralKRModuleSNet>)Project).NetList;
                foreach(var net in nets)
                {
                    list.Add(net.Net);
                }
                return list;
            }
        }

        public ProceduralKRModule(string course) : base(course,KCNames.Procedural)
        {
        } 

        public override KRModuleSNet GetKRModuleSNet(string netName)
        {
            if (Project == null)
                return null;

            SemanticNet net =((KRSNetProject<ProceduralKRModuleSNet>)Project).GetSNet(netName);

            if (net == null)
                return null;

            return new ProceduralKRModuleSNet(net);
        }

        public override TopicModule CreateTopicModule(string netName)
        {
            ProceduralTopicModule topicModule = new ProceduralTopicModule(this, netName);
            return topicModule;
        }

    }
}
