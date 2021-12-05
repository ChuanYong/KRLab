using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using KRLab.Core;
using KRLab.Core.SNet;
using Utilities;

namespace ITS.DomainModule
{
    /// <summary>
    /// 实验的知识表达
    /// </summary>
    public class ExperimentKRModule:KRModule
    {  

        public override object Project
        {
            get
            {
                if(_project==null)
                {
                    string path = FileManager.GetKRSNProjectPath(_course, ProjectType.expsn);
                    if (path == null || !File.Exists(path))
                        return null; 
                    KRSNetProject<ExperimentKRModuleSNet> project = new KRSNetProject<ExperimentKRModuleSNet>();
                    project.LoadFromFile(path);

                    _project = project;
                
                }
                return _project;
            }
        }

        public override List<string> KRAttributes => throw new NotImplementedException();

        public override List<SemanticNet> SNets
        {
            get
            {
                List<SemanticNet> nets = new List<SemanticNet>();
                List<ExperimentKRModuleSNet> conceptNets = ((KRSNetProject<ExperimentKRModuleSNet>)Project).NetList;
                foreach (var n in conceptNets)
                    nets.Add(n.Net);
                return nets;
            }
        }

        public ExperimentKRModule(string course):base(course,KCNames.Experiment)
        {

        } 

        public override KRModuleSNet GetKRModuleSNet(string netName)
        {
            if (Project == null)
                return null;
            SemanticNet net = ((KRSNetProject<ExperimentKRModuleSNet>) Project).GetSNet(netName);
            if (net == null)
                return null;
            return new ExperimentKRModuleSNet(net);
        }

        public override TopicModule CreateTopicModule(string netName)
        {
            ExperimentTopicModule topicModule = new ExperimentTopicModule(this, netName);
            return topicModule;
        }

    }
}
