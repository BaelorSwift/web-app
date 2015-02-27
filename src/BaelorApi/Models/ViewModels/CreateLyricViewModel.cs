using Newtonsoft.Json;

namespace BaelorApi.Models.ViewModels
{
	public class CreateLyricViewModel
	{
		[JsonProperty("song_slug")]
		public string SongSlug { get; set; }

		[JsonProperty("lyrics")]
		public string Lyrics { get; set; }
	}
}
