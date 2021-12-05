
using System;
using System.Runtime.Serialization;
using KRLab.Translations;

namespace KRLab.Core
{
	public class ReservedNameException : BadSyntaxException
	{
		string name;

		public ReservedNameException()
			: base(Strings.ErrorReservedName)
		{
			name = null;
		}

		public ReservedNameException(string name)
			: base(Strings.ErrorReservedName)
		{
			this.name = name;
		}

		public ReservedNameException(string name, Exception innerException)
			: base(Strings.ErrorReservedName, innerException)
		{
			this.name = name;
		}

		public string ReservedName
		{
			get { return name; }
		}
	}
}
