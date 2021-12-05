 

using System;
using System.Collections.Generic;
using System.Drawing;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;
using KRLab.DiagramEditor.NetworkDiagram.Connections;

namespace KRLab.DiagramEditor.NetworkDiagram
{
	internal class ElementContainer : IClipboardItem
	{
		const int BaseOffset = 20;

		List<Shape> shapes = new List<Shape>();
		List<Connection> connections = new List<Connection>();
		Dictionary<Shape, Shape> pastedShapes = new Dictionary<Shape, Shape>();
		int currentOffset = 0;

		public void AddShape(Shape shape)
		{
			shapes.Add(shape);
			pastedShapes.Add(shape, null);
		}

		public void AddConnection(Connection connection)
		{
			connections.Add(connection);
		}

		void IClipboardItem.Paste(IDocument document)
		{
			Diagram diagram = (Diagram) document;
			if (diagram != null)
			{
				bool success = false;

				currentOffset += BaseOffset;
                //Size offset = new Size(
                //    (int) ((diagram.Offset.X + currentOffset) / diagram.Zoom),
                //    (int) ((diagram.Offset.Y + currentOffset) / diagram.Zoom));

                Size offset = new Size((int)(currentOffset / diagram.Zoom),
                    (int)(currentOffset / diagram.Zoom));

				foreach (Shape shape in shapes)
				{
					Shape pasted = shape.Paste(diagram, offset);
					pastedShapes[shape] = pasted;
					success |= (pasted != null);
				}
				foreach (Connection connection in connections)
				{
					Shape first = GetShape(connection.Relationship.First);
					Shape second = GetShape(connection.Relationship.Second);

					if (first != null && pastedShapes[first] != null &&
						second != null && pastedShapes[second] != null)
					{
						Connection pasted = connection.Paste(
							diagram, offset, pastedShapes[first], pastedShapes[second]);
						success |= (pasted != null);
					}
				}

				if (success)
				{
					Clipboard.Clear();
				}
			}
		}

		//TODO: legyenek inkább hivatkozások a shape-ekhez
		public Shape GetShape(IEntity entity)
		{
			foreach (Shape shape in shapes)
			{
				if (shape.Entity == entity)
					return shape;
			}
			return null;
		}
	}
}
