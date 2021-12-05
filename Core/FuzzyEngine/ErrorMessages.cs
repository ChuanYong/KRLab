 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KRLab.Core.FuzzyEngine
{
	public static class ErrorMessages
	{
		public const String RulesAreInvalid = "One or more rules is invalid.";
		public const String InputValusMustBeValid = "Must provide a double, decimal, or integer input value for all variables. Missing: {0}";
		public const String MembershipFunctionsDefuzzType = "All membership functions must be {0} defuzz type.";
		public const String AllMembershipFunctionsMustBeTrapezoid = "All membership functions must be trapezoid and triangle.";
		public const String AArgumentIsInvalid = "Argument a cannot be zero.";
		public const String BArgumentIsInvalid = "Argument b cannot be zero.";
		public const String TouArgumentIsInvalid = "Argument tou cannot be zero.";
	}
}
