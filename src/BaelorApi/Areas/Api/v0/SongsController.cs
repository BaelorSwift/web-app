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

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[ExceptionHandler]
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
		/// Creates a new instance of the Songs Controller.
		/// </summary>
		/// <param name="albumRepository">The repository of <see cref="Album"/> data.</param>
		public SongsController(IAlbumRepository albumRepository, ISongRepository songRepository)
		{
			_albumRepository = albumRepository;
			_songRepository = songRepository;
		}

		/// <summary>
		///		[GET] api/v0/songs
		/// Gets all of Bae's songs.
		/// </summary>
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
		/// <param name="id">The slug of the song.</param>
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			var song = _songRepository.GetBySlug(id);

			if (song != null)
				return Content(HttpStatusCode.OK, new ResponseBase { Result = Models.Api.Response.Partials.Song.Create(song, true) });

			return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidSongSlug), Success = false });
		}

		/// <summary>
		///		[POST] api/v0/songs/
		/// </summary>
		/// <param name="viewModel"></param>
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
	}
}
