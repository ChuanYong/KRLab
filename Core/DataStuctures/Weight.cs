using KRLab.Core.SNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRLab.Core.DataStructures.Graphs
{
    public class CWeight:IComparable<CWeight>
    {
        protected long _weight;
        protected string _rational;
        protected SNRelationshipType _type;

        public long Weight
        {
            get { return _weight; }
        }

        public string Rational
        {
            get { return _rational; }
        }

        public CWeight(string rational,long weight)
        {
            _weight = weight;
            _rational = rational;
        }

        public CWeight(SNRelationshipType relationType,long weight)
        {
            _weight = weight;
            _type = relationType;
            _rational = relationType.ToString();
        }

        public int CompareTo(CWeight other)
        {
            if (other == null) return 1;
            else
                return _weight.CompareTo(other._weight);
        }
    }
}
