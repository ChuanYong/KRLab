 

using System;
using System.Xml;
using KRLab.Translations;

namespace KRLab.Core
{
	public sealed class Comment : Element, IEntity
	{
		string text = string.Empty;

		public event SerializeEventHandler Serializing;
		public event SerializeEventHandler Deserializing;

		public Comment()
		{
		}
	
		public Comment(string text)
		{
			this.text = text;
		}

		public EntityType EntityType
		{
			get { return EntityType.Comment; }
		}

		public string Name
		{
			get { return Strings.Comment; }
		}

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				if (value == null)
					value = string.Empty;

				if (text != value) {
					text = value;
					Changed();
				}
			}
		}

		public Comment Clone()
		{
			return new Comment(this.text);
		}

		void ISerializableElement.Serialize(XmlElement node)
		{
			Serialize(node);
		}

		void ISerializableElement.Deserialize(XmlElement node)
		{
			Deserialize(node);
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		internal void Serialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			XmlElement child = node.OwnerDocument.CreateElement("Text");
			child.InnerText = Text;
			node.AppendChild(child);

			OnSerializing(new SerializeEventArgs(node));
		}

		/// <exception cref="BadSyntaxException">
		/// An error occured while deserializing.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The XML document is corrupt.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		internal void Deserialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			XmlElement textNode = node["Text"];

			if (textNode != null)
				Text = textNode.InnerText;
			else
				Text = null;

			OnDeserializing(new SerializeEventArgs(node));
		}

		private void OnSerializing(SerializeEventArgs e)
		{
			if (Serializing != null)
				Serializing(this, e);
		}

		private void OnDeserializing(SerializeEventArgs e)
		{
			if (Deserializing != null)
				Deserializing(this, e);
		}

		public override string ToString()
		{
			const int MaxLength = 50;

			if (Text == null) {
				return Strings.Comment;
			}
			else if (Text.Length > MaxLength) {
				return '"' + Text.Substring(0, MaxLength) + "...\"";
			}
			else {
				return '"' + Text + '"';
			}
		}
	}
}
