using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

namespace ITS.DomainModule
{
    public class DomainTopicModule:TopicModule
    {
        public DomainTopicKRModuleSNet SNet
        {
            get { return (DomainTopicKRModuleSNet)_sNet; }
        } 

        public DomainTopicModule(DomainTopicKRModule krModule,string topic):
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
    }
}
