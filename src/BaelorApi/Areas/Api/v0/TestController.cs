using System.Web.Http;
using BaelorApi.Attributes;
using Microsoft.AspNet.Mvc;
using System.Net;
using BaelorApi.Models.Api;

namespace BaelorApi.Areas.Api.v0
{
	[ExceptionHandler]
	[SetResponseHeaders]
	//[RequireAuthentication]
	//[RequireAdminAuthentication]
	[Route("api/v0/[controller]")]
	public class TestController : ApiController
	{
		/// <summary>
		///		[GET] api/v0/test
		/// Gets some random test logic
		/// </summary>
		[HttpGet]
		public IActionResult Get()
		{
			return Content(HttpStatusCode.OK, new ResponseBase
			{
				Result = Startup.Configuration.Get("Data:DefaultConnection:ConnectionString")
			});
		}
	}
}