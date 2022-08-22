using System;

namespace DomainLanguageInterpreter.Arithmetic
{
	class Addition : IArithmetic
	{
		readonly IArithmetic _value1;
		readonly IArithmetic _value2;

		public Addition(IArithmetic value1, IArithmetic value2)
		{
			_value1 = value1;
			_value2 = value2;
		}
		public double GetValue(DateTime date)
		{
			return _value1.GetValue(date) + _value2.GetValue(date);
		}

		public string Key => $"[Addition({_value1.Key},{_value2.Key})]";
	}
}
