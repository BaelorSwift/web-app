using System;
using System.Collections.Generic;

namespace BaelorApi.Models.Database
{
	public class Album
		: Audit
	{
		public string Slug { get; set; }

		public string Name { get; set; }

		public DateTime ReleasedAt { get; set; }

		public int LengthSeconds { get; set; }

		public string Label { get; set; }

		public string Genres { get; set; }

		public string Producers { get; set; }

		public ICollection<Song> Songs { get; set; }
	}
}
