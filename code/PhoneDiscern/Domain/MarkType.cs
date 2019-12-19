using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhoneDiscern.Domain
{
	/// <summary>
	/// 标记类型
	/// </summary>
	public class MarkType : IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonElement("index")]
		public int Index { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }
	}
}
