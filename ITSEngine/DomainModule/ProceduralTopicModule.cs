using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class ProceduralTopicModule:TopicModule
    {
        public ProceduralKRModuleSNet SNet
        {
            get { return (ProceduralKRModuleSNet)_sNet; }
        } 

        public List<SNNode> StepNodes
        {
            get { return SNet.StepNodes; }
        }

        public ProceduralTopicModule(string course,KRModuleSNet net):base(course,net)
        {

        }

        public ProceduralTopicModule(ProceduralKRModule krModule,string topic):
            base(krModule,topic)
        {

        }

        /// <summary>
        /// 查询算法的所有步骤
        /// </summary>
        /// <returns></returns>
        public List<string> GetStepNames()
        {
            List<string> names = new List<string>();
            foreach (var node in SNet.StepNodes)
            {
                names.Add(node.Name);
            }
            return names;
        }

        /// <summary>
        /// 获取需要条件的执行步骤
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,List<string>> GetStepConds()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            foreach(var node in SNet.StepNodes)
            {
                List<SNNode> condNodes = SNet.Net.GetOutgoingDestinations(node, SNRational.COND);
                if (condNodes.Count>0)
                {
                    List<string> conds = new List<string>();
                    foreach(var nd in condNodes)
                    {
                        conds.Add(nd.Name);
                    }
                    dict[node.Name] = conds;
                }
            }
            return dict;
        }

        public override string Parse()
        {
            return base.Parse();
        }

        public override string Parse(string name)
        {
            return base.Parse(name);
        }
    }
}
