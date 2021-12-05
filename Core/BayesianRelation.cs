using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace KRLab.Core
{
    public class BayesianRelation:NodeRelationship
    {
 
        public override RelationshipType RelationshipType
        {
            get { return Core.RelationshipType.BN_REL; }
        }

        public BayesianRelation(NodeBase first, NodeBase second):
            base(first,second)
        {
        }

        public BayesianRelation(BasicBayesianNode first, BasicBayesianNode second) :
            base(first, second)
        {
        }

        public BayesianRelation Clone(BasicBayesianNode first, BasicBayesianNode second)
        {
            BayesianRelation rel = new BayesianRelation(first, second);
            rel.CopyFrom(this);
            return rel;
        }
    }
}
