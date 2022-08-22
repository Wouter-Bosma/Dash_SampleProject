using System;

namespace DomainLanguageInterpreter.Arithmetic
{
	class Power : IArithmetic
	{
		private readonly IArithmetic _value1;
		private readonly IArithmetic _value2;
		public Power(IArithmetic value1, IArithmetic value2)
		{
			_value1 = value1;
			_value2 = value2;
		}

		public double GetValue(DateTime date)
		{
			return Math.Pow(_value1.GetValue(date), _value2.GetValue(date));
		}
		public string Key => $"[Power({_value1.Key},{_value2.Key})]";
	}
}
