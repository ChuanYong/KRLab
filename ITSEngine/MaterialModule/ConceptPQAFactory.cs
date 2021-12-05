using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITS.DomainModule;
using KRLab.Core.SNet;
using ITSText;

using Utilities;

namespace ITS.MaterialModule
{
    public class ConceptPQAFactory:PQAFactory
    {
        public ConceptKRModule KRModule
        {
            get { return (ConceptKRModule)_krModule; }
        }

        public ConceptPQAFactory(string course):
            base(new ConceptKRModule(course))//把课程的概念模块代入，在基类中调用，返回_KrModule
        { 
        }

        public override PQA CreateSpecificPQA(string topic)
        {
            ///在对应的课程course中获取topic的语义网
            KRModuleSNet net = KRModule.GetKRModuleSNet(topic);
            if (net == null)
                return null;

            ConceptTopicModule topicModule = new ConceptTopicModule(KRModule.Course,net);
            PQA pqa = new PQA(topic, new Problem());

            //（1）概念内涵的填空题 
            string content = topicModule.Definition;
            List<string> ans = topicModule.GetDefAssocedInfo();//获取定义相关信息
            content = TextProcessor.ReplaceWithUnderLine(content, ans);//将内容和答案用下划线替换
            AddQAs(ref pqa, new[] { 0.3, 0.2, 0.3 }, $"有关<{topic}>的概念，请填空，\n {content}", ans.ToArray());
            //利用模糊算法添加问题到pqa中


            //(2)外延
            //List<string> exts = topicModule.Extensions;
            //if(exts.Count>0)
            //{
            //    string q = $"{topic}包括哪些？";
            //    AddQAs(ref pqa, new[] {0.2,0.5,0.2 }, q, exts.ToArray());
            //}

            //(3)区别概念
            List<string> disConcepts = topicModule.GetDistintiveConcepts();
            if (disConcepts.Count > 0)
            {
                string q = $"你能否列出与{topic}完全不同的、但易于混淆的概念，";
                AddQAs(ref pqa, new[] { 0.3, 0.5, 0.5 }, q, disConcepts.ToArray());
            }

            //（4）近似概念
            List<string> similarCons = topicModule.GetSimilarConcepts();
            if(similarCons.Count>0)
            {
                string q = $"请列出你所指知道的与<{topic}>类似的概念，";
                AddQAs(ref pqa, new [] { 0.2, 0.3, 0.2 }, q, similarCons.ToArray());
            }

            return pqa; 
        }
    }
}
