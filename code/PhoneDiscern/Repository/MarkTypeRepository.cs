using MongoDB.Driver;
using PhoneDiscern.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PhoneDiscern.Repository
{
	public class MarkTypeRepository : IRepository<MarkType>
	{
		private readonly IContext<MarkType> _context;

		public MarkTypeRepository(IContext<MarkType> context)
		{
			_context = context;
		}
		public async Task<string> Insert(MarkType t)
		{
			await _context.Entities.InsertOneAsync(t);
			return t.Id;
		}

		public void BatchInsert(List<MarkType> t)
		{
			_context.Entities.InsertMany(t);
		}

		public async Task<bool> Delete(string id)
		{
			var deleteResult = await _context.Entities.DeleteOneAsync(x => x.Id == id);
			return deleteResult.DeletedCount != 0;
		}

		public async Task<List<MarkType>> GetAllList(int skip = 0, int count = 0)
		{
			var result = await _context.Entities.Find(x => true)
							   .Skip(skip)
							   .Limit(count)
							   .ToListAsync();
			return result;
		}         

		public async Task<List<MarkType>> GetList(Expression<Func<MarkType, bool>> conditions = null)
		{
			var result = new List<MarkType>();
			if (result != null)
			{
				result = await _context.Entities.Find(conditions).ToListAsync();
			}
			else
			{
				result = await _context.Entities.Find(_ => true).ToListAsync();
			}
			return result;
		}

		public async Task<MarkType> GetById(string id)
		{
			return await _context.Entities.Find(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<bool> Update(MarkType t)
		{
			var filter = Builders<MarkType>.Filter.Eq(x => x.Id, t.Id);
			var replaceOneResult = await _context.Entities.ReplaceOneAsync(doc => doc.Id == t.Id, t);
			return replaceOneResult.ModifiedCount != 0;
		}

	}
}
