using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DomainLanguageInterpreter.Arithmetic;
using DomainLanguageInterpreter.Grammar;
using DomainLanguageInterpreter.DataLoading;

namespace DomainLanguageInterpreter
{
	class ProcessData
	{
		private string _alphasFile;
		private string _instrumentFile;
		private string _outputFile;
		private readonly SortedDictionary<string, IArithmetic> constructs = new SortedDictionary<string, IArithmetic>();
		private readonly StringBuilder output = new StringBuilder();
		public ProcessData(string alphasFile, string instrumentFile, string outputFile)
		{
			_alphasFile = alphasFile;
			_instrumentFile = instrumentFile;
			_outputFile = outputFile;
		}

		private List<string> ReadAlphas()
		{
			List<string> alphas = new List<string>();
			using StreamReader sr = new StreamReader(_alphasFile);
			while (!sr.EndOfStream)
			{
				var readLine = sr.ReadLine();
				if (!string.IsNullOrEmpty(readLine))
				{
					alphas.Add(readLine);
				}
			}

			return alphas;
		}

		private void ParseAlphas(List<string> alphas)
		{
			Scanner scanner = new Scanner();
			
			foreach (string line in alphas)
			{
				if (!string.IsNullOrEmpty(line))
				{
					string[] tokens = line.Split('=');
					if (tokens.Length != 2)
					{
						throw new Exception("Expected to find only lines with in the alphas with a name and equation");
					}
					constructs[tokens[0].Trim(' ')] = scanner.ParseSyntax(tokens[1]);
				}
			}
		}

		public int LoadAlphas()
		{
			constructs.Clear();
			var alphas = ReadAlphas();
			ParseAlphas(alphas);
			return constructs.Keys.Count;
		}

		public void LoadData()
		{
			Data.Instance.ReadData(_instrumentFile);
		}

		private bool CalculateConstruct(DateTime date, StringBuilder fails, StringBuilder result)
		{
			try
			{
				result.Append(date.ToString("yyyy/MM/dd HH:mm:ss"));
				foreach (var value in constructs.Values)
				{
					result.Append($";{value.GetValue(date)}");
				}
				result.Append(Environment.NewLine);
			}
			catch(Exception ex)
			{
				fails.AppendLine($"Date failed: {date:yyyy-MM-dd} with reason {ex.Message}");
				return false;
			}

			return true;
		}

		private void PrintProgress(int size, ref int counter, ref int next)
		{
			int progress = size != 0 ? (10 * counter++) / size : 0;
			if (size > 0 && progress >= next)
			{
				Console.Write("\r[");
				for(int i = 0; i < 10; ++i)
				{
					Console.Write($"{(i < next ? "X" : ".")}");
				}
				++next;
				Console.Write("]");
			}
		}

		public void RunDataAnalysis()
		{
			int size = Data.Instance.Size;
			output.Clear();
			output.Append($"Datetime");
			foreach (var key in constructs.Keys)
			{
				output.Append($";{key}");
			}
			output.Append(Environment.NewLine);

			StringBuilder sb2 = new StringBuilder();
			int counter = 0;
			int next = 1;
			Console.Write("[..........]");
			StringBuilder fails = new StringBuilder();
			foreach (DateTime date in Data.Instance.Dates())
			{
				sb2.Clear();
				if (CalculateConstruct(date, fails, sb2))
				{
					output.Append(sb2);
				}

				PrintProgress(size, ref counter, ref next);
			}
			Console.WriteLine("\r[XXXXXXXXXX]");
			Console.WriteLine(fails.ToString());
		}

		public void SaveData()
		{
			using StreamWriter sw = new StreamWriter(_outputFile);
			sw.WriteLine(output.ToString());
		}
	}
}
