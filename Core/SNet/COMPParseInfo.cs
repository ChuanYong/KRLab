using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.SNet
{
    /// <summary>
    /// 要注意与其它连接一起进行解析
    /// </summary>
    public class COMPParseInfo:ParseInfo
    {
        SNNode _node1;
        SNNode _node2;

        public COMPParseInfo(SNNode node1,SNNode node2,SemanticNet net):base(net)
        {
            _node1 = node1;
            _node2 = node2;
        }

        public override void ProduceQAs(out List<System.Tuple<string, string[]>> qas)
        {
            throw new NotImplementedException();
        }

        public System.Tuple<string,string,string> ComRelation()
        { 
            SNRational ral = Net.Rational(_node1, _node2,new[] {SNRational.COMP,SNRational.MIN,
            SNRational.MINE,SNRational.MAJ,SNRational.MAJE});
            string label = GetLabel(ral);

            if(ral!=null && label!=null)
            {
                return new System.Tuple<string,string,string>(_node1.Name, label, _node2.Name);
            }
            ral = Net.Rational(_node2, _node1, new[] { SNRational.COMP });
            label = GetLabel(ral);
            if(ral!=null && label!=null)
            {
                return new Tuple<string, string, string>(_node2.Name, label, _node1.Name);
            }
            return null;
        }

        protected string GetLabel(SNRational ral)
        {
            if (ral == null)
                return null;

            if (ral.Rational == SNRational.COMP)
            {
                return ral.Label;
            }
            else if (ral.Rational == SNRational.MIN)
            {
                if (ral.Label == SNRational.MIN)
                    return "小于";
                else
                    return ral.Label;
            }
            else if (ral.Rational == SNRational.MINE)
            {
                if (ral.Label == SNRational.MINE)
                    return "小于等于";
                else
                    return ral.Label;
            }
            else if (ral.Rational == SNRational.MAJ)
            {
                if (ral.Label == SNRational.MAJ)
                    return "大于";
                else
                    return ral.Label;
            }
            else if(ral.Rational==SNRational.MAJE)
            {
                if (ral.Label == SNRational.MAJE)
                    return "大于等于";
                else
                    return ral.Label;
            }
            else
                return null;
        }
    }
}
