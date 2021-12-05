
using System;
using System.Xml;

namespace KRLab.Core
{
	public interface IEntity : ISerializableElement, IModifiable
	{
		string Name
		{
			get;
		}

		EntityType EntityType
		{
			get;
		}
	}
}
