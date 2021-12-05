 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRLab.Core.FuzzyEngine;

namespace KRLab.Core.FuzzyEngine
{
	public abstract class BaseMembershipFunction : FuzzyRuleToken, IMembershipFunction
	{
		#region ctors

		public BaseMembershipFunction(String name)
			: base(name, FuzzyRuleTokenType.Function)
		{

		}

		#endregion


		#region Private Properties

		private Double _premiseModifier = 0;

		#endregion

		#region public Properties

		public void Reset()
		{
			_premiseModifier = 0;
		}

		public Double PremiseModifier
		{
			get
			{
				return _premiseModifier;
			}
			set
			{
				if (value > _premiseModifier)
					_premiseModifier = value;
			}
		}

		[Obsolete("Use PremiseModifier instead. Modification passes through to PremiseModifier.")]
		public Double Modification
		{
			get
			{
				return PremiseModifier;
			}
			set
			{
				PremiseModifier = value;
			}
		}

		#endregion

		#region Abstract Methods

		public abstract Double Fuzzify(Double inputValue);

		public abstract Double Min();

		public abstract Double Max();

		#endregion
	}
}
