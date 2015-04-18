using System;

namespace BaelorApi.Models.Database
{
	public class Lyric
		: Audit
	{
		public string Content { get; set; }

		public int TimeCode { get; set; }

		#region [ Song ]

		public Nullable<Guid> SongId { get; set; }

		public Song Song { get; set; }

		#endregion
	}
}