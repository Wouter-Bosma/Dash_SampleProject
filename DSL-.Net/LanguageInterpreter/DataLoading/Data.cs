using System;
using System.Collections.Generic;

namespace DomainLanguageInterpreter.DataLoading
{
	public class Data
	{
		private readonly Dictionary<DateTime, int> indexedDates = new Dictionary<DateTime, int>();
		private readonly Dictionary<int, DateTime> indexedDatesReversed = new Dictionary<int, DateTime>();
		/// <summary>
		/// Structure of the data is Date => Item (ie. open/high/low/close/volume) => value
		/// Sorted dictionary => To make the IEnumerable<> direct to implement
		/// </summary>
		/// 
		/// Sorted dictionary (ie. binary tree) is chosen as it is conceptually important to emphasize the order of data although a dictionary (hashed dico) would most likely be more performant. With faster O(1) lookups.
		private SortedDictionary<DateTime, Dictionary<string, double>> data = null;
		private static Data _instance = null;

		public int Size => data.Count;

		/// <summary>
		/// Used a singleton so that data can be used accross all objects. Simplifies the arithmetic classes.
		/// </summary>
		public static Data Instance =>
			//Note this is not a threadsafe implementation of a singleton as that would require the null check => lock => additional null check => possibly create object depending on null check.
			_instance ??= new Data();

		private Data()
		{
		}

		public void ReadData(string fileName)
		{
			data = DataReader.Read(fileName);
			IndexData();
		}

		/// <summary>
		/// Data is indexed to make random access and random access look-up faster
		/// </summary>
		/// <returns></returns>
		private void IndexData()
		{
			indexedDates.Clear();
			indexedDatesReversed.Clear();
			int counter = 0;
			foreach (DateTime date in data.Keys)
			{
				indexedDatesReversed[counter] = date;
				indexedDates[date] = counter++;
			}
		}

		/// <summary>
		/// Simple easy accessor to loop over all dates.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DateTime> Dates()
		{
			foreach (DateTime key in data.Keys)
			{
				yield return key;
			}
		}
		
		public bool TryGetValue(DateTime date, string item, out double value)
		{
			if (data.TryGetValue(date, out var tempObject) && tempObject.TryGetValue(item, out value))
			{
				return true;
			}
			throw new Exception("The requested data is not available for the given date.");
		}

		public bool GetHistoricalDate(DateTime date, int histo, out DateTime previousDate)
		{
			int index = indexedDates[date];
			if (index >= histo)
			{
				//Reverse dico look up is faster at using elementAt() on the original sorted dictionary
				//Having a direct array look-up would be faster still but for demonstration purposes this should sufficient.
				previousDate = indexedDatesReversed[index - histo];
				return true;
			}
			throw new Exception("The requested historical date is not available.");
		}
	}
}
