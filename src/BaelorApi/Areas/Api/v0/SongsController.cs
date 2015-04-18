using Microsoft.AspNet.Mvc;
using System.Net;
using System.Web.Http;
using BaelorApi.Models.Api;
using BaelorApi.Models.Repositories;
using System.Collections.Generic;
using BaelorApi.Models.Api.Error;
using BaelorApi.Models.Error.Enums;
using System;
using BaelorApi.Extentions;
using BaelorApi.Models.ViewModels;
using BaelorApi.Attributes;
using System.Linq;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[ExceptionHandler]
	[SetResponseHeaders]
	[Route("api/v0/[controller]")]
	public class SongsController : ApiController
	{
		/// <summary>
		/// Repository for interacting with <see cref="Album"/> data
		/// </summary>
		private readonly IAlbumRepository _albumRepository;

		/// <summary>
		/// Repository for interacting with <see cref="Song"/> data
		/// </summary>
		private readonly ISongRepository _songRepository;

		/// <summary>
		/// Repository for interacting with <see cref="Lyric"/> data
		/// </summary>
		private readonly ILyricRepository _lyricRepository;

		/// <summary>
		/// Creates a new instance of the Songs Controller.
		/// </summary>
		/// <param name="albumRepository">The repository of <see cref="Album"/> data.</param>
		/// <param name="songRepository">The repository of <see cref="Song"/> data.</param>
		/// <param name="lyricRepository">The repository of <see cref="Lyric"/> data.</param>
		public SongsController(IAlbumRepository albumRepository, 
			ISongRepository songRepository, ILyricRepository lyricRepository)
		{
			_albumRepository = albumRepository;
			_songRepository = songRepository;
			_lyricRepository = lyricRepository;
		}
		
		/// <summary>
		///		[GET] api/v0/songs
		/// Gets all of Bae's songs.
		/// </summary>
		[RequireAuthentication]
		[HttpGet]
		public IActionResult Get()
		{
			var songs = new List<Models.Api.Response.Partials.Song>();
			foreach (var song in _songRepository.GetAll)
				songs.Add(Models.Api.Response.Partials.Song.Create(song, true));

			return Content(HttpStatusCode.OK, new ResponseBase { Result = songs });
		}

		/// <summary>
		///		[GET] api/v0/songs/{slug}
		/// Gets the song by Bae with the specified slug.
		/// </summary>
		/// <param name="slug">The slug of the song.</param>
		[RequireAuthentication(true)]
		[HttpGet("{slug}")]
		public IActionResult Get(string slug)
		{
			var song = _songRepository.GetBySlug(slug);

			if (song != null)
				return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Song.Create(song, true) });

			return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidSongSlug), Success = false });
		}

		/// <summary>
		///		[POST] api/v0/songs/
		/// Adds a song by bae based on the POST'ed view model.
		/// </summary>
		/// <param name="viewModel">The data of the song to create.</param>
		[RequireAuthentication]
		[RequireAdminAuthentication]
		[HttpPost]
		public IActionResult Post([FromBody] CreateSongViewModel viewModel)
		{
			var album = _albumRepository.GetBySlug(viewModel.AlbumSlug);

			if (album == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidAlbumSlug), Success = false });

			var song = new Models.Database.Song
			{
				Album = album,
				Index = viewModel.Index,
				Title = viewModel.Title,
				Slug = viewModel.Title.ToSlug(),
				LengthSeconds = viewModel.LengthSeconds,
				Producers = string.Join(",", viewModel.Producers),
				Writers = string.Join(",", viewModel.Writers)
			};
			song = _songRepository.Add(song);

			if (song != null)
				return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Song.Create(song, true) });

			throw new NotImplementedException("TODO: add error when failing to create song.");
		}

		/// <summary>
		///		[PATCH] api/v0/songs/{slug}
		/// Updates the a song with the data in the PATCH'ed view model.
		/// </summary>
		/// <param name="slug">The slug of the song.</param>
		/// <param name="viewModel">The view model containg the data of the updated aong.</param>
		[RequireAuthentication]
		[RequireAdminAuthentication]
		[HttpPatch("{slug}")]
		public IActionResult Patch(string slug, [FromBody] PatchSongViewModel viewModel)
		{
			var song = _songRepository.GetBySlug(slug);
			if (song == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidSongSlug), Success = false });

			if (viewModel.Index != null) song.Index = (int) viewModel.Index;
			if (viewModel.LengthSeconds != null) song.LengthSeconds = (int)viewModel.LengthSeconds;
			if (viewModel.Producers != null) song.Producers = string.Join(",", viewModel.Producers);
			if (viewModel.Writers != null) song.Writers = string.Join(",", viewModel.Writers);
			if (viewModel.Title != null) song.Title = viewModel.Title;
			if (viewModel.Title != null) song.Slug = viewModel.Title.ToSlug();
			if (viewModel.Lyrics != null || viewModel.Lyrics.Any())
			{
				// delete old lyrics
				var lyrics = _lyricRepository.GetAll;
				foreach (var lyric in lyrics)
					_lyricRepository.TryDelete(lyric.Id);

				// NEW LYRICS SON
				foreach (var newLyric in viewModel.Lyrics)
				{
					_lyricRepository.Add(new Models.Database.Lyric
					{
						Content = newLyric.Content,
						TimeCode = newLyric.TimeCode,
						SongId = song.Id
					});
				}
			}

			song = _songRepository.Update(song.Id, song);
			return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Song.Create(song, true) });
		}
	}
}
