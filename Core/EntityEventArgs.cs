 

using System;

namespace KRLab.Core
{
	public delegate void EntityEventHandler(object sender, EntityEventArgs e);

	public class EntityEventArgs : EventArgs
	{
		IEntity entity;

		public EntityEventArgs(IEntity entity)
		{
			this.entity = entity;
		}

		public IEntity Entity
		{
			get { return entity; }
		}
	}
}
