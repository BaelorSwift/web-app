﻿using Microsoft.AspNet.Mvc;
using System.Net;
using System.Linq;
using System.Web.Http;
using BaelorApi.Models.Api;

namespace BaelorApi.Areas.Api.v0.Controllers
{
	[Route("api/v0/[controller]")]
	public class BaeController : ApiController
	{
		private readonly string[] _baes = {
			"taylor",
			"taylor swift",
			"baelor",
			"baelor swift",
			"swifty",
			"bae",
			"heather",
			"macphearson"
		};

		/// <summary>
		///		[GET] api/v0/bae
		/// Gets Taylor's bae status.
		/// </summary>
		[HttpGet]
		public IActionResult Get()
		{
			return Content(HttpStatusCode.OK, new ResponseBase { Result = new { bae = true } });
		}

		/// <summary>
		///		[GET] api/v0/bae/{word}
		/// Gets the bae status of a word.
		/// </summary>
		/// <param name="id">The word to get the bae status of.</param>
		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			return Content(HttpStatusCode.OK, new ResponseBase { Result = new { bae = _baes.Contains(id.ToLowerInvariant()) } });
		}
	}
}