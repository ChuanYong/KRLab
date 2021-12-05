 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRLab.Core.FuzzyEngine;

namespace KRLab.Core.FuzzyEngine
{
	/// <summary>
	/// Uses the Center of Gravity method to calculate the defuzzification of membership functions.
	/// This is faster alternative method to the other CoGDefuzzification class specifically for 
	/// trapezoid and triangle membership functions
	/// </summary>
	public class TrapezoidCoGDefuzzification : IDefuzzification
	{
		public Double Defuzzify(List<IMembershipFunction> functions)
		{
			if (functions.Any(f => false == f is TrapezoidMembershipFunction))
				throw new ArgumentException(ErrorMessages.AllMembershipFunctionsMustBeTrapezoid);

			Double numerator = 0;
			Double denominator = 0;

			foreach (var function in functions.Select(f => f as TrapezoidMembershipFunction))
			{
				var trapCent = TrapezoidCentroid(function);
				var trapArea = TrapezoidArea(function, trapCent);
				numerator += trapCent * trapArea;
				denominator += trapArea;
			}

			return (0 != denominator) ? numerator / denominator : 0;
		}

		private Double TrapezoidArea(TrapezoidMembershipFunction f, Double centroid)
		{
			return ((f.C - f.B) + (f.D - f.A)) * f.PremiseModifier;
		}

		private Double TrapezoidCentroid(TrapezoidMembershipFunction f)
		{
			var top = f.C - f.B;
			var bot = f.D - f.A;

			var tm = f.B + (top / 2.0); //top midpoint
			var bm = f.A + (bot / 2.0); //bottom midpoint
			if (tm == bm)
				return tm;

			//y value for the centroid
			var y = ((2.0 * top) + bot) / (top + bot);
			y = y / 3.0;

			var mRecip = (tm - bm); // mRecip =  (1 / m)
			var b = (bm / mRecip);

			return (y + b) * mRecip;
		}
	}
}
