using DomainLanguageInterpreter.DataLoading;
using System;

namespace DomainLanguageInterpreter.Arithmetic
{
	class Delay : IArithmetic
	{
		private readonly IArithmetic _value1;
		private readonly IArithmetic _value2;

		public Delay(IArithmetic value1, IArithmetic value2)
		{
			_value1 = value1;
			_value2 = value2;
		}

		public double GetValue(DateTime date)
		{
			double val2 = _value2.GetValue(date);
			int value = (int)(Math.Abs(val2) + 0.5)*Math.Sign(val2);
			if (Data.Instance.GetHistoricalDate(date, value, out DateTime previousDate))
			{
				return _value1.GetValue(previousDate);
			}
			return default;
		}

		public string Key => $"[Delay({_value1.Key},{_value2.Key})]";
	}
}
