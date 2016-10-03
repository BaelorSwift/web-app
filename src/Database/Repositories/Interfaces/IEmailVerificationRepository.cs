using System.Threading.Tasks;
using Baelor.Database.Models;
using MongoDB.Bson;

namespace Baelor.Database.Repositories.Interfaces
{
	public interface IEmailVerificationRepository : IMongoRepository<EmailVerification>
	{
		Task<EmailVerification> GetByCode(string code);

		Task<bool> RevokeAllCodes(ObjectId userId);
	}
}
