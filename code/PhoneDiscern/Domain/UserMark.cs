using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhoneDiscern.Domain
{
	/// <summary>
	/// 用户标记
	/// </summary>
	public class UserMark : IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonElement("mark_index")]
		public int markIndex { get; set; }
		[BsonElement("name")]
		public string Name { get; set; }
		[BsonElement("phone")]
		public string Phone { get; set; }

	}
}
