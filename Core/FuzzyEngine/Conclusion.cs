 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using KRLab.Core.FuzzyEngine;

namespace KRLab.Core.FuzzyEngine
{
	/// <summary>
	/// The then-part of the rule
	/// </summary>
	public class Conclusion : FuzzyRuleClause
	{
		public Conclusion(LinguisticVariable variable, IFuzzyRuleToken @operator, IMembershipFunction function)
			: base(variable, @operator, function)
		{
		}

	}
}
