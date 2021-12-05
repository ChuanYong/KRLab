

using System;

namespace KRLab.Core
{
	public delegate void RelationshipEventHandler(object sender, RelationshipEventArgs e);

	public class RelationshipEventArgs : EventArgs
	{
		Relationship relationship;

		public RelationshipEventArgs(Relationship relationship)
		{
			this.relationship = relationship;
		}

		public Relationship Relationship
		{
			get { return relationship; }
		}
	}
}
