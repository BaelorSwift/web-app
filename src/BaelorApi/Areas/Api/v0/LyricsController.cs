using Microsoft.AspNet.Mvc;
using System.Web.Http;
using BaelorApi.Models.Repositories;
using BaelorApi.Attributes;
using System.Net;
using BaelorApi.Models.Api;
using BaelorApi.Models.Error.Enums;
using BaelorApi.Models.Api.Error;
using BaelorApi.Models.ViewModels;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[ExceptionHandler]
	[SetResponseHeaders]
	[RequireAuthentication]
	[Route("api/v0/songs/{slug}/lyrics")]
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
		///		[GET] api/v0/songs/{slug}/lyrics
		/// Gets the lyrics of a song by Bae with the specified slug.
		/// </summary>
		/// <param name="slug">The slug of the song.</param>
		[HttpGet]
		public IActionResult GetLyrics(string slug)
		{
			var song = _songRepository.GetBySlug(slug);
			if (song == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidSongSlug), Success = false });

			var lyric = song.Lyric;
			if (lyric == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.SongDoesntContainLyrics), Success = false });

			return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Lyric.Create(lyric, true) });
		}

		/// <summary>
		///		[POST] api/v0/songs/{slug}/lyrics
		/// Add's the lyrics to a song based on the POST'ed view model.
		/// </summary>
		/// <param name="slug">The slug of the song.</param>
		/// <param name="viewModel">The view model containg the data of the lyrics.</param>
		[RequireAdminAuthentication]
		[HttpPost]
		public IActionResult Post(string slug, [FromBody] CreateLyricViewModel viewModel)
		{
			var song = _songRepository.GetBySlug(slug);
			if (song == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidSongSlug), Success = false });
			if (song.Lyric != null)
				return Content(HttpStatusCode.Conflict, new ResponseBase { Error = new ErrorBase(ErrorStatus.SongAlreadyContainsLyrics), Success = false });

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

			return Content(HttpStatusCode.BadRequest, new ResponseBase { Error = new ErrorBase(ErrorStatus.UnableToCreateLyrics), Success = false });
		}

		/// <summary>
		///		[PATCH] api/v0/songs/{slug}/lyrics
		/// Updates the lyrics to a song based on the PATCH'ed view model.
		/// </summary>
		/// <param name="slug">The slug of the song.</param>
		/// <param name="viewModel">The view model containg the data of the updated lyrics.</param>
		[HttpPatch]
		[RequireAdminAuthentication]
		public IActionResult Post(string slug, [FromBody] PatchLyricViewModel viewModel)
		{
			var song = _songRepository.GetBySlug(slug);
			if (song == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidSongSlug), Success = false });
			if (song.Lyric == null)
				return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.SongDoesntContainLyrics), Success = false });

			song.Lyric.Lyrics = viewModel.Lyrics;
			var lyric = _lyricRepository.Update(song.Lyric.Id, song.Lyric);
			return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Lyric.Create(lyric, true) });
		}
	}
}
