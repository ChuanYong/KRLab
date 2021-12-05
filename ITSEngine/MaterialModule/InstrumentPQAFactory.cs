using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;
using Utilities;
using ITS.DomainModule;

namespace ITS.MaterialModule
{
    public class InstrumentPQAFactory:PQAFactory
    {
        public InstrumentKRModule KRModule
        {
            get { return (InstrumentKRModule)_krModule; }
        }

        public InstrumentPQAFactory(string course):
            base(new InstrumentKRModule(course))
        {

        }

        public override PQA CreateSpecificPQA(string topic)
        {
            KRModuleSNet net = KRModule.GetKRModuleSNet(topic);
            if (net == null)
                return null;

            InstrumentTopicModule topicModule = new InstrumentTopicModule(KRModule.Course,net);
            PQA pqa = new PQA(topic, new Problem());

            //(1)
            List<string> images = topicModule.GetImageNames();
            if(images.Count==1)
            {
                AddQAs(ref pqa, new[] { 0.3, 0.2, 0.1 }, $"图中显示的是什么仪器？", topicModule.InstrumentName);
            }
            else if(images.Count>1)
            {
                AddQAs(ref pqa, new[] { 0.3, 0.2, 0.3 }, $"图中显示的都是什么仪器", topicModule.InstrumentName);
            }

            //(2)
            List<string> funcs = topicModule.GetFunctionalities();
            AddQAs(ref pqa, new [] {0.2,0.3,0.3 }, $"{topicModule.InstrumentName}是用来做什么的？", funcs.ToArray());

            //(3)
            List<string> notices = topicModule.GetNoticPoints();
            if(notices.Count!=0)
            {
                AddQAs(ref pqa, new[] { 0.2, 0.2, 0.2 }, $"{topicModule.InstrumentName}有哪些使用注意事项？", notices.ToArray());
            }

            return pqa;
        }
    }
}
