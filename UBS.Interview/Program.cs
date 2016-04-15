using System;

namespace UBS.Interview
{
    class Program
    {
        static void Main(string[] args)
        {
			// Get the input
			string inputSentence;
			if (args.Length == 0)
				inputSentence = Console.ReadLine();
			else
				inputSentence = args[0];

			// Get the UBS counter
			var counter = WordCounterBuilder.UBSWordCounter();

			// Count the words and print to console
			var output = counter.CountWords(inputSentence);
			foreach (var wc in output)
				Console.WriteLine(wc);
        }
    }
}
