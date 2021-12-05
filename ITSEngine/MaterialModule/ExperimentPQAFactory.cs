using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITS.DomainModule;
using KRLab.Core.SNet;

namespace ITS.MaterialModule
{
    public class ExperimentPQAFactory:PQAFactory
    {
        public ExperimentKRModule KRModule
        {
            get { return (ExperimentKRModule)_krModule; }
        }

        public ExperimentPQAFactory(string course) 
            :base(new ExperimentKRModule(course))
        { 

        }
        public override PQA CreateSpecificPQA(string topic)
        {
            KRModuleSNet net = KRModule.GetKRModuleSNet(topic);
            if (net == null)
                return null;

            ExperimentTopicModule topicModule = new ExperimentTopicModule(KRModule.Course, net);

            PQA spa = new PQA(topic, new Problem());

            //(1)
            AddQAs(ref spa, new[] {0.3,0.1,0.2 }, $"{topicModule.Topic}的实验原理是什么？", topicModule.GetPrinciple());

            //(2)
            AddQAs(ref spa, new[] {0.3,0.3,0.2  }, $"请阐述{topicModule.Topic}的实验方法或步骤。",topicModule.GetMethods().ToArray());

            //(3)
            AddQAs(ref spa, new[] {0.3,0.3,0.4 }, $"该实验所需的器材主要有哪些？",topicModule.GetInstruments().ToArray());

            //(4)
            AddQAs(ref spa, new[] { 0.2, 0.1, 0.2 }, $"该实验的目的是什么？", topicModule.GetPurposes().ToArray());

            return spa;
        }
    }
}
