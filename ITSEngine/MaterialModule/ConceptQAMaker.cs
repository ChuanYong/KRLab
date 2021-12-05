using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Utilities;
using KRLab.Core.SNet;
using ITS.DomainModule;

namespace ITS.MaterialModule
{
    public class ConceptQAMaker
    {
        //protected StoryMaker _storyMaker;
        //protected List<SNEdge> _UsedEdges;

        //public ConceptQAMaker(SemanticNet net) : base(net)
        //{
        //    _storyMaker = new StoryMaker(net);
        //}
        /// <summary>
        /// 查询能否从该概念进行提问，调用一次即可，不要频繁调用。
        /// </summary>
        /// <param name = "concept" ></ param >
        /// < returns ></ returns >
        //public bool IsCanStart(string concept)
        //{
        //    SNNode node = SNet.FindNode(concept);
        //    if (node == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}


        //public string ProduceCalculationQuestion()
        //{
        //    string str = "For a linear motion with constant acceleration a, time t, initial velocity v0, and finial velocity v1 \n";
        //    str += "(1)please write down the equation for these parameters.\n";
        //    str += "(2)please write down the formula of the final velocity.\n";
        //    str += "(3)please write down the formula of the acceleration.";
        //    return str;
        //    return Question.QuestionCalculationStory(_storyMaker, _formulaMaker);
        //}

        //public void AppraiseResults()
        //{

        //}

        /// <summary>
        /// 该算法首先随机选取一条进入的边，顺着该边一直提问，直到没有出口的结点。
        /// 适合于从顶端结点开始进行提问。
        /// </summary>
        /// <returns></returns>
        //public string ProduceTextQuestionInPath(SNNode currentNode)
        //{

        //    SNEdge edge = FindEdge(currentNode, true);

        //    查找进来的边，主要有IS,ISA,ISO,ISP
        //    if (edge != null)
        //    {
        //        string ques = _ChooseRational(edge);
        //        currentNode = edge.Source;
        //        return ques;

        //    }
        //    else //查找出去的边，主要有ATT, ACT,ACTED,RESULT
        //    {
        //        edge = FindEdge(currentNode, false);
        //        if (edge != null)
        //        {
        //            string ques = _ChooseRational(edge);
        //            currentNode = edge.Destination;
        //            return ques;
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }

        //}


        //private string _ChooseRational(SNEdge edge)
        //{
        //    if (edge.Rational.Rational == SNRational.IS)
        //    {
        //        return Question.QuestionIS(edge);
        //    }
        //    else if (edge.Rational.Rational == SNRational.ISO)
        //    {
        //        return Question.QuestionISO(edge);
        //    }
        //    else if (edge.Rational.Rational == SNRational.ISP)
        //    {
        //        return Question.QuestionISP(edge);
        //    }
        //    else if (edge.Rational.Rational == SNRational.ISA)
        //    {
        //        return Question.QuestionISA(edge);
        //    }

        //    else if (edge.Rational.Rational == SNRational.ATT)
        //    {
        //        if (SNet.IfActionNode(edge.Source))
        //        {
        //            List<SNEdge> edges = SNet.GetOutgoingEdges(edge.Source);
        //            SNEdge act = null, acted = null, result = null;
        //            foreach (SNEdge e in edges)
        //            {
        //                if (e.Rational.Rational == SNRational.ACT)
        //                    act = e;
        //                else if (e.Rational.Rational == SNRational.ACTED)
        //                    acted = e;
        //                else if (e.Rational.Rational == SNRational.RESULT)
        //                    result = e;
        //            }
        //            return Question.QuestionActionATT(edge, act, acted, result);
        //        }

        //        return Question.QuestionAtt(edge);
        //    }

        //    else if (edge.Rational.Rational == SNRational.SPACE)
        //    {
        //        return Question.QuestionSPACE(edge);
        //    }
        //    else if (edge.Rational.Rational == SNRational.TIME)
        //    {
        //        return Question.QuestionTIME(edge);
        //    }
        //    else if (edge.Rational.Rational == SNRational.RESULT)
        //    {
        //        List<SNEdge> edges = SNet.GetOutgoingEdges(edge.Source);
        //        SNEdge act = null, acted = null;
        //        foreach (SNEdge e in edges)
        //        {
        //            if (e.Rational.Rational == SNRational.ACT)
        //                act = e;
        //            if (e.Rational.Rational == SNRational.ACTED)
        //                acted = e;
        //        }
        //        return Question.QuestionRESULT(edge, act, acted);

        //    }

        //    else if (edge.Rational.Rational == SNRational.ACTED)
        //    {
        //        List<SNEdge> edges = SNet.GetOutgoingEdges(edge.Source);
        //        SNEdge result = null, act = null;
        //        foreach (SNEdge e in edges)
        //        {
        //            if (e.Rational.Rational == SNRational.RESULT)
        //                result = e;
        //            if (e.Rational.Rational == SNRational.ACT)
        //                act = e;
        //        }
        //        return Question.QuestionACTED(edge, act, result);
        //    }
        //    else if (edge.Rational.Rational == SNRational.DEPT)
        //    {
        //        return Question.QuestionSIMILAR(edge);
        //    }
        //    else if (edge.Rational.Rational == SNRational.COMP)
        //    {
        //        return Question.QuestionLOGIC(edge);
        //    }
        //    else
        //    {
        //        return ConstStrings.NullQuestion;
        //    }

        //}


        /// <summary>
        /// 随机查找该结点的连接线，已经用过的排除在外。
        /// </summary>
        /// <param name = "node" ></ param >
        /// < returns ></ returns >
        //protected SNEdge FindEdge(SNNode node, bool isIn)
        //{
        //    if (node == null)
        //    {
        //        return null;
        //    }

        //    List<SNEdge> neighbors;
        //    if (isIn)
        //        neighbors = SNet.GetIncomingEdges(node);
        //    else
        //        neighbors = SNet.GetOutgoingEdges(node);

        //    去除已经用过的
        //    foreach (SNEdge e in _UsedEdges)
        //    {
        //        neighbors.Remove(e);
        //    }

        //    if (neighbors.Count() == 0)
        //    {
        //        return null;
        //    }

        //    int nb = neighbors.Count;
        //    Random rand = new Random();
        //    int index = rand.Next(0, nb);
        //    SNEdge edge = neighbors[index];
        //    _UsedEdges.Add(edge);

        //    return edge;

        //}
    }
}
