using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;



using ITS.DomainModule;
using KRLab.Core.SNet;
using MathNet.Numerics;
using Utilities;

using Analytics;
using Exversion;
//using Physics.Units; 

namespace ITS.MaterialModule
{
    public abstract class PQAFactory
    {
        protected KRModule _krModule;

        public PQAFactory(KRModule krModule)
        {
            _krModule =krModule; 
        } 

        public List<KnowledgeTopic> GetDeptTopics(string topic)
        {
            TopicModule tm=new TopicModule(_krModule, topic);
            return tm.GetDeptTopics();
        }

        /// <summary>
        /// 针对某种知识类型产生学习问题，
        /// </summary>
        /// <param name="topic">某种类型语义项目的语义网名称</param>
        /// <returns></returns>
        public abstract PQA CreateSpecificPQA(string topic); 

        /// <summary>
        /// 围绕语义网的单个连接提出问题，跟语义网类型无关。
        /// </summary>
        /// <param name="topic">语义网的名称</param>
        /// <returns></returns>
        public virtual PQA CreateSingleRelPQA(string topic)
        {
            TopicModule tm = new TopicModule(_krModule, topic);
            SemanticNet net = tm.KRModuleSNet.Net;
            
            PQA sqa = new PQA(topic,new Problem(tm.KRModuleSNet.Story));
            foreach(var e in net.Edges)
            {
                SNEdge edge = (SNEdge)e;
                string first = edge.Source.Name;
                string second = edge.Destination.Name;
                string rational = edge.Rational.Rational;
                string label = edge.Rational.Label;

                ///如果没有标签或使用默认的英文标签，则转换为默认的中文标签
                if (label == string.Empty || label == rational)
                {
                    if(SNRational.CHN.ContainsKey(rational))
                        label = SNRational.CHN[rational];
                }                 

                TypeType tt = SNRelTypeType.TopType[rational];

                if (tt==TypeType.IS)
                {
                    if (rational == SNRational.ISA)
                    { 
                        AddQAs(ref sqa, first + "是什么？", second);
                    }
                    else if (rational == SNRational.IS)
                    {
                        if (!KCNames.Names.Contains(second))
                        { 
                            AddQAs(ref sqa, first + "与" + second + "之间存在什么关系？", "是一种");
                        }
                    }
                    else if (rational == SNRational.ISO)
                    { 
                        AddQAs(ref sqa, second + "是由什么组成的？", first);
                    }
                    else if (rational == SNRational.ISP)
                    {
                        int i = Rand.Random(2);
                        if (i == 0)
                        { 
                            AddQAs(ref sqa, first + "是什么的一部分？", second);
                        }
                        else if (i == 1)
                        { 
                            AddQAs(ref sqa, first + "与" + second + "有什么关系？", first, second, "一部分");
                        }
                    }
                    else if (rational == SNRational.ISC)
                    { 
                        AddQAs(ref sqa, second + "有什么特征？", first);
                    }
                    else if (rational == SNRational.AREC)
                    { 
                        AddQAs(ref sqa, "文中提到了什么特征的" + second + "？", first);
                    }
                    else if (rational == SNRational.ISPG)
                    { 
                        AddQAs(ref sqa, first + "是什么的一部分？", second);
                    }
                    else if (rational == SNRational.ATT)
                    { 
                        AddQAs(ref sqa, second + "是什么的属性？", first);
                    }
                    else if (rational == SNRational.VAL)
                    {
                        AddQAs(ref sqa,first + "的值是多少？", second);
                    }
                    else if (rational == SNRational.VALR)
                    {

                    }

                }
                else if(tt==TypeType.ASS)
                {
                    if (rational == SNRational.CAUSAL)
                    {
                        int i = Rand.Random(2);
                        if(i==0)
                        { 
                            AddQAs(ref sqa, first + "的原因是什么？", second);
                        }
                        else if(i==1)
                        { 
                            AddQAs(ref sqa, second + "造成了什么结果？", first);
                        }
                    }
                    else if(rational==SNRational.CONFM)
                    { 
                        AddQAs(ref sqa, first + "依据的什么？", second);
                    }
                    else if(rational==SNRational.DEPT)
                    { 
                        AddQAs(ref sqa, first + "依赖什么条件？", second);
                    }
                    else if(rational==SNRational.ATTCH)
                    { 
                        AddQAs(ref sqa, first + "与" + second + "有什么关系？", label);
                    }
                    else if(rational==SNRational.IMPL)
                    { 
                        AddQAs(ref sqa, first + "隐含或意味什么？", second);
                    }
                    else if(rational==SNRational.ORIG)
                    { 
                        AddQAs(ref sqa, first + "的来源是什么？", second);
                    } 

                }
                else if(tt==TypeType.TS)
                {
                    if(rational==SNRational.TIME)
                    { 
                        AddQAs(ref sqa, first + "的时间是什么？", second);
                    }
                    else if (rational == SNRational.ANTE)
                    {
                        int i = Rand.Random(2);
                        if (i == 0)
                        { 
                            AddQAs(ref sqa, "什么事情先于" + second + "发生？", first);
                        }
                        else if (i == 1)
                        { 
                            AddQAs(ref sqa, first + "和" + second + "，哪个后发生？", second);
                        }

                    }
                    else if(rational==SNRational.STRT)
                    { 
                        AddQAs(ref sqa, first + "是什么时候开始的？", second);
                    }
                    else if(rational==SNRational.DUR)
                    { 
                        AddQAs(ref sqa, first + "多少时间？", second);
                    }
                    else if(rational==SNRational.DEST)
                    { 
                        AddQAs(ref sqa, first + "的目的是什么？", second);
                    }
                    else if(rational==SNRational.TDEST)
                    { 
                        AddQAs(ref sqa, first + "是什么时候结束的？", second);
                    }
                    else if(rational==SNRational.SPACE)
                    { 
                        AddQAs(ref sqa, first + "在" + second + "的什么位置？", label);
                    }
                    else if(rational==SNRational.LRANG)
                    { 
                        AddQAs(ref sqa, "请给出" + first + "的空间范围？", second);
                    }
                    else if(rational==SNRational.LOC)
                    { 
                        AddQAs(ref sqa, "在什么地方" + first + "？", second);
                    }
                    else if(rational==SNRational.ORIGL)
                    { 
                        AddQAs(ref sqa, first + "是在什么地方开始的？", second);
                    }
                    else if(rational==SNRational.DIR)
                    { 
                        AddQAs(ref sqa, "请给出" + first + "的方向？", second);
                    }
                    else if(rational==SNRational.PATH)
                    { 
                        AddQAs(ref sqa, first + "的路径是什么？", second);
                    }

                }
                else if(tt==TypeType.ACT)///ACT中的连接大部分在CreateMultiRelSQA()函数中处理
                {
                    if (rational == SNRational.AFFED)
                    {
                        int i = Rand.Random(2);
                        if (i == 0)
                        { 
                            AddQAs(ref sqa, second + "受到什么影响？", first);
                        }
                        else if (i == 1)
                        { 
                            AddQAs(ref sqa, second + "发生了什么？", first);
                        }
                    }
                    else if(rational==SNRational.CSTR)
                    { 
                        AddQAs(ref sqa, first + "是由什么造成的？", second);
                    }
                    else if(rational==SNRational.EXECR)
                    { 
                        AddQAs(ref sqa, "运行或发生了什么？", second);
                    }
                    else if(rational==SNRational.SUPPL)
                    { 
                        AddQAs(ref sqa, first + "什么？", second);
                    }
                }
                else if(tt==TypeType.COMP)
                {
                    if (rational == SNRational.CORR)
                    { 
                        AddQAs(ref sqa, first + "与什么相当，或对应什么？", second);
                    }
                    else if(rational==SNRational.CNVRS || rational==SNRational.CONTR ||
                        rational==SNRational.COMPL)
                    { 
                        AddQAs( ref sqa,first + "的反面是什么？", second);
                    }
                    else if(rational==SNRational.CONC)
                    { 
                        AddQAs(ref sqa, "尽管什么？" + first, second);
                    }
                    else if (rational == SNRational.ANLG)
                    { 
                        AddQAs(ref sqa,second + "与什么" + edge.Rational.Label + "？", first);
                    }
                    else if (rational == SNRational.ANLG2)
                    { 
                        AddQAs( ref sqa,"在什么方面，所有的" + first + "都类似？", second);
                    }
                    else if (rational == SNRational.ANTO)
                    {
                        int i = Rand.Random(2);
                        if (i == 0)
                        { 
                            AddQAs(ref sqa,first + "与" + second + "之间存在什么关系？", label);
                        }
                        else if (i == 1)
                        { 
                            AddQAs(ref sqa,first + "的" + label + "是什么？", second);
                        }
                    }
                    else if (rational == SNRational.SYNO)
                    { 
                        AddQAs(ref sqa,first + "与什么相同或等价？", second);
                    }

                }
                else if(tt==TypeType.RES)
                {
                    if(rational==SNRational.RANGE)
                    { 
                        AddQAs(ref sqa,first + "的范围是什么？", second );
                    }
                    else if(rational==SNRational.REF)
                    { 
                        AddQAs(ref sqa,first + "是相对或参考什么？", second);
                    }
                    else if(rational==SNRational.AMONG)
                    { 
                        AddQAs(ref sqa,first + "是处于什么之间的？", second);
                    }
                    else if(rational==SNRational.COND)
                    { 
                        AddQAs(ref sqa,first + "的条件是什么？", second);
                    }
                    else if(rational==SNRational.CONTXT)
                    { 
                        AddQAs(ref sqa,first + "是相对什么而言的？", second);
                    }
                    else if(rational==SNRational.INIT)
                    { 
                        AddQAs(ref sqa,second + "是什么的初始值或状态？", first);
                    }
                    else if(rational==SNRational.FINAL)
                    { 
                        AddQAs(ref sqa,second + "是什么的终止值或状态？", first);
                    }
                    else if(rational==SNRational.ORIGM)
                    { 
                        AddQAs(ref sqa,first + "的材料是什么？", second);
                    }
                    else if(rational==SNRational.QMOD)
                    { 
                        AddQAs(ref sqa,"多少的" + first + "?", second);
                    }
                    else if(rational==SNRational.NUM)
                    { 
                        AddQAs(ref sqa,first + "的数目或数量是多少？", second );
                    }
                    else if(rational==SNRational.UNIT)
                    { 
                        AddQAs(ref sqa,first + "所使用的量词或单位是什么？", second );
                    }
                }
                else if(tt==TypeType.MATH)
                {
                    if(rational==SNRational.COMP)
                    {
                        int i = Rand.Random(2);
                        if(i==0)
                        { 
                            AddQAs(ref sqa,"什么" + label + second + "？", first);
                        }
                        else if(i==1)
                        { 
                            AddQAs(ref sqa,first + label + "什么？", second);
                        }
                    }
                    else if(rational==SNRational.MAJ || rational==SNRational.MAJE)
                    { 
                        AddQAs(ref sqa,first + "的下限是什么？", second);
                    }
                    else if(rational==SNRational.MIN || rational==SNRational.MINE)
                    { 
                        AddQAs(ref sqa,first + "的上限是什么？", second);
                    }
                    else if(rational==SNRational.OPPS)
                    { 
                        AddQAs(ref sqa,first + "的相反数是什么？", second);
                    }
                    else if(rational==SNRational.INVR)
                    { 
                        AddQAs(ref sqa,second + "是什么的倒数？", first);
                    }
                    else if(rational==SNRational.EQUL)
                    { 
                        AddQAs(ref sqa,first + "与什么相等？", second);
                    }

                }
                else if(tt==TypeType.OTH)
                {
                    if(rational==SNRational.HAS)
                    {
                        int i = Rand.Random(2);
                        if(i==0)
                        { 
                            AddQAs(ref sqa,first + "有什么？", second);
                        }
                        else if(i==1)
                        { 
                            AddQAs(ref sqa,second + "的所有者是哪个？", first);
                        }
                    }
                    else if(rational==SNRational.BENF)
                    { 
                        AddQAs(ref sqa,label + "什么" + first + "？", second );
                    }
                    else if(rational==SNRational.FORM)
                    { 
                        AddQAs(ref sqa,first+"以什么形式？", second);
                    }
                    else if(rational==SNRational.SUBST)
                    { 
                        AddQAs(ref sqa,"在<" + topic + ">中，使用了" + first + "，而没有使用什么？",
                            second);
                    }
                } 

            }

            return sqa;
        }
        /// <summary>
        /// 针对语义网中的多个连接进行提问，与语义网类型无关
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        protected virtual PQA CreateMultiRelPQA(string topic)
        {
            TopicModule tm = new TopicModule(_krModule, topic);
            SemanticNet net = tm.KRModuleSNet.Net;

            PQA sqa = new PQA(topic, new Problem(tm.KRModuleSNet.Story));

            foreach(var n in net.Vertices)
            {
                SNNode node = (SNNode)n;
                if(net.IsActionNode(node))
                {
                    ACTParseInfo act = new ACTParseInfo(node, net);
                    List<System.Tuple<string, string[]>> qas;
                    act.ProduceQAs(out qas);
                    AddQAs(ref sqa, qas);
                }

                ATTParseInfo att = new ATTParseInfo(node, net);
                if(att.AttNodes.Count!=0)
                {
                    List<System.Tuple<string, string[]>> qas;
                    att.ProduceQAs(out qas);
                    AddQAs(ref sqa, qas);
                }

                if(net.IsAssignedNode(node))
                {
                    ASSGNParseInfo assgn = new ASSGNParseInfo(node, net);
                    List<Tuple<string,string[]>> qas;
                    assgn.ProduceQAs(out qas);
                    AddQAs(ref sqa, qas);

                } 

                if(net.IsExprNode(node))
                {
                    EXPRParseInfo expr = new EXPRParseInfo(node, net);
                    List<Tuple<string, string[]>> qas;
                    expr.ProduceQAs(out qas);
                    AddQAs(ref sqa, qas);
                }

                if(SemanticNet.IsProblemNode(node))
                {
                    ProblemParseInfo prob = new ProblemParseInfo(node, net);
                    List<System.Tuple<string, string[]>> qas;
                    prob.ProduceQAs(out qas);
                    AddQAs(ref sqa, qas);
                }
            }

            return sqa;
        }

        protected void AddQAs(ref PQA pqa, string q, params string[] a)
        {
            QAPair pa = new QAPair(new Question(q),
                new TextAnswer(a));
            pqa.AddQA(pa);
        }

        protected void AddQAs(ref PQA pqa,double[] icd,string q, params string [] a)
        {
            QAPair pa = new QAPair(new Question(q,null, icd), new TextAnswer(a));
            pqa.AddQA(pa);
        }

        protected void AddQAs( ref PQA pqa, List<System.Tuple<string, string[]>> qas)
        {
            foreach (var qa in qas)
            {
                QAPair pa = new QAPair(new Question(qa.Item1,null, new double[] { 0.1, 0.2, 0.1 }), new TextAnswer(qa.Item2));
                pqa.AddQA(pa);
            }
        }

        protected void AddQAs(ref PQA pqa,double[] icd,string q,ProceduralAnswer a)
        {
            QAPair qa = new QAPair(new Question(q,null, icd), a);
            pqa.AddQA(qa);
        }

        /// <summary>
        /// 添加QAPair
        /// </summary>
        /// <param name="pqa"></param>
        /// <param name="icd"></param>
        /// <param name="q"></param>
        /// <param name="a"></param>
        protected void AddQAs(ref PQA pqa,double[]icd,Question q,Answer a)
        {
            QAPair qa = new QAPair(q, a);
            pqa.AddQA(qa);
        }

    }
}
