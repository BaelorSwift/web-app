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
using BaelorApi.Models.ViewModels;
using BaelorApi.Extentions;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[ExceptionHandler]
	[SetResponseHeaders]
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
		[RequireAuthentication]
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
		/// <param name="slug">The slug of the album.</param>
		[RequireAuthentication(true)]
		[HttpGet("{slug}")]
		public IActionResult Get(string slug)
		{
			var album = _albumRepository.GetBySlug(slug);

			if (album != null)
				return Content(HttpStatusCode.OK, new ResponseBase { Result = Album.Create(album, true) });

			return Content(HttpStatusCode.NotFound, new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidAlbumSlug), Success = false });
		}

		/// <summary>
		///		[POST] api/v0/albums/
		/// Adds an album by bae based on the POST'ed view model.
		/// </summary>
		/// <param name="viewModel">The data of the album to create.</param>
		[RequireAuthentication]
		[RequireAdminAuthentication]
		[HttpPost]
		public IActionResult Post([FromBody] CreateAlbumViewModel viewModel)
		{
			var album = new Models.Database.Album
			{
				Genres = string.Join(",", viewModel.Genres),
				Producers = string.Join(",", viewModel.Producers),
				Label = viewModel.Label,
				Name = viewModel.Name,
				Slug = viewModel.Name.ToSlug(),
				LengthSeconds = viewModel.LengthSeconds,
				ImageId = viewModel.ImageId,
				ReleasedAt = viewModel.ReleasedAt
			};
			album = _albumRepository.Add(album);
			return Content(HttpStatusCode.OK, new ResponseBase { Result = Album.Create(album, true) });
		}
	}
}
