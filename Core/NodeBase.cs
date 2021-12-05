using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

using KRLab.Translations;

namespace KRLab.Core 
{
    public abstract class NodeBase:Element,IEntity
    {
        string name;


        public event SerializeEventHandler Serializing;
        public event SerializeEventHandler Deserializing;

        protected NodeBase(string name)
        {
            //Initializing = true;
            //Name=name;
            //Initializing=false;
        }

        public virtual string Name
        {
            get { return name; }
            set
            {
                string newName = value;
                if (newName != name)
                {
                    name = newName;
                    Changed();
                }
            }
        }

        public abstract EntityType EntityType
        {
            get;
        }


        public abstract NodeBase Clone();

        protected static bool MoveUp(IList list, object item)
        {
            if (item == null)
                return false;

            int index = list.IndexOf(item);
            if (index > 0)
            {
                object temp = list[index - 1];
                list[index - 1] = list[index];
                list[index] = temp;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected static bool MoveDown(IList list, object item)
        {
            if (item == null)
                return false;

            int index = list.IndexOf(item);
            if (index >= 0 && index < list.Count - 1)
            {
                object temp = list[index + 1];
                list[index + 1] = list[index];
                list[index] = temp;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void CopyFrom(NodeBase node)
        {
            name = node.name; 
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
        protected internal virtual void Serialize(XmlElement node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            XmlElement child;

            child = node.OwnerDocument.CreateElement("Name");
            child.InnerText = Name;
            node.AppendChild(child);

            OnSerializing(new SerializeEventArgs(node));
        }

        /// <exception cref="BadSyntaxException">
        /// An error occured whiledeserializing.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The XML document is corrupt.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="node"/> is null.
        /// </exception>
        protected internal virtual void Deserialize(XmlElement node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            RaiseChangedEvent = false;
            XmlElement nameChild = node["Name"];
            if (nameChild != null)
                Name = nameChild.InnerText;
             

            RaiseChangedEvent = true;
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
            return Name ;
        }
    }
}
