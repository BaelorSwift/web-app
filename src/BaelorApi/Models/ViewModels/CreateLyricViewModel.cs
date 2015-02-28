using Newtonsoft.Json;

namespace BaelorApi.Models.ViewModels
{
	public class CreateLyricViewModel
	{
		[JsonProperty("lyrics")]
		public string Lyrics { get; set; }
	}
}
