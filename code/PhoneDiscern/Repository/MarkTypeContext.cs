using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PhoneDiscern.Domain;
using PhoneDiscern.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneDiscern.Repository
{
	public class MarkTypeContext:IContext<MarkType>
	{
		private readonly IMongoDatabase _db;
		public MarkTypeContext(IOptions<AppSettings> options)
		{
			var client = new MongoClient(options.Value.mongodb.ConnectionStr);
			_db = client.GetDatabase(options.Value.mongodb.Database);
		}

		public IMongoCollection<MarkType> Entities => _db.GetCollection<MarkType>("MarkType");
	}
}
