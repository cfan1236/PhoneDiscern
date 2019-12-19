using MongoDB.Driver;
using PhoneDiscern.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PhoneDiscern.Repository
{
	public class UserMarkRepository : IRepository<UserMark>
	{
		private readonly IContext<UserMark> _context;

		public UserMarkRepository(IContext<UserMark> context)
		{
			_context = context;
		}
		public async Task<string> Insert(UserMark t)
		{
			await _context.Entities.InsertOneAsync(t);
			return t.Id;
		}

		public void BatchInsert(List<UserMark> t)
		{
			_context.Entities.InsertMany(t);
		}

		public async Task<bool> Delete(string id)
		{
			var deleteResult = await _context.Entities.DeleteOneAsync(x => x.Id == id);
			return deleteResult.DeletedCount != 0;
		}

		public async Task<List<UserMark>> GetAllList(int skip = 0, int count = 0)
		{
			var result = await _context.Entities.Find(x => true)
							   .Skip(skip)
							   .Limit(count)
							   .ToListAsync();
			return result;
		}

		public async Task<List<UserMark>> GetList(Expression<Func<UserMark, bool>> conditions = null)
		{
			var result = new List<UserMark>();
			if (conditions != null)
			{
				result = await _context.Entities.Find(conditions).ToListAsync();
			}
			else
			{
				result = await _context.Entities.Find(_ => true).ToListAsync();
			}

			return result;
		}

		public async Task<UserMark> GetById(string id)
		{
			return await _context.Entities.Find(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<bool> Update(UserMark t)
		{
			var filter = Builders<UserMark>.Filter.Eq(x => x.Id, t.Id);
			var replaceOneResult = await _context.Entities.ReplaceOneAsync(doc => doc.Id == t.Id, t);
			return replaceOneResult.ModifiedCount != 0;
		}

	}
}
