using Microsoft.AspNet.Mvc;
using System.Net;
using System.Web.Http;
using BaelorApi.Models.Api;
using BaelorApi.Models.Repositories;
using System.Collections.Generic;
using BaelorApi.Models.Api.Response.Partials;
using BaelorApi.Models.Api.Error;
using BaelorApi.Models.Error.Enums;

//using BaelorApi.Models.Database;
//using System;
//using BaelorApi.Extentions;

namespace BaelorApi.Areas.Api.v0.Controllers
{
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

			#region [ Populate Albums ]

			//_albumRepository.Add(new Album
			//{
			//	Genres = "Country",
			//	Producers = "Scott Borchetta,Nathan Chapman,Robert Ellis Orrall",
			//	Label = "Big Machine",
			//	LengthSeconds = 2306,
			//	Name = "Taylor Swift",
			//	ReleasedAt = new DateTime(2006, 10, 24),
			//	Slug = "Taylor Swift".ToSlug()
			//});

			//_albumRepository.Add(new Album
			//{
			//	Genres = "Country,Country Pop",
			//	Producers = "Scott Borchetta,Nathan Chapman,Taylor Swift",
			//	Label = "Big Machine",
			//	LengthSeconds = 3213,
			//	Name = "Fearless",
			//	ReleasedAt = new DateTime(2008, 11, 11),
			//	Slug = "Fearless".ToSlug()
			//});

			//_albumRepository.Add(new Album
			//{
			//	Genres = "Country Pop",
			//	Producers = "Nathan Chapman,Taylor Swift",
			//	Label = "Big Machine",
			//	LengthSeconds = 4049,
			//	Name = "Speak Now",
			//	ReleasedAt = new DateTime(2010, 10, 25),
			//	Slug = "Speak Now".ToSlug()
			//});

			//_albumRepository.Add(new Album
			//{
			//	Genres = "Country,Pop Rock",
			//	Producers = "Scott Borchetta,Jeff Bhasker,Nathan Chapman,Dann Huff,Jacknife Lee,Max Martin,Shellback,Taylor Swift,Butch Walker,Dan Wilson",
			//	Label = "Big Machine",
			//	LengthSeconds = 3911,
			//	Name = "Red",
			//	ReleasedAt = new DateTime(2012, 10, 22),
			//	Slug = "Red".ToSlug()
			//});

			//_albumRepository.Add(new Album
			//{
			//	Genres = "Pop,Synthpop",
			//	Producers = "Max Martin,Taylor Swift,Jack Antonoff,Nathan Chapman,Imogen Heap,Greg Kurstin,Mattman & Robin,Ali Payami,Shellback,Ryan Tedder,Noel Zancanella",
			//	Label = "Big Machine",
			//	LengthSeconds = 2921,
			//	Name = "1989",
			//	ReleasedAt = new DateTime(2014, 10, 27),
			//	Slug = "1989".ToSlug()
			//});

			#endregion
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
