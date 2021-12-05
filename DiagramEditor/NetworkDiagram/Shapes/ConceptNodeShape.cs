using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core; 

namespace KRLab.DiagramEditor.NetworkDiagram.Shapes
{
    public class ConceptNodeShape:SimpleNodeShape
    {
        private BasicConceptualNode _Node;

        public ConceptNodeShape(BasicConceptualNode node)
            : base(node)
        {
            _Node = node;
        }

        public override NodeBase Node
        {
            get { return _Node; }
        }
       
    }
}
