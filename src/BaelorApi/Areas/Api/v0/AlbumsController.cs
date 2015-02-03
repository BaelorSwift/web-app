using Microsoft.AspNet.Mvc;
using System.Net;
using System.Web.Http;
using BaelorApi.Models.Api;
using BaelorApi.Models.Repositories;
using BaelorApi.Models.Database;
using System;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[Route("api/v0/[controller]")]
	public class AlbumsController : ApiController
	{
		/// <summary>
		/// Repository for interacting with <see cref="TokenPair"/> data
		/// </summary>
		private readonly IAlbumRepository _albumRepository;

		/// <summary>
		/// Creates a new instance of the Albums Controller.
		/// </summary>
		/// <param name="albumRepository">The repository of <see cref="Album"/> data.</param>
		public AlbumsController(IAlbumRepository albumRepository)
		{
			_albumRepository = albumRepository;

			//_albumRepository.Add(new Album
			//{
			//	Genres = new[]
			//	{
			//		"Country"
			//	},
			//	Producers = new[]
			//	{
			//		"Scott Borchetta",
			//		"Nathan Chapman",
			//		"Robert Ellis Orrall"
			//	},
			//	Label = "Big Machine",
			//	LengthSeconds = 2306,
			//	Name = "Taylor Swift",
			//	ReleasedAt = new DateTime(2006, 10, 24),
			//	Slug = "taylor-swift"
			//});

			//_albumRepository.Add(new Album
			//{
			//	Genres = new[]
			//	{
			//		"Christmas",
			//		"Country",
			//		"Pop"
			//	},
			//	Producers = new[]
			//	{
			//		"Scott Borchetta",
			//		"Nathan Chapman",
			//		"Shelli Hill",
			//		"Sue Patterson"
			//	},
			//	Label = "Big Machine",
			//	LengthSeconds = 1155,
			//	Name = "Sounds of the Season: The Taylor Swift Holiday Collection",
			//	ReleasedAt = new DateTime(2007, 10, 14),
			//	Slug = "sounds-of-the-season"
			//});
		}

		/// <summary>
		///		[GET] api/v0/albums
		/// Gets all of Bae's albums.
		/// </summary>
		[HttpGet]
		public IActionResult Get()
		{
			return Content(HttpStatusCode.OK, new ResponseBase { Result = _albumRepository.GetAll });
		}

		/// <summary>
		///		[GET] api/v0/albums/{slug}
		/// Gets the bae status of a word.
		/// </summary>
		/// <param name="id">The slug of the album.</param>
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			var album = _albumRepository.GetBySlug(id);

			return Content(HttpStatusCode.OK, new ResponseBase { Result = new { bae = false } });
		}
	}
}
