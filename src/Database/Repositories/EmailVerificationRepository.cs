using System;
using System.Threading.Tasks;
using Baelor.Database.Models;
using Baelor.Database.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Baelor.Database.Repositories
{
	public class EmailVerificationRepository : MongoRepository<EmailVerification>, IEmailVerificationRepository
	{
		public EmailVerificationRepository(MongoDatabase mongoDatabase)
			: base(mongoDatabase)
		{
			CollectionName = "email_verifications";
		}

		public async Task<EmailVerification> GetByCode(string code)
		{
			var result = await Query(Builders<EmailVerification>.Filter.Eq(ev => ev.Code, code));
			return await result.FirstOrDefaultAsync();
		}

		public async Task<bool> RevokeAllCodes(ObjectId userId)
		{
			var filter = Builders<EmailVerification>.Filter.Eq(u => u.UserId, userId);
			var update = Builders<EmailVerification>.Update
				.Set("expires_at", DateTime.UtcNow)
				.CurrentDate("updated_at");

			var result = await _mongoDatabase.Database.GetCollection<EmailVerification>(CollectionName)
				.UpdateManyAsync(filter, update);

			return result.IsAcknowledged;
		}
	}
}
