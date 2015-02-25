using Newtonsoft.Json;
using System;
using System.Linq;

namespace BaelorApi.Models.Api.Response.Partials
{
	public class Album
	{
		[JsonProperty("slug")]
		public string Slug { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("released_at")]
		public DateTime ReleasedAt { get; set; }

		[JsonProperty("length")]
		public TimeSpan Length { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("genres")]
		public string[] Genres { get; set; }

		[JsonProperty("producers")]
		public string[] Producers { get; set; }

		[JsonProperty("songs", NullValueHandling = NullValueHandling.Ignore)]
		public Song[] Songs { get; set; }

		[JsonProperty("album_cover")]
		public Image AlbumCover { get; set; }

		public static Album Create(Database.Album album, bool goingDeep)
		{
			return new Album
			{
				Genres = album.Genres.Split(','),
				Producers = album.Producers.Split(','),
				Label = album.Label,
				Length = TimeSpan.FromSeconds(album.LengthSeconds),
				ReleasedAt = album.ReleasedAt,
				Name = album.Name,
				Slug = album.Slug,
				AlbumCover = Image.Create(album.Image),
				Songs = goingDeep
					? album.Songs.Select(s => Song.Create(s, false)).OrderBy(s => s.Index).AsEnumerable().ToArray() 
					: null
			};
		}
	}
}