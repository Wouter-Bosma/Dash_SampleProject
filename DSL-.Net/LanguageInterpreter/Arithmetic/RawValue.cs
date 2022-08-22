using DomainLanguageInterpreter.DataLoading;
using System;

namespace DomainLanguageInterpreter.Arithmetic
{
	/// <summary>
	/// RawValue (by lack of a better name) is the accessor to the 
	/// </summary>
	class RawValue : IArithmetic
	{
		private readonly string _item;
		public RawValue(string item)
		{
			_item = item;
		}

		public double GetValue(DateTime date)
		{
			if (Data.Instance.TryGetValue(date, _item, out var value))
			{
				return value;
			}
			throw new Exception($"Value not foud for {_item} on {date:yyyy:MM:dd}");
		}
		public string Key => $"[Data({_item})]";
	}
}
