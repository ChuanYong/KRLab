 

using System;
using System.Xml;

using KRLab.Core.SNet;

namespace KRLab.Core
{
	public abstract class Relationship : Element, ISerializableElement
	{
		string label = string.Empty; 
		bool attached = false;

		public event EventHandler Attaching;
		public event EventHandler Detaching;
		public event SerializeEventHandler Serializing;
		public event SerializeEventHandler Deserializing;

		public abstract IEntity First
		{
			get;
			protected set;
		}

		public abstract IEntity Second
		{
			get;
			protected set;
		}

		public abstract RelationshipType RelationshipType
		{
            get; 
		}         

		public virtual string Label
		{
			get
			{
				return label;
			}
			set
			{
				if (value == "")
					value = null;
				
				if (label != value && SupportsLabel)
				{
					label = value;
					Changed();
				}
			}
		}

		public virtual bool SupportsLabel
		{
			get { return false; }
		}

        public virtual bool SupportsEndStartRole
        {
            get { return false; }
        }

		/// <exception cref="RelationshipException">
		/// Cannot finalize relationship.
		/// </exception>
		internal void Attach()
		{
			if (!attached)
				OnAttaching(EventArgs.Empty);
			attached = true;
		}

		public void Detach()
		{
			if (attached)
				OnDetaching(EventArgs.Empty);
			attached = false;
		}

		protected virtual void CopyFrom(Relationship relationship)
		{
			label = relationship.label;
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
		public virtual void Serialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

            if (SupportsLabel && Label != null)
			{
				XmlElement labelNode = node.OwnerDocument.CreateElement("Label");
                labelNode.InnerText = Label.ToString();
				node.AppendChild(labelNode);
			}
			OnSerializing(new SerializeEventArgs(node));
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		public virtual void Deserialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("node"); 

			if (SupportsLabel)
			{
				XmlElement labelNode = node["Label"];
				if (labelNode != null)
				{
					if (labelNode.InnerText == "ACT")
						label = "ACTR";
					else if (labelNode.InnerText == "EXE")
						label = "EXECR";
					else if (labelNode.InnerText == "ENVIR")
						label = "CIRCU";
					else if (labelNode.InnerText == "TIME")
						label = "ANTE";
					else if (labelNode.InnerText == "IFTHEN")
						label = "COND";
					else if (labelNode.InnerText == "∫Õ")
						label = "AND";
					else if (labelNode.InnerText == "ªÚ")
						label = "OR";
					else if (labelNode.InnerText == "IS" && (KCNames.Names.Contains(Second.Name) ||
						Second.Name == "À„∑®"))
						label = "KTYPE"; 
					else
						Label = labelNode.InnerText; 
				}
				else
					label = string.Empty;
			}
			OnDeserializing(new SerializeEventArgs(node));
		}

		protected virtual void OnAttaching(EventArgs e)
		{
			if (Attaching != null)
				Attaching(this, e);
		}

		protected virtual void OnDetaching(EventArgs e)
		{
			if (Detaching != null)
				Detaching(this, e);
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

	}
}
