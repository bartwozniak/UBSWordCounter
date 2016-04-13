using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBS.Interview
{
    class Program
    {
        static void Main(string[] args)
        {
			var input = "Some sentence\twith\nvarious separators.";
			var output = WordCounter.CountWords(input);
			foreach (var wc in output)
			{
				Console.WriteLine(wc);
			}
        }
    }
}
