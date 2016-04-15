using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UBS.Interview
{
	public static class WordCounterTransformations
	{
		public static IEnumerable<string> SplitSentenceOnSingleSpaceCharacter(string sentence)
		{
			if (sentence == null)
				throw new ApplicationException("You asked to split a null string!");
			
			return sentence.Split(new Char [] {' '}, StringSplitOptions.RemoveEmptyEntries);
		}

		public static bool ContainsWordCharacter(string word)
		{
			if (word == null)
				throw new ApplicationException("You asked if a null string is a word!");

			var containsWordCharacter = new Regex(@"\w");
			return containsWordCharacter.IsMatch(word);
		}

		public static bool IsNotEmptyOrWhitespace(string word)
		{
			return !string.IsNullOrWhiteSpace(word);
		}

		public static string ConvertAllWhitespacesToSingleSpaceCharacter(string sentence) 
		{
			if (sentence == null)
				throw new ApplicationException("You asked to sanitize whitespace in a null string!");

			var allSeparatorCharacters = new Regex(@"\s+");
			return allSeparatorCharacters.Replace(sentence, " ");
		}

		public static string RemoveAllPunctuationApartFromHyphens(string word)
		{
			if (word == null)
				throw new ApplicationException("You asked to remove punctuation from a null string!");

			var removePunctuationApartFromHyphens = new Regex(@"[^\w-]");
			return removePunctuationApartFromHyphens.Replace(word, string.Empty);
		}

		public static string RemoveHyphensThatAreNotInsideWords(string word)
		{
			if (word == null)
				throw new ApplicationException("You asked to remove hyphens from a null string!");

			var removeNonInnerWordHyphens = new Regex(@"^(-+(?=\w))|(-+$)");
			return removeNonInnerWordHyphens.Replace(word, string.Empty);
		}

		public static string ConvertToLowerCase(string word)
		{
			if (word == null)
				throw new ApplicationException("You cannot convert null to lower case!");
			
			return word.ToLowerInvariant();
		}
	}
}

