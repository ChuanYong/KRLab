using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core;


namespace KRLab.DiagramEditor.NetworkDiagram.Shapes
{
    public class SemanticNodeShape:SimpleNodeShape
    {
        private BasicSemanticNode _Node;

        public SemanticNodeShape(BasicSemanticNode node)
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
