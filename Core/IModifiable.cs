

using System;

namespace KRLab.Core
{
	public interface IModifiable
	{
		event EventHandler Modified;

		bool IsDirty { get; }

		void Clean();
	}
}
