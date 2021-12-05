using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using ITS.DomainModule;
using KRLab.Core.SNet;

using Utilities;
using ITS.MathSolvers;

using Symbolism;

namespace ITS.MaterialModule
{
    public class EquationPQAFactory:PQAFactory
    {
        protected AssemblyInstance _assembly;

        public EquationKRModule KRModule
        {
            get { return (EquationKRModule)_krModule; }
        }

        public EquationPQAFactory(string course) :
            base(new EquationKRModule(course))
        { 
        }

        public override PQA CreateSpecificPQA(string topic)
        {
            KRModuleSNet net = KRModule.GetKRModuleSNet(topic);
            if (net == null)
                return null;

            EquationTopicModule topicModule = new EquationTopicModule(KRModule.Course, net);
            PQA spa = new PQA(topic,new Problem());

            //(1)方程的算法，需要查询相关的算法语义知识模块
            foreach(var equ in topicModule.EquNames)
            {
                List<string> algorithms = topicModule.GetAlgorithms(equ);
                if(algorithms.Count!=0)
                { 
                    AddQAs(ref spa, new[] { 0.1, 0.4, 0.3 }, equ + "相关算法有哪些？", algorithms.ToArray());

                    foreach(var al in algorithms)
                    {
                        ProceduralKRModule pkr = new ProceduralKRModule(KRModule.Course);
                        if(pkr.SNetNames.Contains(al))
                        {
                            ProceduralTopicModule ptm = (ProceduralTopicModule)pkr.CreateTopicModule(al);
                            string[] steps = ptm.GetStepNames().ToArray();
                            AddQAs(ref spa, new double[] { 0.5, 0.3, 0.4 }, $"如何用{al}求解{equ}？",steps);
                            Dictionary<string, List<string>> condDict = ptm.GetStepConds();
                            if(condDict.Count!=0)
                            {
                                foreach(var dict in condDict)
                                {
                                    AddQAs(ref spa, new[] { 0.5, 0.3, 0.6 }, $"执行<{dict.Key}>步骤有什么条件？", dict.Value.ToArray());
                                }
                            }
                        }
                    }
                }
            }

            //（2）方程的求解过程 
            List<EquElem> equs = topicModule.GetEquElems();
            foreach(var equ in equs)
            {
                string equTypeName = topicModule.GetEquClassName(equ);
                ISolving solving = CreateEquInstance(equTypeName);
                Debug.Assert(solving != null);
                if(solving!=null)
                {
                    string[] ans = solving.SolvingSteps().ToArray();
                    List<MathObject> mObs = topicModule.GetEquExprs(equ);
                    string equExprs = string.Empty;
                    foreach (var ob in mObs)
                        equExprs += ob.StandardForm() + "\n";
                    AddQAs(ref spa, new[] { 0.6, 0.7, 0.8 }, $"请求解{equExprs}", ans);
                }
            }

            //(3)处理语义网中的提问


            return spa;
        }

        protected ISolving CreateEquInstance(string equName)
        {
            if(_assembly==null)
            {
                AssemblyName name = new AssemblyName("MathSolvers");
                _assembly = new AssemblyInstance(name);
            }

            object ob=_assembly.CreateInstance(equName);
            if (ob != null)
            {
                return (ISolving)ob;
            }
            else
                return null;
        }


    }
}
