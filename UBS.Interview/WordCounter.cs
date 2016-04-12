using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UBS.Interview
{
	public static class WordCounter
	{
		public static IEnumerable<WordCount> CountWords(string sentence)
		{
			return sentence
				.Split(new Char [] {' '}, StringSplitOptions.RemoveEmptyEntries)
				.GroupBy(word => word.ToLowerInvariant())
				.Select(group => new WordCount { Word = group.Key, Count = group.Count() });
		}
	}
}

