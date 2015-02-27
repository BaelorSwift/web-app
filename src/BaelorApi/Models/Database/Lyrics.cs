using System;

namespace BaelorApi.Models.Database
{
	public class Lyric
		: Audit
	{
		public string Lyrics { get; set; }

		public string Slug { get; set; }

		#region [ Song ]

		public Nullable<Guid> SongId { get; set; }

		public Song Song { get; set; }

		#endregion
	}
}
