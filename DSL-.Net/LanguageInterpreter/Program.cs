using System;
using System.IO;
using DomainLanguageInterpreter.DataLoading;

namespace DomainLanguageInterpreter
{
	/// <summary>
	/// Assumptions:
	/// - Thread safety is not an issue
	/// 
	/// Program development
	/// - Bottom up -> First read data -> Implement data manipulation Arithmetic -> Implement scanner/parser for reading text files -> save data
	/// 
	/// Design patterns used in this challenge:
	/// -> Interpreter for the implementation of the grammar (Class: Parser)
	/// -> Singleton for accessing the data globally within the project (Class: Data)
	/// -> Decorator to solve calculations (Class: IArithmetic)
	/// -> Facade to give an easy interface for the end implementation (Class: ProcessData)
	/// </summary>
	class Program
	{
		/// <summary>
		/// Structure of the program:
		/// - Parse arguments
		/// - Load the alphas => Output relevant information (num of alphas) for debug purposes
		/// - Load data => Output size of data loaded
		/// - Run the analysis and save the file => Output relevant data like: Progress bar, dates skipped (possibly due to delay and histo moving averages)
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			try
			{
				string alphaFile;
				string instrumentFile;
				string outputFile;
				if (args.Length == 2 || args.Length == 3)
				{
					alphaFile = args[0];
					instrumentFile = args[1];
					if (args.Length > 2)
					{
						outputFile = args[2];
					}
					else
					{
						outputFile = Path.Combine(Path.GetDirectoryName(instrumentFile), "Output.csv");
					}
				}
				else
				{
					Console.WriteLine("Usage is <Alpha File Name> <Instrument file name> Optional:<Output file name>");
					throw new Exception("Insufficient command line arguments given, at least required to supply alpha file name and instrument data file name.");
				}

				//Constructing the opbject that does do the work
				ProcessData pd = new ProcessData(alphaFile, instrumentFile, outputFile);

				//Make it load the alpha
				int alphas = pd.LoadAlphas();
				Console.WriteLine($"Loaded {alphas} alphas and converted them to arithmetics");

				//Make it load the data
				pd.LoadData();
				
				Console.WriteLine($"Read {Data.Instance.Size} lines of data.");
				//Running the analysis
				pd.RunDataAnalysis();

				//saving the data
				pd.SaveData();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
