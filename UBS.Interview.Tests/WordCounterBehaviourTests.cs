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
		public void PunctuationIsRemovedFromWords()
		{
			var wordCounts = WordCounter.CountWords("A sentence in this test has punctuation, which needs to be removed!");
			CollectionAssert.IsNotEmpty(wordCounts);
			Assert.False(
				wordCounts.SelectMany(
					wc => wc.Word.ToCharArray()).Any(
						c => Char.IsPunctuation(c)), "All punctuation has not been removed.");
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
		public void TrailingAndLeadingNonWordCharactersAreNotPartOfWords()
		{
			var wordCounts = WordCounter.CountWords("/some!@ !'strange%$ :;sentence-");
			var expectedOutput = new HashSet<WordCount>() {
				new WordCount { Word = "some", Count = 1 },
				new WordCount { Word = "strange", Count = 1 },
				new WordCount { Word = "sentence", Count = 1 }
			};
			CollectionAssert.AreEquivalent(expectedOutput, wordCounts, 
				"Not all leading or trailing non-word characters have been removed.");
		}

		[Test]
		public void WordsAreDelimitedBySeparators()
		{
			var sentence = "Some sentence\twith\nvarious separators.";
			var wordCounts = WordCounter.CountWords(sentence);
			var numberOfSeparators = sentence.ToCharArray().Where(Char.IsSeparator).Count();
			Assert.AreSame(numberOfSeparators + 1, wordCounts.Count(), 
				"The number of words is not equal to the number of separators + 1");
		}

		[Test]
		public void WordsCanBeDelimitedByMultipleSeparators()
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
			Assert.Fail();
		}

		[Test]
		public void InnerWordPunctuationIsTreatedAsPartOfWord() 
		{
			Assert.Fail();
		}

		[Test]
		public void NumbersAreWords()
		{
			Assert.Fail();
		}
    }
}
