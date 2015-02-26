using Microsoft.AspNet.Mvc;
using System.Net;
using System.Web.Http;
using BaelorApi.Models.Api;
using BaelorApi.Models.Repositories;
using BaelorApi.Models.Api.Error;
using BaelorApi.Models.Error.Enums;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using BaelorApi.Attributes;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[ExceptionHandler]
	[SetResponseHeaders]
	[Route("api/v0/[controller]")]
	public class ImagesController : ApiController
	{
		/// <summary>
		/// Repository for interacting with <see cref="Image"/> data
		/// </summary>
		private readonly IImageRepository _imageRepository;

		/// <summary>
		/// Creates a new instance of the Image Controller.
		/// </summary>
		/// <param name="imageRepository">The repository of <see cref="Image"/> data.</param>
		public ImagesController(IImageRepository imageRepository)
		{
			_imageRepository = imageRepository;
		}

		/// <summary>
		///		[GET] api/v0/images/{slug}?w={width}&h={height}
		/// Gets an image from it's id.
		/// </summary>
		/// <param name="id">The id of the image.</param>
		[HttpGet("{id}")]
		public HttpResponseMessage Get(string id)
		{
			Guid imageId = Constants.InvalidGuid;
			Guid.TryParse(id, out imageId);

			var image = _imageRepository.GetById(imageId);
			HttpResponseMessage response;
			if (image == null)
			{
				response = Request.CreateResponse(HttpStatusCode.NotFound);
				var responseBase = new ResponseBase { Error = new ErrorBase(ErrorStatus.InvalidImageId), Success = false };
				response.Content = new StringContent(responseBase.ToString(), Encoding.UTF8, "application/json");
				return response;
			}
			
			response = Request.CreateResponse(HttpStatusCode.OK);
			response.Content = new StreamContent(new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, image.FilePath), FileMode.Open));
			response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
			return response;
		}
	}
}
