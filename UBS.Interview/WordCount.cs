namespace UBS.Interview
{
	public struct WordCount
	{
		public string Word { get; set; }
		public int Count { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}", Word, Count);
		}
	}
}
