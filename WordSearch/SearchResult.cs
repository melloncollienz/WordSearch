using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordSearch
{
	internal class SearchResult
	{
		public string WordSearch { get; set; }
		public bool IsFound { get; set; }
		public int StartX { get; set; }
		public int StartY { get; set; }
		public int EndX { get; set; }
		public int EndY { get; set; }

	}
}
