using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace KRLab.Core
{
    public class CMRelationship: NodeRelationship
    {
 
        public override RelationshipType RelationshipType
        {
            get { return RelationshipType.CM_REL; }
        }
        public CMRelationship(BasicConceptualNode first, BasicConceptualNode second):
            base(first,second)
        {
        }
    }
}
