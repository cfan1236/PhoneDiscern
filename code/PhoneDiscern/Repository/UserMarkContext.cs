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
	public class UserMarkContext : IContext<UserMark>
	{
		private readonly IMongoDatabase _db;
		public UserMarkContext(IOptions<AppSettings> options)
		{
			var client = new MongoClient(options.Value.mongodb.ConnectionStr);
			_db = client.GetDatabase(options.Value.mongodb.Database);
		}

		public IMongoCollection<UserMark> Entities => _db.GetCollection<UserMark>("UserMark");
	}
}
