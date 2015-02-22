using BaelorApi.Exceptions;
using BaelorApi.Models.Database;
using BaelorApi.Models.Error.Enums;
using BaelorApi.Models.Miscellaneous;
using BaelorApi.Models.Repositories;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Net;

namespace BaelorApi.Attributes
{
	public class RequireAuthenticationAttribute
		: ActionFilterAttribute
	{
		/// <summary>
		/// Checks if the request has valid authentication headers.
		/// </summary>
		public RequireAuthenticationAttribute() { }

		/// <summary>
		/// Check the `Authentication` header for a valid api key.
		/// </summary>
		/// <param name="context">The action context.</param>
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			string[] authTokens;
			context.HttpContext.Request.Headers
				.TryGetValue("Authorization", out authTokens);

			// Check if the header exists
			if (authTokens == null || !authTokens.Any() || 
				!authTokens.First().ToLowerInvariant().StartsWith("bearer"))
				throw new BaelorV0Exception(ErrorStatus.RequestRequiredAuthentication, HttpStatusCode.Forbidden);

			// Extract token
			var apiKey = authTokens.First().Remove(0, 7);

			using (var dbContext = new DatabaseContext())
			{
				var userRepo = new UserRepository(dbContext);
				var rateLimitRepo = new RateLimitRepository(dbContext);

				// Validate token exists and hasn't been revoked
				var user = userRepo.GetByApiKey(apiKey);
				if (user == null)
					throw new BaelorV0Exception(ErrorStatus.InvalidApiKey, HttpStatusCode.Forbidden);
				if (user.IsRevoked)
					throw new BaelorV0Exception(ErrorStatus.RevokedApiKey, HttpStatusCode.Forbidden);

				// Save user authentication
				context.HttpContext.SetFeature<AuthenticationStorage>(
					new AuthenticationStorage(user));

				// Get (or create?) a rate limit
				var rateLimit = rateLimitRepo.GetByUserId(user.Id);
				if (rateLimit == null)
				{
					rateLimit = new RateLimit
					{
						RequestsMade = 0,
						UserId = user.Id
					};
					rateLimit = rateLimitRepo.Add(rateLimit);
				}

				// dem valz
				var rateLimitExceeded = false;
				var rateLimitMax = RateLimit.RequestHourlyLimit;
				var rateLimitRequestsMade = rateLimit.RequestsMade;

				// calculate utc reset
				var now = DateTime.UtcNow;
				var reset = new DateTime(
					now.Year, now.Month, now.Day, now.Hour, 0, 0);
				reset = reset.AddHours(1);
				
				if (rateLimit.RateLimitReached)
					rateLimitExceeded = true;
				else
					rateLimitRequestsMade = 
						rateLimitRepo.IncrementRequestCount(rateLimit.Id);

				context.HttpContext.Response.Headers
					.Add("X-RateLimit-Limit", new[] { rateLimitMax.ToString() });
				context.HttpContext.Response.Headers
					.Add("X-RateLimit-Remaining", new[] { (rateLimitMax - rateLimitRequestsMade).ToString() });
				context.HttpContext.Response.Headers
					.Add("X-RateLimit-Reset", new[] { reset.ToString() });

				if (rateLimitExceeded)
					throw new BaelorV0Exception(ErrorStatus.RateLimitExceeded, (HttpStatusCode) 429);
			}

			base.OnActionExecuting(context);
		}
	}
}
