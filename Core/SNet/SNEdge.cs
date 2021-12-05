using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.DataStructures.Graphs;

namespace KRLab.Core.SNet
{
    public class SNEdge:WeightedEdge<SNNode>
    {
        public SNRational Rational
        {
            get { return Weight as SNRational; }
        }

        public SNEdge()
        {
        }

        public SNEdge(SNNode first, SNNode second,SNRational rational)
            : base(first, second,rational)
        {
        }
    }
}
