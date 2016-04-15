using System;
using System.Collections.Generic;

namespace UBS.Interview
{
	public class WordCounterBuilder : IWordCounterBuilderWithSplitter
	{
		/// <summary>
		/// The UBS word counter - solution to the exercise.
		/// </summary>
		public static WordCounter UBSWordCounter()
		{
			return new WordCounterBuilder()
				.WithSentenceSanitizer(WordCounterTransformations.ConvertAllWhitespacesToSingleSpaceCharacter)
				.WithWordSanitizer(WordCounterTransformations.ConvertToLowerCase)
				.WithWordSanitizer(WordCounterTransformations.RemoveAllPunctuationApartFromHyphens)
				.WithWordSanitizer(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords)
				.WithWordFilter(WordCounterTransformations.ContainsWordCharacter)
				.WithWordFilter(WordCounterTransformations.IsNotEmptyOrWhitespace)
				.WithSentenceSplitter(WordCounterTransformations.SplitSentenceOnSingleSpaceCharacter)
				.Build();
		}

		private List<SentenceSanitizer> sentenceSanitizers = new List<SentenceSanitizer>();
		private List<WordSanitizer> wordSanitizers = new List<WordSanitizer>();
		private List<WordFilter> wordFilters = new List<WordFilter>();
		private SentenceSplitter sentenceSplitter = sentence => sentence.Split(' ');

		public WordCounterBuilder WithSentenceSanitizer(SentenceSanitizer sentenceSanitizer)
		{
			this.sentenceSanitizers.Add(sentenceSanitizer);
			return this;
		}

		public WordCounterBuilder WithWordSanitizer(WordSanitizer wordSanitizer)
		{
			this.wordSanitizers.Add(wordSanitizer);
			return this;
		}

		public WordCounterBuilder WithWordFilter(WordFilter wordFilter)
		{
			this.wordFilters.Add(wordFilter);
			return this;
		}

		public IWordCounterBuilderWithSplitter WithSentenceSplitter(SentenceSplitter sentenceSplitter)
		{
			this.sentenceSplitter = sentenceSplitter;
			return this;
		}

		WordCounter IWordCounterBuilderWithSplitter.Build()
		{
			// Combine all sanitizers and filters into one.
			// Applied in the order in which they were registered.

			SentenceSanitizer sentenceSanitizer = sentence => {
				foreach (var sanitizer in sentenceSanitizers)
					sentence = sanitizer(sentence);
				return sentence; 
			};
			WordSanitizer wordSanitizer = word => {
				foreach (var sanitizer in wordSanitizers)
					word = sanitizer(word);
				return word;
			};
			WordFilter wordFilter = word => {
				var isOK = true;
				foreach (var filter in wordFilters)
					isOK &= filter(word);
				return isOK;
			};

			return new WordCounter(sentenceSanitizer, wordSanitizer, wordFilter, sentenceSplitter);
		}
	}

	// Small interface to provide nicer API to the builder.
	// You will only be able to get a WordCounter if you've provided a splitter.
	public interface IWordCounterBuilderWithSplitter
	{

		/// <summary>
		/// Constructs an instance of WordCounter that includes all given
		/// sanitizers, filters and a splitter.
		/// </summary>
		WordCounter Build();
	}
}

