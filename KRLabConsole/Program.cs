using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks; 

 
using WpfMath; 
using KRLab.Core.FuzzyEngine;
using KRLab.Core.SNet;
using KRLab.Core;

using ITS.DomainModule; 

using Utilities;
using ITSText;
 
using System.Reflection; 

namespace KRLabConsole
{ 
    class Program
    {
        public static void callback(string name)
        {
            Console.WriteLine(name);
        }
        static void Main(string[] args)
        {
            string str = "X\xB2";
            Console.WriteLine(str);
             
             
            //Console.WriteLine(equ.ToString());

            //Expr c = Expr.Parse("2");
            //c = c.Sqrt();
            //Expr b = Expr.Parse("2^(1/2)");
            //if(b.ToInternalString()==c.ToInternalString())
            //    Console.WriteLine(b.ToInternalString());

            //YYECEqu equ = new YYECEqu();
            //equ.CreateEquModelOne(new Tuple<int, int, int>(2,3,0),new Tuple<int, int, int>(0,2,3));
            //Console.WriteLine(equ.ToString());
        }
        //static void Main(string[] args)
        //{
        //    Symbol x = new Symbol("x");
        //    MathObject y = new DoubleFloat(0);

        //    Equation obj = new Equation(x, y, Equation.Operators.NotEqual);
        //    obj.StandardForm();
        //    Console.WriteLine(obj.StandardForm());


        //    //string path = FileManager.KnowledgePath + @"\物理\结论.consn";
        //    //SemanticNet net = KRModuleSNet.CreateASNet("运动和静止的相对性", path);
        //    //SNNode node = net.FuzzyGetNode("运动和静止的相对性");

        //    ////ACTParseInfo actInfo = new ACTParseInfo(node,net);
        //    ////List<SNNode> actors = actInfo.GetActorNodes();
        //    ////List<SNNode> results = actInfo.GetAssocResultNodes(actors[0]);

        //    //ATTParseInfo info = new ATTParseInfo(node, net);
        //    //string str=info.ParseAttValue("内容");

        //    //SNNode node1 = net.FastGetNode("内容");
        //    //ATTParseInfo info1 = new ATTParseInfo(node1, net);
        //    //string str1 = info1.ParseAttValue("特征");

        //}
        class Fuzzy
        {
            bool selected = false;
            IFuzzyEngine engine;
            public float Speedmult = 0.3f;
            LinguisticVariable distance, direction;

            public void Init()
            {
                // Here we need to setup the Fuzzy Inference System
                distance = new LinguisticVariable("distance");
                var farRight = distance.MembershipFunctions.AddTrapezoid("farRight", -100, -100, -60, -45);
                var right = distance.MembershipFunctions.AddTrapezoid("right", -50, -50, -7, -0.05f);
                var none = distance.MembershipFunctions.AddTrapezoid("none", -7, -0.5, 0.5, 7);
                var left = distance.MembershipFunctions.AddTrapezoid("left", 0.05f, 7, 50, 50);
                var farLeft = distance.MembershipFunctions.AddTrapezoid("farLeft", 45, 60, 100, 100);

                direction = new LinguisticVariable("direction");
                var farRightD = direction.MembershipFunctions.AddTrapezoid("farRight", -100, -100, -60, -45);
                var rightD = direction.MembershipFunctions.AddTrapezoid("right", -50, -50, -7, -0.05f);
                var noneD = direction.MembershipFunctions.AddTrapezoid("none", -7, -0.5, 0.5, 7);
                var leftD = direction.MembershipFunctions.AddTrapezoid("left", 0.05f, 7, 50, 50);
                var farLeftD = direction.MembershipFunctions.AddTrapezoid("farLeft", 45, 60, 100, 100);

                engine = new FuzzyEngineFactory().Default();
                var rule0 = Rule.If(distance.Is(farRight)).Then(direction.Is(farLeftD));
                var rule1 = Rule.If(distance.Is(right)).Then(direction.Is(leftD));
                var rule2 = Rule.If(distance.Is(left)).Then(direction.Is(rightD));
                var rule3 = Rule.If(distance.Is(none)).Then(distance.Is(noneD));
                var rule4 = Rule.If(distance.Is(farLeft)).Then(direction.Is(farRightD));

                engine.Rules.Add(rule0, rule1, rule2, rule3, rule4);
                Console.WriteLine(engine.Defuzzify(new { distance = 10 }));
            }

            private LinguisticVariable _performance;
            private LinguisticVariable _difficulty;
            private IFuzzyEngine _engine;

            public void Init1()
            {
                _performance = new LinguisticVariable("Performance");
                IMembershipFunction excellent = _performance.MembershipFunctions.AddRectangle("Excellent", 9, 10);
                IMembershipFunction poor = _performance.MembershipFunctions.AddRectangle("Poor", 0, 9);

                _difficulty = new LinguisticVariable("Difficulty");
                IMembershipFunction difficulty = _difficulty.MembershipFunctions.AddRectangle("Difficulty", 5, 10);
                IMembershipFunction easy = _difficulty.MembershipFunctions.AddRectangle("Easy", 0, 5);

                FuzzyRule rule0 = Rule.If(_performance.Is(excellent)).Then(_difficulty.Is(difficulty));
                FuzzyRule rule1 = Rule.If(_performance.Is(poor)).Then(_difficulty.Is(easy));

                _engine = new FuzzyEngineFactory().Default();
                _engine.Rules.Add(rule0, rule1);

                Console.WriteLine(_engine.Defuzzify(new { Performance = 9.5 }));

            }
        }
        //static void Main(string[] args)
        //{

        //    //Fuzzy f = new Fuzzy();
        //    //f.Init1();
        //    //BaiChenEvaluationMethod method = new BaiChenEvaluationMethod();
        //    //method.Run();

        //    //Paper20200322 paper = new Paper20200322();
        //    //paper.Run();

        //    //double x = Symbolics.Calculate("2*4/x", "x", 5.0);

        //}

        //static void Main(string[] args)
        //{

        //    BDIExample bdi0 = new BDIExample("first_bdi");
        //    bdi0.Config();
        //    Agent agent0 = new Agent("first", 0, bdi0);
        //    agent0.Go();

        //}
        //static void Main(string[] args)
        //{
        //    try
        //    {
        //        QAEngine qa = new QAEngine();
        //        qa.Config("初中物理计算.fvc", "机械能", "机械能");
        //        //Console.WriteLine(qa.EquationMaker.Equations[0].RightFormula.ExprString);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //}
    }
}
