using BaelorApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaelorApi.Models.Repositories
{
	public class RateLimitRepository
		: IRateLimitRepository
	{
		/// <summary>
		/// Gets the <see cref="DatabaseContext"/> used by the repository.
		/// </summary>
		private readonly DatabaseContext _db;

		/// <summary>
		/// Creates a new <see cref="RateLimitRepository"/>.
		/// </summary>
		/// <param name="db">An initalized <see cref="DatabaseContext"/> used for database connection management.</param>
		public RateLimitRepository(DatabaseContext db)
		{
			_db = db;
		}

		public IEnumerable<RateLimit> GetAll
		{
			get
			{
				return _db.RateLimits.AsEnumerable();
			}
		}

		public RateLimit Add(RateLimit item)
		{
			_db.RateLimits.Add(item);

			if (_db.SaveChanges() > 0)
				return item;

			return null;
		}

		public RateLimit GetById(Guid id)
		{
			return _db.RateLimits.FirstOrDefault(r => r.Id == id);
		}

		public RateLimit GetByUserId(Guid userId)
		{
			return _db.RateLimits.FirstOrDefault(r => r.UserId == userId);
		}

		public bool HasRateLimitBeenReached(Guid id)
		{
			var item = _db.RateLimits.FirstOrDefault(r => r.Id == id);
			if (item == null)
				return true;

			return item.RateLimitReached;
		}

		public int IncrementRequestCount(Guid id)
		{
			var item = _db.RateLimits.FirstOrDefault(r => r.Id == id);
			if (item == null)
				return -1;

			item.RequestsMade++;
			Update(item.Id, item);
			return item.RequestsMade;
		}

		public void SetRquestCount(Guid id, int requestCount)
		{
			var item = _db.RateLimits.FirstOrDefault(r => r.Id == id);
			if (item == null)
				return;

			item.RequestsMade = requestCount;
			Update(item.Id, item);
		}

		public RateLimit Update(Guid id, RateLimit delta)
		{
			var item = _db.RateLimits.FirstOrDefault(r => r.Id == id);
			if (item == null)
				return null;

			item.RequestsMade = delta.RequestsMade;
			item.UserId = delta.UserId;

			if (_db.SaveChanges() > 0)
				return delta;

			return null;
		}

		public bool TryDelete(Guid id)
		{
			var RateLimit = _db.RateLimits.FirstOrDefault(c => c.Id == id);
			if (RateLimit != null)
			{
				_db.RateLimits.Remove(RateLimit);
				return _db.SaveChanges() > 0;
			}
			return false;
		}
	}
}