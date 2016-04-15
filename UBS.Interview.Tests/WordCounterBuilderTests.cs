using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace UBS.Interview.Tests
{
	[TestFixture]
	public class WordCounterBuilderTests
	{
		[Test]
		public void WordCounterAppliesAllSanitizersAndFilters()
		{
			// Sentence sanitizers
			var sentenceSanitizer1 = new Mock<SentenceSanitizer>();
			sentenceSanitizer1.Setup(f => f(It.IsAny<string>())).Returns<string>(s => s);
			var sentenceSanitizer2 = new Mock<SentenceSanitizer>();
			sentenceSanitizer2.Setup(f => f(It.IsAny<string>())).Returns<string>(s => s);

			// Word sanitizers
			var wordSanitizer1 = new Mock<WordSanitizer>();
			wordSanitizer1.Setup(f => f(It.IsAny<string>())).Returns<string>(s => s);
			var wordSanitizer2 = new Mock<WordSanitizer>();
			wordSanitizer2.Setup(f => f(It.IsAny<string>())).Returns<string>(s => s);

			// Filters
			var filter1 = new Mock<WordFilter>();
			filter1.Setup(f => f(It.IsAny<string>())).Returns<string>(s => true);
			var filter2 = new Mock<WordFilter>();
			filter2.Setup(f => f(It.IsAny<string>())).Returns<string>(s => true);

			// Splitter is required
			var sentenceSplitter = new Mock<SentenceSplitter>();
			sentenceSplitter.Setup(f => f(It.IsAny<string>())).Returns<string>(s => s.Split(' '));

			var counter = new WordCounterBuilder()
				.WithSentenceSanitizer(sentenceSanitizer1.Object)
				.WithSentenceSanitizer(sentenceSanitizer2.Object)
				.WithWordSanitizer(wordSanitizer1.Object)
				.WithWordSanitizer(wordSanitizer2.Object)
				.WithWordFilter(filter1.Object)
				.WithWordFilter(filter2.Object)
				.WithSentenceSplitter(sentenceSplitter.Object)
				.Build();
			var result = counter.CountWords("Some sentence");

			// This is needed to actually generate the result
			CollectionAssert.IsNotEmpty(result);

			sentenceSanitizer1.Verify(f => f(It.IsAny<string>()), Times.Once);
			sentenceSanitizer2.Verify(f => f(It.IsAny<string>()), Times.Once);
			wordSanitizer1.Verify(f => f(It.IsAny<string>()), Times.AtLeastOnce);
			wordSanitizer2.Verify(f => f(It.IsAny<string>()), Times.AtLeastOnce);
			filter1.Verify(f => f(It.IsAny<string>()), Times.AtLeastOnce);
			filter2.Verify(f => f(It.IsAny<string>()), Times.AtLeastOnce);
		}

		[Test]
		public void WordCounterAppliesSanitizersAndFiltersInOrderOfRegistration()
		{
			// Sentence sanitizers
			var wasCalled = false;
			SentenceSanitizer sentenceSanitizer1 = s => {
				wasCalled = true;
				return s;
			};
			SentenceSanitizer sentenceSanitizer2 = s => {
				Assert.True(wasCalled, 
					"Second sentence sanitizer was not called in the correct order");
				wasCalled = false;
				return s;
			};

			// Word sanitizers
			WordSanitizer wordSanitizer1 = w => {
				wasCalled = true;
				return w;
			};
			WordSanitizer wordSanitizer2 = w => {
				Assert.True(wasCalled, 
					"Second word sanitizer was not called in the correct order");
				wasCalled = false;
				return w;
			};

			// Filters
			WordFilter filter1 = w => {
				wasCalled = true;
				return true;
			};
			WordFilter filter2 = w => {
				Assert.True(wasCalled, 
					"Second word filter was not called in the correct order");
				wasCalled = false;
				return true;
			};

			// Splitter is required
			SentenceSplitter sentenceSplitter = s => s.Split(' ');

			var counter = new WordCounterBuilder()
				.WithSentenceSanitizer(sentenceSanitizer1)
				.WithSentenceSanitizer(sentenceSanitizer2)
				.WithWordSanitizer(wordSanitizer1)
				.WithWordSanitizer(wordSanitizer2)
				.WithWordFilter(filter1)
				.WithWordFilter(filter2)
				.WithSentenceSplitter(sentenceSplitter)
				.Build();
			var result = counter.CountWords("Some sentence");

			// This is needed to actually generate the result
			CollectionAssert.IsNotEmpty(result);
		}
	}
}

