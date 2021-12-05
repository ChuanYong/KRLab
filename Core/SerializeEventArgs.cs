 

using System;
using System.Xml;

namespace KRLab.Core
{
	public delegate void SerializeEventHandler(object sender, SerializeEventArgs e);

	public class SerializeEventArgs : EventArgs
	{
		XmlElement node;

		public SerializeEventArgs(XmlElement node)
		{
			this.node = node;
		}

		public XmlElement Node
		{
			get { return node; }
		}
	}
}