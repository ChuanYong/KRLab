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
    /// <summary>
    /// 单位的知识表达
    /// </summary>
    public class UnitKRModule:KRModule
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
                    KRSNetProject<UnitKRModuleSNet> project = new KRSNetProject<UnitKRModuleSNet>();
                    project.LoadFromFile(path);

                    _project = project;
                }
                return _project;
            }
        }

        public override List<string> KRAttributes
        {
            get { return new List<string>() {"符号表示" }; }
        }


        public override List<SemanticNet> SNets
        {
            get
            {
                List<SemanticNet> nets = new List<SemanticNet>();
                List<UnitKRModuleSNet> conceptNets = ((KRSNetProject<UnitKRModuleSNet>)Project).NetList;
                foreach (var n in conceptNets)
                    nets.Add(n.Net);
                return nets;
            }
        }

        public UnitKRModule(string course):base(course,KCNames.Unit)
        { 
        } 

        public override KRModuleSNet GetKRModuleSNet(string netName)
        {
            if (Project == null)
                return null;

            SemanticNet net = ((KRSNetProject<UnitKRModuleSNet>)Project).GetSNet(netName);
            if (net == null)
                return null;

            return new UnitKRModuleSNet(net);
        }

        public override TopicModule CreateTopicModule(string netName)
        {
            UnitTopicModule topicModule = new UnitTopicModule(this, netName);
            return topicModule;
        }

    }
     
}
