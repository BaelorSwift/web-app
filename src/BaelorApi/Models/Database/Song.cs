using System;

namespace BaelorApi.Models.Database
{
	public class Song
		: Audit
	{
		/// <summary>
		/// 
		/// </summary>
		public int Index { get; set; }

		public string Slug { get; set; }

		public string Title { get; set; }

		public int LengthSeconds { get; set; }

		public string Writers { get; set; }

		public string Producers { get; set; }

		#region [ Album ]

		public Guid AlbumId { get; set; }

		public Album Album { get; set; }

		#endregion

		#region [ Lyric ]

		public Nullable<Guid> LyricId { get; set; }

		public Lyric Lyric { get; set; }

		#endregion
	}
}
