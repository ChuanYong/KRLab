using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.SNet;
using ITS.DomainModule;

namespace ITS.MaterialModule
{

    /// <summary>
    /// 提出的问题是执行某个算法，所以答案是相关算法的执行过程
    /// </summary>
    public class ProceduralAnswer : Answer
    {
        //TopicModule对应的语义网中的某个问题，比如公式语义网中
        //的某个公式名称。
        protected string _qName;
        protected TopicModule _topicModule;
        protected ProceduralTopicModule _ptm;

        public override object Content => throw new NotImplementedException();


        /// <summary>
        /// 某个知识模块tm中涉及到了流程模块ptm，通过该类，获取流程模块的相关知识
        /// </summary>
        /// <param name="qName"></param>
        /// <param name="tm"></param>
        /// <param name="ptm"></param>
        public ProceduralAnswer(string qName,TopicModule tm, ProceduralTopicModule ptm)
        {
            _qName = qName;
            _topicModule = tm;
            _ptm = ptm;
        }

        /// <summary>
        /// 提供算法每个步骤的答案
        /// </summary>
        /// <param name="info">反馈算法在这个步骤的内容</param>
        /// <param name="correctResult">反馈算法在这个步骤执行得到的结果</param>
        public void ProvideStepAnswer(out string info,out string correctResult)
        {
            EquationTopicModule etm = _topicModule as EquationTopicModule;
            if(etm!=null)
            {
                ForEquationSolving(etm,out info, out correctResult);
            }
            else
            {
                info = string.Empty;
                correctResult = string.Empty;
            }
        }

        /// <summary>
        /// 为求解公式提供指导
        /// </summary>
        /// <param name="etm"></param>
        /// <param name="info"></param>
        /// <param name="correctResult"></param>
        protected void ForEquationSolving(EquationTopicModule etm,out string info,out string correctResult)
        {
            info = string.Empty;
            correctResult = string.Empty;

            List<SNNode> stepNodes = _ptm.StepNodes;
            foreach(var step in stepNodes)
            {
                
            }
        } 
    }
}
