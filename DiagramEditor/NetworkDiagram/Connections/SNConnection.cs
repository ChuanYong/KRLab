 

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using KRLab.Core; 
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using KRLab.DiagramEditor.NetworkDiagram.Dialogs;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram.Connections
{
	internal sealed class SNConnection : Connection
	{
		static Pen linePen = new Pen(Color.Black); 
        static SolidBrush lineBrush = new SolidBrush(Color.Black);
        static SolidBrush textBrush = new SolidBrush(Color.Black);
        static StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);

        internal SNRelationship SNRelationship
		{
			get { return (SNRelationship)Relationship; }
		}

		static SNConnection()
		{
			linePen.MiterLimit = 2.0F;
			linePen.LineJoin = LineJoin.MiterClipped;
		}
         
        public SNConnection(SNRelationship conn, Shape startShape, Shape endShape)
			: base(conn, startShape, endShape)
		{
            SNRelationship.Label = conn.Label;
		}

		protected override bool IsDashed
		{
			get { return base.IsDashed; }
		}

		protected override Size EndCapSize
		{
			get { return Arrowhead.OpenArrowSize; }
		}

        protected override int EndSelectionOffset
        {
            get
            {
                return Arrowhead.ClosedArrowHeight;
            }
        }

		protected override void DrawEndCap(IGraphics g, bool onScreen, Style style)
		{
			linePen.Color = style.RelationshipColor;
			linePen.Width = style.RelationshipWidth;
            g.FillPath(Brushes.White, Arrowhead.ClosedArrowPath);
            g.DrawPath(linePen, Arrowhead.ClosedArrowPath);
		}    

        public override void ShowEditDialog()
        {
            using (SNConnectionDialog dialog = new SNConnectionDialog())
            {
                dialog.SNRelationship = SNRelationship;
                dialog.ShowDialog();
            }
        }

		protected override bool CloneRelationship(Diagram diagram, Shape first, Shape second)
		{ 

            BasicSemanticNode firstType = first.Entity as BasicSemanticNode;
            BasicSemanticNode secondType = second.Entity as BasicSemanticNode;

            if (firstType != null && secondType != null)
            {
                SNRelationship clone = SNRelationship.Clone(firstType, secondType);
                return diagram.InsertSNRelationship(clone);
            }
            else
            {
                return false;
            }
		} 

        protected override void DrawStartRole(IGraphics g, Style style)
        {
            string startRole = SNRelationship.StartRole;
            if (startRole != null)
            {
                DrawRole(g, style, startRole, RouteCache[0], RouteCache[1], StartCapSize);
            }
        }

        protected override void DrawEndRole(IGraphics g, Style style)
        {
            string endRole = SNRelationship.EndRole;
            if (endRole != null)
            {
                int last = RouteCache.Count - 1;
                DrawRole(g, style, endRole, RouteCache[last], RouteCache[last - 1], EndCapSize);
            }
        }
        protected override void DrawStartMultiplicity(IGraphics g, Style style)
        {
            string startMultiplicity = SNRelationship.StartMultiplicity;
            if (startMultiplicity != null)
            {
                DrawMultiplicity(g, style, startMultiplicity, RouteCache[0],
                    RouteCache[1], StartCapSize);
            }
        }

        protected override void DrawEndMultiplicity(IGraphics g, Style style)
        {
            string endMultiplicity = SNRelationship.EndMultiplicity;
            if (endMultiplicity != null)
            {
                int last = RouteCache.Count - 1;
                DrawMultiplicity(g, style, endMultiplicity, RouteCache[last],
                    RouteCache[last - 1], EndCapSize);
            }
        }


        private void DrawRole(IGraphics g, Style style, string text, Point firstPoint,
            Point secondPoint, Size capSize)
        {
            float angle = GetAngle(firstPoint, secondPoint);
            Point point = firstPoint;

            if (angle == 0) // Down
            {
                point.X += capSize.Width / 2 + TextMargin.Width;
                point.Y += style.ShadowOffset.Height + TextMargin.Height;
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
            }
            else if (angle == 90) // Left
            {
                point.X -= TextMargin.Width;
                point.Y -= capSize.Width / 2 + TextMargin.Height;
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Far;
            }
            else if (angle == 180) // Up
            {
                point.X += capSize.Width / 2 + TextMargin.Width;
                point.Y -= TextMargin.Height;
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Far;
            }
            else // Right
            {
                point.X += style.ShadowOffset.Width + TextMargin.Width;
                point.Y -= capSize.Width / 2 + TextMargin.Height;
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Far;
            }

            textBrush.Color = style.RelationshipTextColor;
            g.DrawString(text, style.RelationshipTextFont, textBrush, point, stringFormat);
        }



        private void DrawMultiplicity(IGraphics g, Style style, string text, Point firstPoint,
            Point secondPoint, Size capSize)
        {
            float angle = GetAngle(firstPoint, secondPoint);
            Point point = firstPoint;

            if (angle == 0) // Down
            {
                point.X -= capSize.Width / 2 + TextMargin.Width;
                point.Y += style.ShadowOffset.Height + TextMargin.Height;
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Near;
            }
            else if (angle == 90) // Left
            {
                point.X -= TextMargin.Width;
                point.Y += capSize.Width / 2 + TextMargin.Height;
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Near;
            }
            else if (angle == 180) // Up
            {
                point.X -= capSize.Width / 2 + TextMargin.Width;
                point.Y -= TextMargin.Height;
                stringFormat.Alignment = StringAlignment.Far;
                stringFormat.LineAlignment = StringAlignment.Far;
            }
            else // Right
            {
                point.X += style.ShadowOffset.Width + TextMargin.Width;
                point.Y += capSize.Width / 2 + TextMargin.Height;
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
            }

            textBrush.Color = style.RelationshipTextColor;
            g.DrawString(text, style.RelationshipTextFont, textBrush, point, stringFormat);
        }

    }
}
