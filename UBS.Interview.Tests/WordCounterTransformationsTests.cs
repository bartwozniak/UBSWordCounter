using System;
using System.Linq;
using NUnit.Framework;

namespace UBS.Interview.Tests
{
	[TestFixture]
	public class WordCounterTransformationsTests
	{
		[Test]
		public void HyphensNotInsideWordsAreRemovedCorrectly()
		{
			Assert.False(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords("Huh-")
				.ToCharArray().Any(c => c == '-'),	"Trailing hyphen was not removed!");

			Assert.False(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords("-100")
				.ToCharArray().Any(c => c == '-'),	"Leading hyphen was not removed!");

			Assert.False(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords("---")
				.ToCharArray().Any(c => c == '-'),	"Multiple hyphens were not removed!");
			
			Assert.False(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords("---Hello")
				.ToCharArray().Any(c => c == '-'),	"Multiple leading hyphens were not removed!");

			Assert.False(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords("hello---")
				.ToCharArray().Any(c => c == '-'),	"Multiple trailing hyphens were not removed!");

			Assert.False(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords("---hello---")
				.ToCharArray().Any(c => c == '-'),	"Multiple hyphens were not removed from both sides!");
		}

		[Test]
		public void HyphensInsideWordsAreNotRemoved()
		{
			Assert.True(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords("some-word")
				.Contains('-'), "Middle hyphen was removed!");

			Assert.True(WordCounterTransformations.RemoveHyphensThatAreNotInsideWords("some---word")
				.Contains("---"), "Middle multiple hyphens were removed!");
		}
	}
}

