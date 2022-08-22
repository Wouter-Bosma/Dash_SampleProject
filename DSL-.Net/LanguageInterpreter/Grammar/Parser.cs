using System;
using System.Collections.Generic;
using DomainLanguageInterpreter.Arithmetic;

namespace DomainLanguageInterpreter.Grammar
{
	/// <summary>
	/// Task of the grammar is to convert the token stream into an arithmetic that can execute the requested language request. See scanner for further information.
	/// </summary>
	class Parser
	{
		private Queue<Token> _tokens;
		public Parser(Queue<Token> tokens)
		{
			_tokens = tokens;
		}
		public IArithmetic Level1()
		{
			IArithmetic first = Level2();
			if (_tokens.Count == 0)
			{
				return first;
			}
			Token peek = _tokens.Peek();
			while (true)
			{
				switch (peek.Type)
				{
					case TokenType.Addition:
						_tokens.Dequeue();
						first = new Addition(first, Level2());
						break;
					case TokenType.Subtraction:
						_tokens.Dequeue();
						first = new Subtraction(first, Level2());
						break;
					case TokenType.ParameterSeparator:
						_tokens.Dequeue();
						return first;
					default:
						return first;
				}
				if (_tokens.Count == 0)
				{
					return first;
				}
				peek = _tokens.Peek();
			}
		}
		private IArithmetic Level2()
		{
			IArithmetic second = Level3();
			if (_tokens.Count == 0)
			{
				return second;
			}
			Token peek = _tokens.Peek();
			while (true)
			{
				switch (peek.Type)
				{
					case TokenType.Multiplication:
						_tokens.Dequeue();
						second = new Multiplication(second, Level3());
						break;
					case TokenType.Division:
                        _tokens.Dequeue();
                        second = new Division(second, Level3());
                        break;
					case TokenType.Power:
						_tokens.Dequeue();
						second = new Power(second, Level3());
						break;
					default:
						return second;
				}
				if (_tokens.Count == 0)
				{
					return second;
				}
				peek = _tokens.Peek();
			}
		}

		private void RetrieveInputParameters(int n, List<IArithmetic> arithmetics, string name)
		{
			if (_tokens.Count == 0 || _tokens.Peek().Type != TokenType.BracketOpen)
			{
				throw new Exception($"Expected to have bracket open after {name}");
			}
			_tokens.Dequeue();
			for (int i = 0; i < n; ++i)
			{
				arithmetics.Add(Level1());
			}
			if (_tokens.Count == 0 || _tokens.Peek().Type != TokenType.BracketClose)
			{
				throw new Exception($"Expected to have bracket close after two {name} parameters");
			}
			_tokens.Dequeue();
		}

		private IArithmetic Level3()
		{
			Token token = _tokens.Dequeue();
			while (true)
			{
				switch (token.Type)
				{
					case TokenType.BracketOpen:
						IArithmetic final = Level1();
						token = _tokens.Dequeue();
						if (token.Type == TokenType.BracketClose)
						{
							return final;
						}
						break;
					case TokenType.Subtraction:
						return new Subtraction(new Constant(0), Level1());
					case TokenType.Constant:
						return new Constant(token.Value);
					case TokenType.Open:
						return new RawValue("OPEN");
					case TokenType.High:
						return new RawValue("HIGH");
					case TokenType.Low:
						return new RawValue("LOW");
					case TokenType.Close:
						return new RawValue("CLOSE");
					case TokenType.Volume:
						return new RawValue("VOLUME");
					case TokenType.SMA:
						{
							List<IArithmetic> arithmetics = new List<IArithmetic>();
							RetrieveInputParameters(2, arithmetics, "SMA");
							return new SMA(arithmetics[0], arithmetics[1]);
						}
					case TokenType.Delay:
						{
							List<IArithmetic> arithmetics = new List<IArithmetic>();
							RetrieveInputParameters(2, arithmetics, "DELAY");
							return new Delay(arithmetics[0], arithmetics[1]);
						}
					default:
						throw new Exception("Expected to have finalized tokens.");
				}
			}
		}
	}
}
