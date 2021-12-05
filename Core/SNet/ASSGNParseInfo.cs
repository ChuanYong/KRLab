using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    public class ASSGNParseInfo:ParseInfo
    {
        protected SNNode _rightNode;
        protected SNNode _leftNode;

        public ASSGNParseInfo(SNNode node,SemanticNet net):
            base(net)
        {
            _leftNode = node;
            _rightNode = net.GetOutgoingDestination(node, SNRational.ASSGN);
        }

        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            qas = new List<System.Tuple<string, string[]>>();
            SNNode leftNumNode = Net.GetOutgoingDestination(_leftNode, SNRational.NUM);
            SNNode leftUnitNode = Net.GetOutgoingDestination(_leftNode, SNRational.UNIT);
            SNNode rightNumNode = Net.GetOutgoingDestination(_rightNode, SNRational.NUM);
            SNNode rightUnitNode = Net.GetOutgoingDestination(_rightNode, SNRational.UNIT);

            string q = string.Empty;
            string a = string.Empty;
            if(leftNumNode!=null && leftUnitNode!=null && 
                rightUnitNode!=null && rightNumNode!=null)
            {
                q= "请给出" + leftNumNode.Name+leftUnitNode.Name + _leftNode.Name + "等于多少";
                a = rightNumNode.Name + rightUnitNode.Name + _leftNode.Name;
            }
            else if(leftNumNode==null && leftUnitNode==null &&
                rightNumNode!=null & rightUnitNode!=null)
            {
                q= "请给出" + _leftNode.Name + "等于多少";
                a = rightNumNode.Name + rightUnitNode.Name + _rightNode.Name;
            }
            else if(leftNumNode!=null && leftUnitNode==null &&
                rightNumNode!=null && rightUnitNode==null)
            {
                q= "请给出" + leftNumNode.Name + _leftNode.Name + "等于多少";
                a = rightNumNode.Name + _leftNode.Name;
            } 
            else
            {
                q = "请给出" + _leftNode.Name + "等于多少";
                a = _rightNode.Name;
            }

            qas.Add(new System.Tuple<string, string[]>(q, new[] { a }));
        }
    }
}
