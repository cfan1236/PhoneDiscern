using MongoDB.Driver;
using PhoneDiscern.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneDiscern.Repository
{
	public interface IContext<T> where T:IEntity
	{
		IMongoCollection<T> Entities { get; }
	}
}
