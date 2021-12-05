 
using KRLab.Core.FuzzyEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KRLab.Core.FuzzyEngine
{
	public class GaussianMembershipFunction : BaseMembershipFunction
	{
		public GaussianMembershipFunction(String name, Double c, Double tou, Double min, Double max)
			: base(name)
		{
			if (0 == tou)
				throw new ArgumentException(ErrorMessages.TouArgumentIsInvalid);

			_c = c;
			_tou = tou;
			_min = min;
			_max = max;
		}

		public GaussianMembershipFunction(String name, Double c, Double tou)
			: this(name, c, tou, 0, 200)
		{
		}

		private Double _c;
		private Double _tou;
		private Double _min;
		private Double _max;

		#region Public Methods

		public override Double Fuzzify(Double inputValue)
		{
			//http://www.wolframalpha.com/input/?i=e%5E%28%28-1%2F2%29%28%28x-50%29%2F20%29%5E2%29+for+x+%3D+50+

			var power = -0.5 * Math.Pow((inputValue - _c) / _tou, 2.0);
			return Math.Min(1.0, Math.Exp(power));
		}

		public override Double Min()
		{
			return _min;
		}

		public override Double Max()
		{
			return _max;
		}

		#endregion


	}
}
