using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS.DomainModule;
using KRLab.Core.SNet;

namespace ITS.MaterialModule
{
    public class DomainTopicPQAFactory : PQAFactory
    { 
        public DomainTopicKRModule KRModule
        {
            get { return (DomainTopicKRModule)_krModule; }
        }

        public DomainTopicPQAFactory(string course) :
            base(new DomainTopicKRModule(course))
        { 
        } 

        public override PQA CreateSpecificPQA(string topic)
        {
            DomainTopicModule topicModule = (DomainTopicModule)KRModule.CreateTopicModule(topic);
            PQA spa = new PQA(topic, new Problem());

            return spa;
        }
    }
}
