using KRLab.Core.SNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DomainModule
{
    public class ProceduralStep
    {
        protected SNNode _stepNode;

        public SNNode StepNode
        {
            get { return _stepNode; }
        }

        public ProceduralStep(SNNode stepNode)
        {
            _stepNode = stepNode;
        }
    }
}
