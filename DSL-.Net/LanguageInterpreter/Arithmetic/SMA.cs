using DomainLanguageInterpreter.DataLoading;
using System;

namespace DomainLanguageInterpreter.Arithmetic
{
	class SMA : IArithmetic
	{
		private readonly IArithmetic _value1;
		private readonly IArithmetic _value2;
		public SMA(IArithmetic value1, IArithmetic value2)
		{
			_value1 = value1;
			_value2 = value2;
		}

		public double GetValue(DateTime date)
		{
			//To keep it simple IArithmetic outputs doubles only and as a result the SMA receives doubles only as a period.
			//Thus to get the correct integer of moving average steps we add 0.5 and truncate (ie. cast) it.
			int value = (int)(_value2.GetValue(date) + 0.5);
			if (value > 0)
			{
				double sum = 0;
				for (int i = 0; i < value; ++i)
				{
					if (Data.Instance.GetHistoricalDate(date, i, out DateTime previousDate))
					{
						sum += _value1.GetValue(previousDate);
					}
					else
					{
						throw new Exception($"Value not found for SMA on {date:yyyy-MM-dd}");
					}
				}
				
				return sum / value;
			}
			throw new Exception($"Can only take an SMA over period >= 1");
		}

		public string Key => $"[SMA({_value1.Key},{_value2.Key})]";
	}
}
