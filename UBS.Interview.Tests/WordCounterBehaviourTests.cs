using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using UBS.Interview;

namespace UBS.Interview.Tests
{
	[TestFixture]
    public class WordCounterBehaviourTests
    {
		[Test]
		public void WordCountIsCaseInsensitive()
		{
			var wordCounts = WordCounter.CountWords("This Is a Title Case Sentence");
			var expectedOutput = new HashSet<WordCount>() {
				new WordCount { Word = "this", Count = 1 },
				new WordCount { Word = "is", Count = 1 },
				new WordCount { Word = "a", Count = 1 },
				new WordCount { Word = "title", Count = 1 },
				new WordCount { Word = "case", Count = 1 },
				new WordCount { Word = "sentence", Count = 1 }
			};
			CollectionAssert.AreEquivalent(expectedOutput, wordCounts, "Not all words are equivalent.");
		}

		[Test]
		public void EveryWordIsCounted()
		{
			var sentence = "There are five words here.";
			var wordCounts = WordCounter.CountWords(sentence);
			var numOfWordsCounted = wordCounts.Select(wc => wc.Count).Sum();
			Assert.AreEqual(5, numOfWordsCounted, "The number of words counted is incorrect.");
		}

		[Test]
		public void EachWordIsReportedOnlyOnce()
		{
			var repeatedWords = String.Join(" ", Enumerable.Repeat("word some other", 10));
			var wordCounts = WordCounter.CountWords(repeatedWords);
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "word"), "Repeated word was returned more than once.");
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "some"), "Repeated word was returned more than once.");
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "other"), "Repeated word was returned more than once.");
		}

		[Test]
		public void SentencesGetSplitOnWhitespaces()
		{
			var sentence = "Some sentence\twith\nvarious separators.";
			var wordCounts = WordCounter.CountWords(sentence);
			var numberOfSeparators = sentence.ToCharArray().Where(Char.IsWhiteSpace).Count();
			Assert.AreEqual(numberOfSeparators + 1, wordCounts.Count(), 
				"The number of words is not equal to the number of separators + 1");
		}

		[Test]
		public void SentencesGetSplitByMultipleWhitespaces()
		{
			var sentence = "Is this  a    sentence\t\t\twith\n\n\nvarious  . ,   separators?";
			var wordCounts = WordCounter.CountWords(sentence);
			var expectedOutput = new HashSet<WordCount>() {
				new WordCount { Word = "is", Count = 1 },
				new WordCount { Word = "this", Count = 1 },
				new WordCount { Word = "a", Count = 1 },
				new WordCount { Word = "sentence", Count = 1 },
				new WordCount { Word = "with", Count = 1 },
				new WordCount { Word = "various", Count = 1 },
				new WordCount { Word = "separators", Count = 1 }
			};
			CollectionAssert.AreEquivalent(expectedOutput, wordCounts, 
				"Some multi-separators have been considered to be words.");
		}

		[Test]
		public void NonWordCharactersOnlyDoNotMakeWords()
		{
			var wordCounts = WordCounter.CountWords("This ~£!@#}{ is not a word !@#$%%^&^*?:>");
			var expectedOutput = new HashSet<WordCount>() {
				new WordCount { Word = "this", Count = 1 },
				new WordCount { Word = "is", Count = 1 },
				new WordCount { Word = "not", Count = 1 },
				new WordCount { Word = "a", Count = 1 },
				new WordCount { Word = "word", Count = 1 },
			};
			CollectionAssert.AreEquivalent(expectedOutput, wordCounts, 
				"Some non-words of only non-word characters have been considered to be words.");
		}

		[Test]
		public void NumbersAreCountedAsWords()
		{
			var sentence = "Number 10 is also a word and so is this10.";
			var wordCounts = WordCounter.CountWords(sentence);
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "10"), 
				"The number 10 was not returned.");
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "this10"), 
				"The word this10 was not returned.");
		}

		[Test]
		public void BracesAndQuotesAreRemovedFromSentence()
		{
			var sentence = "\"Quotes (or braces) are not part of words.\"";
			var wordCounts = WordCounter.CountWords(sentence);
			var expectedOutput = new HashSet<WordCount>() {
				new WordCount { Word = "quotes", Count = 1 },
				new WordCount { Word = "or", Count = 1 },
				new WordCount { Word = "braces", Count = 1 },
				new WordCount { Word = "are", Count = 1 },
				new WordCount { Word = "not", Count = 1 },
				new WordCount { Word = "part", Count = 1 },
				new WordCount { Word = "of", Count = 1 },
				new WordCount { Word = "words", Count = 1 }
			};
			CollectionAssert.AreEquivalent(expectedOutput, wordCounts, 
				"Some quotes or braces have been included in the output.");
		}

		[Test]
		public void WhenCountingWordsAllPunctuationIsIgnoredApartFromHyphens()
		{
			var sentence = "All punctuation, apart from inner-word hyphens, is ignored!";
			var wordCounts = WordCounter.CountWords(sentence);
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "inner-word"), 
				"The word with a hyphen characters inside was not returned.");
			Assert.False(wordCounts.Where(wordCount => wordCount.Word != "inner-word")
					.Any(wordCount => wordCount.Word.ToCharArray().Where(Char.IsPunctuation).Count() > 0),
				"Some words were returned that contain punctuation.");
		}
    }
}
