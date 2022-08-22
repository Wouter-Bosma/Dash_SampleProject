using System;

namespace DomainLanguageInterpreter.Arithmetic
{

	public interface IArithmetic
	{
		double GetValue(DateTime step);
		string Key { get; }
	}
}
