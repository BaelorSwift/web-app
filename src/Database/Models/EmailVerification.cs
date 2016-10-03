using System;
using Baelor.Converters;
using Baelor.Crypto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Baelor.Database.Models
{
	public class EmailVerification : Audit
	{
		public EmailVerification() { }

		public EmailVerification(ObjectId userId)
		{
			Code = RandomHelpers.GetUniqueKey(20);
			UserId = userId;
			ExpiresAt = DateTime.UtcNow.AddMinutes(5);
		}

		[BsonElement("code")]
		public string Code { get; set; }

		[BsonElement("user_id")]
		[JsonConverter(typeof(ObjectIdConverter))]
		public ObjectId UserId { get; set; }

		[BsonElement("used")]
		public bool Used { get; set; }

		[BsonElement("expires_at")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime ExpiresAt { get; set; }
	}
}
