using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utilities;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 解析一个主体所执行的动作或行为。 
    /// （1）主体与行为用ACT连接，一个主体可以有多个行为。
    /// （2）行为可以一个或多个主体。
    /// （3）行为除了主体之外，还可以有0个或多个被执行的客体,用ACTED指明。
    /// （4）行为可以有0个或多个结果，用RESULT关系连接。
    /// （5）行为有发生的时间和空间环境，分别用SPACE和TIME指明。
    /// （6）行为可以有0个或多个发生的条件，用DEPT关系指明。
    /// （7）如果主体有多个行为，可以用TIME连接指明它们的先后顺序。
    /// （8）主体可以有属性结点与行为相连，表示该行为对该属性会产生影响。
    ///      如果主体有多个行为，用ASSOC关系将某属性和指定的行为关联起来，指明
    ///      行为的对主体的性质的影响。
    /// （9）一个行为可以有多个主体，每个主体得到的结果有可能不一样，这时候，用ASSOC
    ///      将指定的结果进行关联。
    ///（10）行为的目的。
    ///（11）行为的原因。
    ///      
    /// 
    ///             【客体】    
    ///  【属性】        \          【发生条件】  
    ///    |              \          /
    ///    |               \       /
    /// 【主体1】<---------【行为1】------>【结果】
    ///     \                  |  \  \
    ///      \                 |    \  \
    ///       \            TIME|     \  【时间环境】
    ///        \               |      \
    ///       【行为2】        |  【空间环境】
    ///                       \|/
    ///                    【行为3】 
    ///                       
    /// ACTParseInfo按一个行为进行建模                        
    /// </summary>
    public class ACTParseInfo : ParseInfo
    {
        //行为结点
        protected SNNode _actNode;

        protected SNNode _timeNode;//一个行为只能有一个时间环境
        protected SNNode _locNode;//一个行为只能有一个空间环境

        //行为主体
        protected SNNode _actorNode;
        //客体
        protected SNNode _actedNode;
        //条件
        protected SNNode _condNode;
        protected List<SNNode> _outCauseNodes;
        protected List<SNNode> _inCauseNodes;
        //行为结果，结果可能会有其关联的条件
        protected List<SNNode> _resultNodes;
        //一个行为可以有多个过往行为
        protected List<SNNode> _postNodes;
        //一个行为可以有多个未来行为
        protected List<SNNode> _preNodes;

        //伪动词
        public static string Larger = "大于";
        public static string Smaller = "小于";
        public static string Equal = "等于";
        public static string Increase = "增";
        public static string Decrease = "减";

        public static Dictionary<string, List<string>> Synonym = new Dictionary<string, List<string>>()
        {
            { Increase,new List<string>(){"越大","越强","越高"} },
            { Decrease,new List<string>(){"越小","越弱","越低"} },
        };


        public SNNode ActNode
        {
            get { return _actNode; }
        }
        public SNNode TimeNode
        {
            get { return _timeNode; }
        }
        public SNNode LocNode
        {
            get { return _locNode; }
        }
        public SNNode ActorNode
        {
            get { return _actorNode; }
        }
        public SNNode ActedNode
        {
            get { return _actedNode; }
        }
        public SNNode CondNode
        {
            get { return _condNode; }
        }
        public List<SNNode> ResultNodes
        {
            get { return _resultNodes; }
        }
        public List<SNNode> PostNodes
        {
            get { return _postNodes; }
        }
        public List<SNNode> PreNodes
        {
            get { return _preNodes; }
        }

        /// <summary>
        /// 要确保actNode不为null
        /// </summary>
        /// <param name="actNode"></param>
        /// <param name="net"></param>
        public ACTParseInfo(SNNode actNode, SemanticNet net) : base(net)
        {
            _actNode = actNode;
            Parse();
        }

        protected void Parse()
        {
            _timeNode = _net.GetOutgoingDestination(_actNode, SNRational.TIME);
            _locNode = _net.GetOutgoingDestination(_actNode, SNRational.LOC);

            _actorNode = _net.GetIncomingSource(_actNode, SNRational.ACTR);
            _actedNode = _net.GetIncomingSource(_actNode, SNRational.ACTED);
            _condNode = _net.GetOutgoingDestination(_actNode, SNRational.COND);

            _outCauseNodes = _net.GetOutgoingDestinations(_actNode, SNRational.CAUSAL);
            _inCauseNodes = _net.GetIncomingSources(_actNode, SNRational.CAUSAL);
            _resultNodes = _net.GetOutgoingDestinations(_actNode, SNRational.RESULT);

            _postNodes = _net.GetIncomingSources(_actNode, SNRational.ANTE);
            _preNodes = _net.GetOutgoingDestinations(_actNode, SNRational.ANTE);

        }

        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            qas = new List<System.Tuple<string, string[]>>();

            if (ActorNode != null)
            {
                string actor = ActorNode.Name;
                string behavior = string.Empty;

                if (ActedNode != null)
                    behavior = ActNode.Name + ActedNode.Name;
                else if (ActedNode == null)
                    behavior = ActNode.Name;

                System.Tuple<string, string[]> qa = new System.Tuple<string, string[]>(actor + "做了什么？",
                    new[] { behavior });
                qas.Add(qa);

                List<SNNode> andNodes;
                if (Net.IsAndOrNode(ActorNode, out andNodes) == 1)
                {
                    int i = Rand.Random(andNodes.Count);
                    qa = new System.Tuple<string, string[]>(andNodes[i] + behavior + "了吗",
                        new[] { "是" });
                    qas.Add(qa);
                }

                if (CondNode != null)
                {
                    CONDParseInfo cond = new CONDParseInfo(CondNode, _net);
                    qa = new System.Tuple<string, string[]>(actor + behavior + "，是在什么条件下？",
                        new[] { cond.CondInfo });
                    qas.Add(qa);

                    List<System.Tuple<string, string[]>> tmp;
                    cond.ProduceQAs(out tmp);
                    foreach(var tp in tmp)
                    {
                        qa = new System.Tuple<string, string[]>(actor + behavior + tp.Item1 + "?",
                            tp.Item2);
                        qas.Add(qa);
                    }
                }
                if (TimeNode != null)
                {
                    qa = new System.Tuple<string, string[]>(actor + behavior + "，是在什么时候？",
                        new[] { TimeNode.Name });
                    qas.Add(qa);
                }
                if (LocNode != null)
                {
                    qa = new System.Tuple<string, string[]>(actor + behavior + "，是在什么地方？",
                        new[] { LocNode.Name });
                    qas.Add(qa);
                }

                if (ResultNodes.Count != 0)
                {
                    string[] results = new string[ResultNodes.Count];
                    for (int i = 0; i < ResultNodes.Count; i++)
                    {
                        results[i] = ResultNodes[i].Name;
                    }
                    qa = new System.Tuple<string, string[]>(actor + behavior + "，有些什么结果？",
                        results);
                    qas.Add(qa);
                }

            }
        }


        public static void Check(List<IEntity> entities, List<Relationship> relations,
            Action<bool, string> callback)
        {
            List<IEntity> actNodes = new List<IEntity>();
            foreach (var rl in relations)
            {
                SNRelationship snr = (SNRelationship)rl;

                if (snr.SNRelationshipType.ToString() == SNRational.ACT)
                {
                    actNodes.Add(snr.Second);
                }

            }
            foreach (var actNode in actNodes)
            {
                List<IEntity> actorNodes = new List<IEntity>();

                List<IEntity> resultNodes = new List<IEntity>();
                List<IEntity> deptNodes = new List<IEntity>();
                foreach (var rl in relations)
                {
                    SNRelationship snr = (SNRelationship)rl;

                    if (snr.SNRelationshipType.ToString() == SNRational.RESULT && snr.First == actNode)
                    {
                        resultNodes.Add(snr.Second);
                    }
                    if (snr.SNRelationshipType.ToString() == SNRational.DEPT && snr.First == actNode)
                    {
                        deptNodes.Add(snr.Second);
                    }

                }
                if (resultNodes.Count > 1)
                {
                    bool isOk1 = false;
                    bool isOk2 = false;
                    foreach (var ie in resultNodes)
                    {
                        foreach (var rl in relations)
                        {
                            SNRelationship snr = (SNRelationship)rl;

                            if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.First == ie
                                && actorNodes.Contains(snr.Second))
                            {
                                isOk1 = true;
                            }
                            if (snr.SNRelationshipType.ToString() == SNRational.ASSOC && snr.Second == ie
                                && deptNodes.Contains(snr.First))
                            {
                                isOk2 = true;
                            }

                        }
                    }

                    if (actorNodes.Count > 1 && !isOk1)
                    {
                        callback(false, "有多个主体结点和结果结点，应该将结果结点与主体结点的关联");
                        return;
                    }
                    if (deptNodes.Count > 1 && !isOk2)
                    {
                        callback(false, "有多个条件结点和结果结点，应该讲结果结点与条件结点关联起来");
                        return;
                    }
                }


            }

        }

    }
}
