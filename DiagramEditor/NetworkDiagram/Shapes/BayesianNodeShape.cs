using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using KRLab.Core;

namespace KRLab.DiagramEditor.NetworkDiagram.Shapes
{
    public class BayesianNodeShape:CompositeNodeShape
    {
        CompositeNode _Node;

        public BayesianNodeShape(CompositeNode node)
            : base(node)
        {
            _Node = node;
        }

        protected override CompositeNode CompositeNode
        {
            get { return _Node; }
        }

        protected override Color GetBackgroundColor(Style style)
        {
            return style.NodeBackgroundColor;
        }

        protected override bool CloneEntity(Diagram diagram)
        {
            return diagram.InsertNode(CompositeNode.Clone());
        }

        protected override Color GetHeaderColor(Style style)
        {
            return style.NodeHeaderColor;
        }

        protected override GradientStyle GetGradientHeaderStyle(Style style)
        {
            return style.NodeGradientHeaderStyle;
        }

        protected override Color GetBorderColor(Style style)
        {
            return style.NodeBorderColor;
        }

        protected override int GetBorderWidth(Style style)
        {
            return style.NodeBorderWidth;
        }
        protected override int GetRoundingSize(Style style)
        {
            return style.ClassRoundingSize;
        }
        protected override bool IsBorderDashed(Style style)
        {
            return style.IsNodeBorderDashed;
        }
    }
}
