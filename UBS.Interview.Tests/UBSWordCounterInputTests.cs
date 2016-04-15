using NUnit.Framework;
using System;

namespace UBS.Interview.Tests
{
	[TestFixture]
	public class UBSWordCounterInputTests
	{
		private WordCounter wordCounter;

		[SetUp]
		public void InstantiateWordCounter()
		{
			wordCounter = WordCounterBuilder.UBSWordCounter();
		}

		[Test]
		public void EmptyStringIsValid()
		{
			var wordCounts = wordCounter.CountWords(string.Empty);
			CollectionAssert.IsEmpty(wordCounts);
		}

		[Test]
		public void ExceptionIsThrownOnNullInput()
		{
			Assert.Throws<ApplicationException>(() => wordCounter.CountWords(null), 
				"ApplicationException should be thrown when input is null.");
		}
	}
}

