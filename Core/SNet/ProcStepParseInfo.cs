using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 算法步骤的解析
    /// </summary>
    public class ProcStepParseInfo
    {
        //步骤节点
        protected SNNode _stepNode;
        //步骤涉及的操作节点
        protected List<SNNode> _oprNodes;

        public SNNode StepNode
        {
            get { return _stepNode; }
        }

        public ProcStepParseInfo()
        {

        }
    }
}
