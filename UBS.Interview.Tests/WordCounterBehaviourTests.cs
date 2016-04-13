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
		public void EachWordIsReportedOnlyOnce()
		{
			var repeatedWords = String.Join(" ", Enumerable.Repeat("word some other", 10));
			var wordCounts = WordCounter.CountWords(repeatedWords);
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "word"), "Repeated word was returned more than once.");
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "some"), "Repeated word was returned more than once.");
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "other"), "Repeated word was returned more than once.");
		}

		[Test]
		public void TrailingPunctuationCharactersAreNotPartOfWords()
		{
			var wordCounts = WordCounter.CountWords("some.,. !strange%$?! :;sentence- *what.?0");
			var expectedOutput = new HashSet<WordCount>() {
				new WordCount { Word = "some", Count = 1 },
				new WordCount { Word = "!strange%$", Count = 1 },
				new WordCount { Word = ":;sentence", Count = 1 },
				new WordCount { Word = "*what.?0", Count = 1 }
			};
			CollectionAssert.AreEquivalent(expectedOutput, wordCounts, 
				"Not all trailing punctuation characters have been removed.");
		}

		[Test]
		public void LeadingPunctuationCharactersArePartOfWords()
		{
			var wordCounts = WordCounter.CountWords("We all love .Net and #tags!");
			var expectedOutput = new HashSet<WordCount>() {
				new WordCount { Word = "we", Count = 1 },
				new WordCount { Word = "all", Count = 1 },
				new WordCount { Word = "love", Count = 1 },
				new WordCount { Word = ".net", Count = 1 },
				new WordCount { Word = "and", Count = 1 },
				new WordCount { Word = "#tags", Count = 1 }
			};
			CollectionAssert.AreEquivalent(expectedOutput, wordCounts, 
				"Not all leading punctuation characters have been persisted.");

		}

		[Test]
		public void WordsAreDelimitedByWhitespaces()
		{
			var sentence = "Some sentence\twith\nvarious separators.";
			var wordCounts = WordCounter.CountWords(sentence);
			var numberOfSeparators = sentence.ToCharArray().Where(Char.IsWhiteSpace).Count();
			Assert.AreEqual(numberOfSeparators + 1, wordCounts.Count(), 
				"The number of words is not equal to the number of separators + 1");
		}

		[Test]
		public void WordsCanBeDelimitedByMultipleWhitespaces()
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
		public void NonWordCharactersDoNotMakeWords()
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
		public void EveryWordIsCounted()
		{
			var sentence = "There are five words here.";
			var wordCounts = WordCounter.CountWords(sentence);
			var numOfWordsCounted = wordCounts.Select(wc => wc.Count).Sum();
			Assert.AreEqual(5, numOfWordsCounted, "The number of words counted is incorrect.");
		}

		[Test]
		public void InnerWordPunctuationIsPartOfWords() 
		{
			var sentence = "Some proper nouns, like This?IsMade!Up, may be a company name or a company-like entity.";
			var wordCounts = WordCounter.CountWords(sentence);
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "this?ismade!up"), 
				"The word with non-word characters inside was not returned.");
			Assert.DoesNotThrow(() => wordCounts.Single(wc => wc.Word == "company-like"), 
				"The word with a hyphen characters inside was not returned.");
		}

		[Test]
		public void NumbersAreWords()
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
    }
}
