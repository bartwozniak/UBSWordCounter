using System;
using System.Collections.Generic;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

using UBS.Interview;

namespace UBS.Interview.Spec
{
	[Binding]
	public class TestSpec
	{
		[Given ("a sentence (.*)")]
		public void GivenASentence(string sentence)
		{
			ScenarioContext.Current["sentence"] = sentence;
		}

		[When ("the program is run")]
		public void WhenTheProgramIsRun()
		{
			var wordCounter = WordCounterBuilder.UBSWordCounter();
			
			var result = wordCounter.CountWords((string) ScenarioContext.Current["sentence"]);
			ScenarioContext.Current.Set<IEnumerable<WordCount>>(result);
		}

		[Then ("the result should be")]
		public void ThenTheResultShouldBe(Table result)
		{
			var wordCounts = ScenarioContext.Current.Get<IEnumerable<WordCount>>();
			result.CompareToSet<WordCount>(wordCounts);
		}
	}
}
        
