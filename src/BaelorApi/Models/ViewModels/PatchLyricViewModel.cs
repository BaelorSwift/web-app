using Newtonsoft.Json;

namespace BaelorApi.Models.ViewModels
{
	public class PatchLyricViewModel
	{
		[JsonProperty("content")]
		public string Content { get; set; }

		[JsonProperty("time_code")]
		public int TimeCode { get; set; }
	}
}
