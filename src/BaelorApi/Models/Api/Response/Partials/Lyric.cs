using Newtonsoft.Json;
using System;

namespace BaelorApi.Models.Api.Response.Partials
{
	public class Lyric
	{
		[JsonProperty("content")]
		public string Content { get; set; }

		[JsonProperty("time_code")]
		public TimeSpan TimeCode { get; set; }

		public static Lyric Create(Database.Lyric lyric)
		{
			return new Lyric
			{
				Content = lyric.Content,
				TimeCode = TimeSpan.FromTicks(lyric.TimeCode)
			};
		}
	}
}