using Newtonsoft.Json;
using System;

namespace BaelorApi.Models.ViewModels
{
	public class CreateAlbumViewModel
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("producers")]
		public string[] Producers { get; set; }

		[JsonProperty("genres")]
		public string[] Genres { get; set; }

		[JsonProperty("length_in_seconds")]
		public int LengthSeconds { get; set; }

		[JsonProperty("released_at")]
		public DateTime ReleasedAt { get; set; }

		[JsonProperty("image_id")]
		public Guid ImageId { get; set; }
	}
}
