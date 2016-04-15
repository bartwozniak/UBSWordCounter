using System;
using System.Linq;
using System.Collections.Generic;

namespace UBS.Interview
{
	public delegate string SentenceSanitizer(string sentence);
	public delegate string WordSanitizer(string word);
	public delegate bool WordFilter(string word);
	public delegate IEnumerable<string> SentenceSplitter(string sentence);

	public class WordCounter
	{
		private SentenceSanitizer sentenceSanitizer;
		private WordSanitizer wordSanitizer;
		private WordFilter wordFilter;
		private SentenceSplitter sentenceSplitter;

		internal WordCounter(SentenceSanitizer sentenceSanitizer, WordSanitizer wordSanitizer, 
			WordFilter wordFilter, SentenceSplitter sentenceSplitter)
		{
			this.sentenceSanitizer = sentenceSanitizer;
			this.wordSanitizer = wordSanitizer;
			this.wordFilter = wordFilter;
			this.sentenceSplitter = sentenceSplitter;
		}

		/// <summary>
		/// Counts the words in a sentence.
		/// </summary>
		/// <returns>Enumeration of word counts.</returns>
		/// <param name="sentence">Sentence to count word occurrences in.</param>
		public IEnumerable<WordCount> CountWords(string sentence)
		{
			if (sentence == null)
				throw new ApplicationException("You asked to count words in a null string!");

			// Sanitize the input
			var sanitizedSentence = this.sentenceSanitizer(sentence);

			// Split the sentence into words
			var words = this.sentenceSplitter(sanitizedSentence);

			// Sanitize each word
			var sanitizedWords = words.Select(word => this.wordSanitizer(word));

			// Filter words
			var wordsToCount = sanitizedWords.Where(word => this.wordFilter(word));

			// Count words
			return wordsToCount.GroupBy(x => x).Select(group => new WordCount { Word = group.Key, Count = group.Count() });
		}
	}
}