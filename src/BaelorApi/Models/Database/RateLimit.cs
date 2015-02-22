using System;

namespace BaelorApi.Models.Database
{
	public class RateLimit
		: Audit
	{
		public const int RequestHourlyLimit = 1000;

		public int RequestsMade { get; set; }

		public bool RateLimitReached
		{
			get { return RequestsMade >= RequestHourlyLimit; }
		}

		#region [ User ]

		public User User { get; set; }

		public Guid UserId { get; set; }

		#endregion
	}
}
