using Microsoft.AspNet.Mvc;
using System.Net;
using System.Web.Http;
using BaelorApi.Models.Api;
using BaelorApi.Models.Repositories;
using BaelorApi.Attributes;
using BaelorApi.Models.ViewModels;
using BaelorApi.Models.Api.Error;
using BaelorApi.Models.Error.Enums;
using System;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[ExceptionHandler]
	[SetResponseHeaders]
	[RequireAuthentication]
	[Route("api/v0/[controller]")]
	public class LyricsController : ApiController
	{
		/// <summary>
		/// Repository for interacting with <see cref="Song"/> data
		/// </summary>
		private readonly ISongRepository _songRepository;

		/// <summary>
		/// Repository for interacting with <see cref="Lyric"/> data
		/// </summary>
		private readonly ILyricRepository _lyricRepository;

		/// <summary>
		/// Creates a new instance of the Lyrics Controller.
		/// </summary>
		/// <param name="albumRepository">The repository of <see cref="Album"/> data.</param>
		/// <param name="lyricRepository">The repository of <see cref="Lyric"/> data.</param>
		public LyricsController(ISongRepository songRepository, ILyricRepository lyricRepository)
		{
			_songRepository = songRepository;
			_lyricRepository = lyricRepository;
		}

		/// <summary>
		///		[GET] api/v0/lyrics/{song-slug}
		/// Gets all of Bae's Lyrics.
		/// </summary>
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			var lyric = _lyricRepository.GetBySlug(id);
			if (lyric == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.SongDoesntContainLyrics), Success = false });

			return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Lyric.Create(lyric, true) });
		}

		/// <summary>
		///		[POST] api/v0/lyrics/
		/// Add's the lyrics to a song based on the POST'ed view model.
		/// </summary>
		/// <param name="viewModel">The data of the lyrics.</param>
		[HttpPost]
		[RequireAdminAuthentication]
		public IActionResult Post([FromBody] CreateLyricViewModel viewModel)
		{
			var song = _songRepository.GetBySlug(viewModel.SongSlug);
			
			if (song == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidSongSlug), Success = false });

			var lyric = new Models.Database.Lyric
			{
				Lyrics = viewModel.Lyrics,
				Slug = song.Slug,
				Song = song,
				SongId = song.Id
			};
			lyric = _lyricRepository.Add(lyric);
			
			if (lyric != null)
				return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Lyric.Create(lyric, true) });

			throw new NotImplementedException("TODO: add error when failing to create lyric.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		[HttpPatch("{id}")]
		[RequireAdminAuthentication]
		public IActionResult Patch(string id, [FromBody] PatchLyricViewModel viewModel)
		{
			var lyric = _lyricRepository.GetBySlug(id);
			if (lyric == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.SongDoesntContainLyrics), Success = false });

			lyric.Lyrics = viewModel.Lyrics;
			lyric = _lyricRepository.Update(lyric.Id, lyric);
			
			return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Lyric.Create(_lyricRepository.GetById(lyric.Id), true) });
		}
	}
}
