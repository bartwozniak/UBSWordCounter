using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UBS.Interview
{
	public static class WordCounter
	{
		private static string RemoveTrailingPunctuation(string word)
		{
			if (word == null)
				throw new ApplicationException ("You asked to remove trailing punctuation from a null string.");

			return new string(word.ToCharArray().Reverse().SkipWhile(Char.IsPunctuation).Reverse().ToArray());
		}

		public static IEnumerable<WordCount> CountWords(string sentence)
		{
			return sentence
				.Split(new Char [] {' '}, StringSplitOptions.RemoveEmptyEntries)
				.GroupBy(word => RemoveTrailingPunctuation(word.ToLowerInvariant()))
				.Select(group => new WordCount { Word = group.Key, Count = group.Count() });
		}
	}
}

