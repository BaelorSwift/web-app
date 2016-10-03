using System;
using Baelor.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Baelor.Database.Models
{
	public abstract class Audit
	{
		[JsonConverter(typeof(ObjectIdConverter))]
		public ObjectId Id { get; set; }

		[BsonElement("created_at")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		[BsonElement("updated_at")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}
