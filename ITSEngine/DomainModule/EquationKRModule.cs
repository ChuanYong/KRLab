using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using KRLab.Core;
using KRLab.Core.SNet;
using Utilities;
using System.Reflection;

namespace ITS.DomainModule
{
    /// <summary>
    /// 
    /// </summary>
    public class EquationKRModule:KRModule
    { 

        public override object Project
        {
            get
            {
                if(_project==null)
                { 
                    string path = FileManager.GetKRSNProjectPath(_course, ProjectType.equsn);
                    if (path == null || !File.Exists(path))
                        return null;
                    KRSNetProject<EquationKRModuleSNet> project = new KRSNetProject<EquationKRModuleSNet>();
                    project.LoadFromFile(path);

                    _project = project;
                    
                }
                return _project;
            }
        }

        public override List<string> KRAttributes
        {
            get { throw new NotImplementedException(); }
        }


        public override List<SemanticNet> SNets
        {
            get
            {
                List<SemanticNet> nets = new List<SemanticNet>();
                List<EquationKRModuleSNet> conceptNets = ((KRSNetProject<EquationKRModuleSNet>)Project).NetList;
                foreach (var n in conceptNets)
                    nets.Add(n.Net);
                return nets;
            }
        }

        public EquationKRModule(string course):base(course,KCNames.Equation)
        {
        } 

        public override KRModuleSNet GetKRModuleSNet(string netName)
        {
            if (Project == null)
                return null;
            SemanticNet net = ((KRSNetProject<EquationKRModuleSNet>) Project).GetSNet(netName);
            if (net == null)
                return null;
            return new EquationKRModuleSNet(net);
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="netName"></param>
        /// <param name="topic">方程的中文名</param>
        /// <returns></returns>
        public override TopicModule CreateTopicModule(string netName)
        { 
            EquationTopicModule topicModule = new EquationTopicModule(this, netName);
            return topicModule;
        } 
    }
}
