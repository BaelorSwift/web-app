using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BaelorApi.Models.ViewModels
{
	public class PatchSongViewModel
	{
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("producers")]
		public string[] Producers { get; set; }

		[JsonProperty("writers")]
		public string[] Writers { get; set; }

		[JsonProperty("length_in_seconds")]
		public Nullable<int> LengthSeconds { get; set; }

		[JsonProperty("index")]
		public Nullable<int> Index { get; set; }

		[JsonProperty("lyrics")]
		public IEnumerable<PatchLyricViewModel> Lyrics { get; set; }
	}
}
