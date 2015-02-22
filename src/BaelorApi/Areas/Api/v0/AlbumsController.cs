using Microsoft.AspNet.Mvc;
using System.Net;
using System.Web.Http;
using BaelorApi.Models.Api;
using BaelorApi.Models.Repositories;
using System.Collections.Generic;
using BaelorApi.Models.Api.Response.Partials;
using BaelorApi.Models.Api.Error;
using BaelorApi.Models.Error.Enums;
using BaelorApi.Attributes;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[RequireAuthentication]
	[ExceptionHandler]
	[Route("api/v0/[controller]")]
	public class AlbumsController : ApiController
	{
		/// <summary>
		/// Repository for interacting with <see cref="Album"/> data
		/// </summary>
		private readonly IAlbumRepository _albumRepository;

		/// <summary>
		/// Creates a new instance of the Albums Controller.
		/// </summary>
		/// <param name="albumRepository">The repository of <see cref="Album"/> data.</param>
		public AlbumsController(IAlbumRepository albumRepository)
		{
			_albumRepository = albumRepository;
		}

		/// <summary>
		///		[GET] api/v0/albums
		/// Gets all of Bae's albums.
		/// </summary>
		[HttpGet]
		public IActionResult Get()
		{
			var albums = new List<Album>();
			foreach (var album in _albumRepository.GetAll)
				albums.Add(Album.Create(album, true));

			return Content(HttpStatusCode.OK, new ResponseBase { Result = albums });
		}

		/// <summary>
		///		[GET] api/v0/albums/{slug}
		/// Gets the album by Bae with the specified slug.
		/// </summary>
		/// <param name="id">The slug of the album.</param>
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			var album = _albumRepository.GetBySlug(id);

			if (album != null)
				return Content(HttpStatusCode.OK, new ResponseBase { Result = Album.Create(album, true) });

			return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidAlbumSlug), Success = false});
		}
	}
}
