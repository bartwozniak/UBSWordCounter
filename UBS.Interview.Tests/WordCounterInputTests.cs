using NUnit.Framework;
using System;

namespace UBS.Interview.Tests
{
	[TestFixture]
	public class WordCounterInputTests
	{
		[Test]
		public void EmptyStringIsValid()
		{
			var wordCounts = WordCounter.CountWords(string.Empty);
			CollectionAssert.IsEmpty(wordCounts);
		}

		[Test]
		public void ExceptionIsThrownOnNullInput()
		{
			Assert.Throws<ApplicationException>(() => WordCounter.CountWords(null), 
				"ApplicationException should be thrown when input is null.");
		}
	}
}

