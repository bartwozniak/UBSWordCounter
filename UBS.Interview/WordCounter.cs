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
				throw new ApplicationException ("You asked to remove trailing punctuation from a null string!");

			var removeTrailingPunctuation = new Regex(@"\p{P}+$");
			return removeTrailingPunctuation.Replace(word, String.Empty);
			//return new string(word.ToCharArray().Reverse().SkipWhile(Char.IsPunctuation).Reverse().ToArray());
		}

		private static bool IsWord(string word)
		{
			if (word == null)
				throw new ApplicationException ("You asked if a null string is a word!");
			
			var containsWordCharacter = new Regex(@"\w");
			return containsWordCharacter.IsMatch(word);
		}

		private static string SanitizeWhitespaces(string sentence) 
		{
			if (sentence == null)
				throw new ApplicationException ("You asked to sanitize whitespace in a null string!");
			
			var allSeparatorCharacters = new Regex(@"\s+");
			return allSeparatorCharacters.Replace(sentence, " ");
		}

		private static string RemoveBracesAndQuotes(string sentence)
		{
			if (sentence == null)
				throw new ApplicationException ("You asked to remove braces and quotes from a null string!");

			var allBracesAndQuotes = new Regex(@"[\p{Ps}\p{Pe}\p{Pi}\p{Pf}""']");
			return allBracesAndQuotes.Replace(sentence, String.Empty);
		}

		/// <summary>
		/// Counts the words in a sentence.
		/// </summary>
		/// <returns>Enumeration of word counts.</returns>
		/// <param name="sentence">Sentence to count word occurrences in.</param>
		public static IEnumerable<WordCount> CountWords(string sentence)
		{
			if (sentence == null)
				throw new ApplicationException ("You asked to count words in a null string!");

			return RemoveBracesAndQuotes(SanitizeWhitespaces(sentence))
				.Split(new Char [] {' '}, StringSplitOptions.RemoveEmptyEntries)
				.GroupBy(word => RemoveTrailingPunctuation(word.ToLowerInvariant()))
				.Where(group => IsWord(group.Key))
				.Select(group => new WordCount { Word = group.Key, Count = group.Count() });
		}
	}
}

