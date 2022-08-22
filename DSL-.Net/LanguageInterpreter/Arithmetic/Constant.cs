using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLanguageInterpreter.Arithmetic
{
	class Constant : IArithmetic
	{
		private readonly double _value;
		public Constant(double value)
		{
			_value = value;
		}

		public double GetValue(DateTime date)
		{
			return _value;
		}

		public string Key => $"[Constant({_value})]";
	}
}
