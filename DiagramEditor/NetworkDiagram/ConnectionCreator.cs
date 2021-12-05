 

using System;
using System.Drawing;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using KRLab.Translations;

namespace KRLab.DiagramEditor.NetworkDiagram
{
	internal class ConnectionCreator
	{
		const int BorderOffset = 8;
		const int BorderOffset2 = 12;
		const int Radius = 5;
		static readonly float[] dashPattern = new float[] { 3, 3 };
		static readonly Pen firstPen;
		static readonly Pen secondPen;
		static readonly Pen arrowPen;

		Diagram diagram;
		RelationshipType type;
		bool firstSelected = false;
		bool created = false;
		Shape first = null;
		Shape second = null;
        Point _CurrentMousePos;

		static ConnectionCreator()
		{
			firstPen = new Pen(Color.Blue);
			firstPen.DashPattern = dashPattern;
			firstPen.Width = 1.5F;
			secondPen = new Pen(Color.Red);
			secondPen.DashPattern = dashPattern;
			secondPen.Width = 1.5F;
			arrowPen = new Pen(Color.Red); 
			arrowPen.CustomEndCap = new AdjustableArrowCap(6, 7, true);
		}

		public ConnectionCreator(Diagram diagram,RelationshipType type)
		{
			this.diagram = diagram;
			this.type = type;
		}

		public bool Created
		{
			get { return created; }
		} 

		public void MouseMove(AbsoluteMouseEventArgs e)
		{
            _CurrentMousePos = new Point((int)e.X, (int)e.Y);	
		
			foreach (Shape shape in diagram.Shapes)
			{
                if (shape.BorderRectangle.Contains(_CurrentMousePos))
				{
					if (!firstSelected)
					{
						if (first != shape)
						{
							first = shape;
							diagram.Redraw();
						}
					}
					else
					{
						if (second != shape)
						{
							second = shape;
							diagram.Redraw();
						}
					}
					return;
				}
			}

			if (!firstSelected)
			{
				if (first != null)
				{
					first = null;
					diagram.Redraw();
				}
			}
			else
			{
				if (second != null)
				{
					second = null;
					diagram.Redraw();
				}
			}
		}

		public void MouseDown(AbsoluteMouseEventArgs e)
		{ 
			if (!firstSelected)
			{
				if (first != null)
					firstSelected = true;
			} 
		}

        public void MouseUp(AbsoluteMouseEventArgs e)
        {
			if (first!=null && second != null)
				CreateConnection();
        }

		private void CreateConnection()
		{
            if(type==RelationshipType.SN_REL)
            {
                CreateSNRelationship(); 
            }  
			created = true;
			diagram.Redraw();
		}

        private void CreateSNRelationship()
        {
            NodeShape shape1 = first as NodeShape;
            NodeShape shape2 = second as NodeShape;

            if (shape1 != null && shape2 != null)
            {
                diagram.AddSNRelationship(shape1.Node, shape2.Node);
            }
            else
            {
                MessageBox.Show(Strings.ErrorCannotCreateRelationship);
            }
        }  
        

		public void Draw(Graphics g)
		{
			if (first != null)
			{
				Rectangle border = first.BorderRectangle;
				border.Inflate(BorderOffset, BorderOffset);
				g.DrawRectangle(firstPen, border);
                if (firstSelected)
                {    
                    g.DrawLine(arrowPen, first.CenterPoint, _CurrentMousePos);
                    diagram.Redraw();
                }

			}
			
			if (second != null)
			{
				Rectangle border = second.BorderRectangle;
				if (second == first)
					border.Inflate(BorderOffset2, BorderOffset2);
				else
					border.Inflate(BorderOffset, BorderOffset);
				g.DrawRectangle(secondPen, border);
			}
		}
	}
}
