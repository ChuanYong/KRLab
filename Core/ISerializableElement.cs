

using System;
using System.Xml;

namespace KRLab.Core
{
	public interface ISerializableElement
	{
		event SerializeEventHandler Serializing;
		event SerializeEventHandler Deserializing;

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		void Serialize(XmlElement node);

		/// <exception cref="BadSyntaxException">
		/// An error occured while deserializing.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The XML document is corrupt.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		void Deserialize(XmlElement node);
	}
}
