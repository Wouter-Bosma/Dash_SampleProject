using System;

namespace DomainLanguageInterpreter.Arithmetic
{
	class Multiplication : IArithmetic
	{
		private readonly IArithmetic _value1;
		private readonly IArithmetic _value2;

		public Multiplication(IArithmetic value1, IArithmetic value2)
		{
			_value1 = value1;
			_value2 = value2;
		}

		public double GetValue(DateTime date)
		{
			return _value1.GetValue(date) * _value2.GetValue(date);
		}

		public string Key => $"[Multiply({_value1.Key},{_value2.Key})]";
	}
}
