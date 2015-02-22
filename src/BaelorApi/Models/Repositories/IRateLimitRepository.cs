using BaelorApi.Models.Database;
using System;

namespace BaelorApi.Models.Repositories
{
	public interface IRateLimitRepository
		: IDatabaseRepository<RateLimit>
	{
		/// <summary>
		/// Gets an item in the repository with the specified User Id.
		/// </summary>
		/// <param name="userId">The User Id of the item to find.</param>
		/// <returns>Returns the item that uses the specified User Id. Returns null if no item has that User Id.</returns>
		RateLimit GetByUserId(Guid userId);

		/// <summary>
		/// Gets if a specific user has reached the rate limit.
		/// </summary>
		/// <param name="id">The Id of the item in the repository.</param>
		/// <returns>A <see cref="bool"/> representation of the limit being reached.</returns>
		bool HasRateLimitBeenReached(Guid id);

		/// <summary>
		/// Adds 1 to the request count of the RateLimit item in the repository.
		/// </summary>
		/// <param name="id">The Id of the item in the repository.</param>
		/// <returns>The new request count of the <see cref="RateLimit"/>.</returns>
		int IncrementRequestCount(Guid id);

		/// <summary>
		/// Sets the request count of the RateLimit item in the repository.
		/// </summary>
		/// <param name="id">The Id of the item in the repository.</param>
		/// <param name="requestCount">The new request count to set on the item.</param>
		void SetRequestCount(Guid id, int requestCount);
	}
}