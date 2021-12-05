 

using System;
using System.Collections.Generic;
using KRLab.Core;
using KRLab.DiagramEditor.NetworkDiagram.Shapes;

namespace KRLab.DiagramEditor.NetworkDiagram.Connections
{
	internal class CommentConnection : Connection
	{
		CommentRelation relationship;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="relationship"/> is null.-or-
		/// <paramref name="startShape"/> is null.-or-
		/// <paramref name="endShape"/> is null.
		/// </exception>
		public CommentConnection(CommentRelation relationship, Shape startShape, Shape endShape)
			: base(relationship, startShape, endShape)
		{
			this.relationship = relationship;
            CommentRelationship.Label = "";
		}

		internal CommentRelation CommentRelationship
		{
			get { return relationship; }
		}

		protected override bool IsDashed
		{
			get { return true; }
		}

		protected override bool CloneRelationship(Diagram diagram, Shape first, Shape second)
		{
			Comment comment = first.Entity as Comment;
			if (comment != null)
			{
				CommentRelation clone = relationship.Clone(comment, second.Entity);
				return diagram.InsertCommentRelationship(clone);
			}
			else
			{
				return false;
			}
		}

        public override void ShowEditDialog()
        {
            
        }
	}
}
