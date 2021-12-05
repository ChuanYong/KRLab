

using System;
using System.Xml;

namespace KRLab.Core
{
	public abstract class Element : IModifiable
	{
		bool isDirty = false;
		bool initializing = false;
		int dontRaiseRequestCount = 0;

		public event EventHandler Modified;

		public bool IsDirty
		{
			get { return isDirty; }
		}

		public virtual void Clean()
		{
			isDirty = false;
		}

		protected bool Initializing
		{
			get { return initializing; }
			set { initializing = value; }
		}

		protected bool RaiseChangedEvent
		{
			get
			{
				return (dontRaiseRequestCount == 0);
			}
			set
			{
				if (!value)
					dontRaiseRequestCount++;
				else if (dontRaiseRequestCount > 0)
					dontRaiseRequestCount--;

				if (RaiseChangedEvent && isDirty)
					OnModified(EventArgs.Empty);
			}
		}

		protected void Changed()
		{
			if (!Initializing)
			{
				if (RaiseChangedEvent)
					OnModified(EventArgs.Empty);
				else
					isDirty = true;
			}
		}

		private void OnModified(EventArgs e)
		{
			isDirty = true;
			if (Modified != null)
				Modified(this, e);
		}
	}
}