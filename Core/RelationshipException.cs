

using System;
using System.Runtime.Serialization;
using KRLab.Translations;

namespace KRLab.Core
{
	public class RelationshipException : Exception
	{
		public RelationshipException() : base(Strings.ErrorCannotCreateRelationship)
		{
		}

		public RelationshipException(string message) : base(message)
		{
		}

		public RelationshipException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RelationshipException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
