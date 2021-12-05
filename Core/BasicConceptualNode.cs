using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace KRLab.Core
{
    public class BasicConceptualNode:NodeBase
    { 

        public BasicConceptualNode(string name) :
            base(name)
        {
        }
        public override EntityType EntityType
        {
            get { return Core.EntityType.ConceptualNode; }
        }

        public override NodeBase Clone()
        {
            BasicConceptualNode node = new BasicConceptualNode("NewConceptNode");
            node.CopyFrom(this);
            return node;
        }
    }
}
