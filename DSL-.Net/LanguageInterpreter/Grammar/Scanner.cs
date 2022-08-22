using System;
using System.Collections.Generic;
using DomainLanguageInterpreter.Arithmetic;

namespace DomainLanguageInterpreter.Grammar
{
	public enum TokenType
	{
		Multiplication, //Basic * and ^ functionality
		Power,
		Division,

		Addition, //Addition and subtraction
		Subtraction,

		BracketOpen,
		BracketClose,

		ParameterSeparator,

		Constant, //Just the token for a numerical value
		Open, //OHLC - Volume are represented in arithmetic as RawValues
		High,
		Low,
		Close,
		Volume,
		Delay, //Programmed arithmetics
		SMA,
	}

	public class Token
	{
		public TokenType Type;
		public double Value;
	}

	public class Scanner
	{
		private bool GetToken(string item, out Token token)
		{
			if (string.IsNullOrEmpty(item))
			{
				token = new Token();
				return false;
			}
			if (double.TryParse(item, out double value))
			{
				token = new Token { Value = value, Type = TokenType.Constant };
				return true;
			}
			else
			{
				switch (item)
				{
					case "OPEN":
						token = new Token { Type = TokenType.Open };
						return true;
					case "HIGH":
						token = new Token { Type = TokenType.High };
						return true;
					case "LOW":
						token = new Token { Type = TokenType.Low };
						return true;
					case "CLOSE":
						token = new Token { Type = TokenType.Close };
						return true;
					case "VOLUME":
						token = new Token { Type = TokenType.Volume };
						return true;
					case "SMA":
						token = new Token { Type = TokenType.SMA };
						return true;
					case "DELAY":
						token = new Token { Type = TokenType.Delay };
						return true;
				}
			}
			token = new Token();
			return false;
		}

		private IEnumerable<Token> GetTokens(string line)
		{
			string item = string.Empty;
			Token token;
			foreach (char x in line.ToUpper())
			{
				if (x >= 'A' && x <= 'Z')
				{
					item += x;
					continue;
				}
				switch (x)
				{
					case '0':case '1':
					case '2':case '3':
					case '4':case '5':
					case '6':case '7':
					case '8':case '9':
					case '.':
						item += x;
						break;
					case ' ':
						if (GetToken(item, out token))
						{
							yield return token;
							item = string.Empty;
						}
						break;
					case '+':
						if (GetToken(item, out token))
						{
							yield return token;
							item = string.Empty;
						}
						yield return new Token() { Type = TokenType.Addition };
						break;
					case '(':
						if (GetToken(item, out token))
						{
							yield return token;
							item = string.Empty;
						}
						yield return new Token() { Type = TokenType.BracketOpen };
						break;
					case ')':
						if (GetToken(item, out token))
						{
							yield return token;
							item = string.Empty;
						}
						yield return new Token() { Type = TokenType.BracketClose };
						break;
					case '-':
						if (GetToken(item, out token))
						{
							yield return token;
							item = string.Empty;
						}
						yield return new Token() { Type = TokenType.Subtraction };
						break;
					case '^':
						if (GetToken(item, out token))
						{
							yield return token;
							item = string.Empty;
						}
						yield return new Token() { Type = TokenType.Power };
						break;
					case '*':
						if (GetToken(item, out token))
						{
							yield return token;
							item = string.Empty;
						}
						yield return new Token() { Type = TokenType.Multiplication };
						break;
                    case '/':
                        if (GetToken(item, out token))
                        {
                            yield return token;
                            item = string.Empty;
                        }
                        yield return new Token() { Type = TokenType.Division };
                        break;
					case ',':
						if (GetToken(item, out token))
						{
							yield return token;
							item = string.Empty;
						}
						yield return new Token() { Type = TokenType.ParameterSeparator };
						break;
					default:
						throw new Exception("Unexpected character encountered in the parsing of the equation");
				}
			}

			if (!GetToken(item, out token)) yield break;
			yield return token;
		}

		public IArithmetic ParseSyntax(string line)
		{
			Queue<Token> tokens = new Queue<Token>();
			tokens.Clear();
			foreach(Token token in GetTokens(line))
			{
				tokens.Enqueue(token);
			}

			Parser myGrammar = new Parser(tokens);

			IArithmetic result = myGrammar.Level1();
			if (tokens.Count != 0)
			{
				throw new Exception($"Parseable string {line} is incorrect, after parsing tokens left");
			}
			return result;
		}
	}
}
