using Newtonsoft.Json;

namespace BaelorApi.Models.ViewModels
{
	public class PatchLyricViewModel
	{
		[JsonProperty("lyrics")]
		public string Lyrics { get; set; }
	}
}
