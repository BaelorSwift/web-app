using BaelorApi.Attributes;
using BaelorApi.Exceptions;
using BaelorApi.Models.Api;
using BaelorApi.Models.Error.Enums;
using BaelorApi.Models.Repositories;
using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace BaelorApi.Areas.Api.v0
{
	[ExceptionHandler]
	[SetResponseHeaders]
	[Route("api/v0/[controller]")]
	public class RateLimitResetController : ApiController
	{
		/// <summary>
		/// 
		/// </summary>
		private IRateLimitRepository _rateLimitRepository;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rateLimitRepository"></param>
		public RateLimitResetController(IRateLimitRepository rateLimitRepository)
		{
			_rateLimitRepository = rateLimitRepository;
		}
		
		/// <summary>
		///		[PUT] api/v0/ratelimitreset
		/// Resets all the rate limits
		/// </summary>
		[HttpPut]
		public IActionResult Put()
		{
			string[] authTokens;
			Context.Request.Headers.TryGetValue("Authorization", out authTokens);

			// Check if the header exists
			if (authTokens == null || !authTokens.Any() || !authTokens.First().ToLowerInvariant().StartsWith("bearer"))
				throw new BaelorV0Exception(ErrorStatus.RequestRequiredAuthentication, HttpStatusCode.Forbidden);
			if (string.Format("bearer {0}", authTokens.First()) != Startup.Configuration.Get("Data:AzureJobSecretIdetifier"))
				throw new BaelorV0Exception(ErrorStatus.InvalidApiKey, HttpStatusCode.Forbidden);

			// Reset bro
			foreach (var rateLimit in _rateLimitRepository.GetAll)
				_rateLimitRepository.SetRequestCount(rateLimit.Id, 0);

			// All Gucci
			return Content(HttpStatusCode.OK,
				new ResponseBase { Result = true });
		}
	}
}