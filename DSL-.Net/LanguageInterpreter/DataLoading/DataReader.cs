using System;
using System.Collections.Generic;
using System.IO;

namespace DomainLanguageInterpreter.DataLoading
{
	/// <summary>
	/// This ReadData class is a helper class to read the data from a file and expose it to the environment.
	/// This is to abstract the file.IO in a separate class.
	/// </summary>
	class DataReader
	{
		private static readonly char[] separators = { '\t', ',', ';' };

		public static SortedDictionary<DateTime, Dictionary<string, double>> Read(string fileName)
		{
			var data = new SortedDictionary<DateTime, Dictionary<string, double>>();
			if (File.Exists(fileName))
            {
                try
                {
                    bool isFirst = true;
                    var header = new Dictionary<int, string>();
                    var headerColumns = new Dictionary<string, int>();
                    using StreamReader sr = new StreamReader(fileName);
                    while (!sr.EndOfStream)
                    {
                        string readLine;
                        if (string.IsNullOrEmpty(readLine = sr.ReadLine()))
                        {
                            continue;
                        }
                        readLine = readLine.ToUpper();
                        var tokens = readLine.Split(separators);

                        if (isFirst)
                        {
                            ParseHeader(tokens, headerColumns, header);
                            isFirst = false;
                        }
                        else
                        {
                            ParseLine(tokens, headerColumns, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error reading the file.\n" + ex);
                }
            }
			if (data.Count == 0)
			{
				throw new Exception("No data is loaded, check if the filename / contents / formatting are correct.");
			}
			return data;
		}

        private static void ParseHeader(string[] tokens, Dictionary<string, int> headerColumns, Dictionary<int, string> header)
        {
			for (int i = 0; i < tokens.Length; ++i)
            {
                header[i] = tokens[i];
                headerColumns[tokens[i]] = i;
            }
        }

        private static void ParseLine(string[] tokens, Dictionary<string, int> headerColumns, SortedDictionary<DateTime, Dictionary<string, double>> data)
        {
            int dateColumn = headerColumns["DATETIME"];
            DateTime date = DateTime.ParseExact(tokens[dateColumn], "yyyy-MM-dd HH:mm:ss", null);
            var tempDico = new Dictionary<string, double>();
            bool allOk = true;
            foreach (var columnInfo in headerColumns)
            {
                if (columnInfo.Key == "DATETIME") continue;
                
                if (double.TryParse(tokens[columnInfo.Value], out double value))
                {
                    tempDico[columnInfo.Key] = value;
                }
                else
                {
                    allOk = false;
                    break;
                }
            }
            if (allOk)
            {
                data[date] = tempDico;
            }
		}
	}
}
