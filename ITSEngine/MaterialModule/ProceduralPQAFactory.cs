using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;

using ITS.DomainModule;

namespace ITS.MaterialModule
{
    public class ProceduralPQAFactory : PQAFactory
    {
        public ProceduralKRModule KRModule
        {
            get { return (ProceduralKRModule)_krModule; }
        }
        public ProceduralPQAFactory(string course):
            base(new ProceduralKRModule(course))
        { 
        }

        public override PQA CreateSpecificPQA(string topic)
        {
            KRModuleSNet net = KRModule.GetKRModuleSNet(topic);
            if (net == null)
                return null;

            ProceduralTopicModule topicModule = new ProceduralTopicModule(KRModule.Course,net);
            PQA pqa = new PQA(topic,new Problem());

            //（1）考察算法中有哪些步骤 
            AddQAs(ref pqa, new[] { 0.3, 0.2, 0.3 }, "请列出" + topic + "包括的主要步骤，",
                topicModule.GetStepNames().ToArray());

            //(2)考察算法中需要条件的执行步骤
            Dictionary<string, List<string>> condDicts = topicModule.GetStepConds();
            foreach(var d in condDicts.Keys)
            { 
                AddQAs(ref pqa, new[] { 0.5, 0.5, 0.5 }, "执行<" + topic + ">中的<" + d + ">步骤需要什么条件？",
                    condDicts[d].ToArray());
            }
            return pqa;
        }
    }
}
