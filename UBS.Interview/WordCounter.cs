using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UBS.Interview
{
	public static class WordCounter
	{
		private static bool ContainsWordCharacter(string word)
		{
			if (word == null)
				throw new ApplicationException("You asked if a null string is a word!");
			
			var containsWordCharacter = new Regex(@"\w");
			return containsWordCharacter.IsMatch(word);
		}

		private static string ConvertAllWhitespacesToSingleSpaceCharacter(string sentence) 
		{
			if (sentence == null)
				throw new ApplicationException("You asked to sanitize whitespace in a null string!");
			
			var allSeparatorCharacters = new Regex(@"\s+");
			return allSeparatorCharacters.Replace(sentence, " ");
		}

		private static string RemoveAllPunctuationApartFromHyphens(string word)
		{
			if (word == null)
				throw new ApplicationException("You asked to remove punctuation from a null string!");

			var removePunctuationApartFromHyphens = new Regex(@"[^\w-]");
			return removePunctuationApartFromHyphens.Replace(word, string.Empty);
		}

		private static string RemoveHyphensThatAreNotInsideWords(string word)
		{
			if (word == null)
				throw new ApplicationException("You asked to remove hyphens from a null string!");

			var removeNonInnerWordHyphens = new Regex(@"^(-(?=\w))|(-$)");
			return removeNonInnerWordHyphens.Replace(word, string.Empty);
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

			return ConvertAllWhitespacesToSingleSpaceCharacter(sentence)
				.Split(new Char [] {' '}, StringSplitOptions.RemoveEmptyEntries)
				.GroupBy(word => RemoveHyphensThatAreNotInsideWords(RemoveAllPunctuationApartFromHyphens(word.ToLowerInvariant())))
				.Where(group => ContainsWordCharacter(group.Key))
				.Select(group => new WordCount { Word = group.Key, Count = group.Count() });
		}
	}
}

