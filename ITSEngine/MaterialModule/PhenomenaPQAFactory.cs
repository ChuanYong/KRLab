using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS.DomainModule;
using ITSText;
using KRLab.Core.SNet;

namespace ITS.MaterialModule
{
    public class PhenomenaPQAFactory : PQAFactory
    {
        public PhenomenaKRModule KRModule
        {
            get { return (PhenomenaKRModule)_krModule; }
        }
        public PhenomenaPQAFactory(string course):
            base(new PhenomenaKRModule(course))
        { 
        }

        public override PQA CreateSpecificPQA(string topic)
        {
            KRModuleSNet net = KRModule.GetKRModuleSNet(topic);
            if (net == null)
                return null;

            PhenomenaTopicModule topicModule = new PhenomenaTopicModule(KRModule.Course, net);
            PQA spa = new PQA(topic, new Problem());

            //（1）
            string content = topicModule.GetContent();
            List<string> assocs = topicModule.GetAssocConcepts();
            AddQAs(ref spa, new[] {0.3,0.3,0.3 }, TextProcessor.ReplaceWithUnderLine(content, assocs), assocs.ToArray());

            return spa;
        }
    }
}
