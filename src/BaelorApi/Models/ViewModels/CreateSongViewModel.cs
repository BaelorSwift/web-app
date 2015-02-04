using Newtonsoft.Json;

namespace BaelorApi.Models.ViewModels
{
	public class CreateSongViewModel
	{
		[JsonProperty("album_slug")]
		public string AlbumSlug { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("producers")]
		public string[] Producers { get; set; }

		[JsonProperty("writers")]
		public string[] Writers { get; set; }

		[JsonProperty("length_in_seconds")]
		public int LengthSeconds { get; set; }

		[JsonProperty("index")]
		public int Index { get; set; }
	}
}
