using System;
using System.Collections;
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
			ScenarioContext.Current.Pending();
		}

		[When ("the program is run")]
		public void WhenTheProgramIsRun()
		{
			ScenarioContext.Current.Pending();
		}

		[Then ("the result should be")]
		public void ThenTheResultShouldBe (Table result)
		{
			ScenarioContext.Current.Pending();
		}
	}
}
        
