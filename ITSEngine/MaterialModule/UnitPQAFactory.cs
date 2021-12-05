using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;
using Utilities;

using ITS.DomainModule;
using MathNet.Numerics;

namespace ITS.MaterialModule
{
    public class UnitPQAFactory:PQAFactory
    {
        public UnitKRModule KRModule
        {
            get { return (UnitKRModule)_krModule; }
        }

        public UnitPQAFactory(string course):
            base(new UnitKRModule(course))
        { 
        } 

        public override PQA CreateSpecificPQA(string topic)
        {
            KRModuleSNet net = KRModule.GetKRModuleSNet(topic);
            if (net == null)
                return null;

            UnitTopicModule topicModule = new UnitTopicModule(KRModule.Course, net);
            PQA spa = new PQA(topic, new Problem());

            //（1）
            AddQAs(ref spa, new[] { 0.2, 0.2, 0.1 }, $"请列出{topic}有哪些单位？", topicModule.Units.ToArray());

            //(2)
            List<string> ans = new List<string>();
            foreach (var u in topicModule.Units)
            {
                string str = u + "：" + topicModule.GetUnitSymbol(u);
                ans.Add(str);
            }
            AddQAs(ref spa, new[] { 0.3, 0.1, 0.1 }, $"请分别列出{topic}中每个单位的符号表示。", ans.ToArray());

            //(3)
            AddQAs(ref spa, new[] { 0.2, 0.1, 0.1 }, $"在{ topic}中，国际单位是哪个？", topicModule.GetISUnit());

            //(4)
            int nb = topicModule.Units.Count;
            int i = new System.Random().Next(0, nb);
            string qStr = topicModule.GetDefinition(topicModule.Units[i]);
            if (qStr != null)
            {
                int k = Rand.Random(0, nb, i);
                qStr += "。请问是如何定义" + topicModule.Units[k] + "的？"; 
                string aStr = topicModule.GetDefinition(topicModule.Units[k]);
                if (aStr != null)
                {
                    AddQAs(ref spa, new[] {0.2, 0.4, 0.2}, qStr, aStr); 
                }
            }

            ///(5)
            List<int> arr = Enumerable.Range(0, topicModule.Units.Count).ToList();
            List<Tuple<int, int>> indexPair = Rand.RandomPairs(arr, 3);
            for (i = 0; i < indexPair.Count; i++)
            {
                qStr = "请计算下面的单位转换：" + System.Environment.NewLine;
                int m = indexPair[i].Item1;
                int n = indexPair[i].Item2;
                double x = Rand.Random(2, 10);
                x = Math.Round(x, 1);
                qStr += x.ToString() + topicModule.Units[m] + "等于多少" + topicModule.Units[n] + "？";
                string equ = topicModule.GetResult("x", topicModule.Units[m], topicModule.Units[n]);
                double v = Symbolics.Calculate(equ, "x", x);
                string aStr = v.ToString() + topicModule.Units[n];

                AddQAs(ref spa, new[] { 0.6, 0.3, 0.4 }, qStr, x.ToString() + topicModule.Units[m] + "=", aStr);
            }

            return spa;  
        }
    }
}
