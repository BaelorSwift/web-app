using Newtonsoft.Json;

namespace BaelorApi.Models.Api.Response.Partials
{
	public class Lyric
	{
		[JsonProperty("lyrics")]
		public string Lyrics { get; set; }

		[JsonProperty("song", NullValueHandling = NullValueHandling.Ignore)]
		public Song Song { get; set; }

		public static Lyric Create(Database.Lyric lyric, bool includeSong)
		{
			return new Lyric
			{
				Lyrics = lyric.Lyrics,
				Song = includeSong ? Song.Create(lyric.Song, true) : null
			};
		}
	}
}