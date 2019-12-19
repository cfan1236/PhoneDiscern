using NLog;
using PhoneDiscern.Domain;
using PhoneDiscern.IServices;
using PhoneDiscern.Model;
using PhoneDiscern.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneDiscern.Services
{
	/// <summary>
	/// 数据处理服务
	/// </summary>
	public class DataService : IDataService
	{
		private readonly IRepository<MarkType> _marktypeRepository;
		private readonly IRepository<UserMark> _userMarkRepository;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public DataService(
			   IRepository<MarkType> marktypeRepository,
			   IRepository<UserMark> userMarkRepository)
		{
			_marktypeRepository = marktypeRepository;
			_userMarkRepository = userMarkRepository;
		}

		public async Task<List<MarkType>> GetAllMarkType()
		{
			try
			{
				return await _marktypeRepository.GetAllList();
			}
			catch (System.Exception ex)
			{
				_logger.Error(ex);
			}

			return null;
		}
		/// <summary>
		/// 添加用户标记
		/// </summary>
		/// <param name="addMark"></param>
		/// <returns></returns>
		public async Task<bool> AddUserMark(AddMark addMark)
		{
			try
			{
				var id = await _userMarkRepository.Insert(new UserMark()
				{
					markIndex = addMark.mark_index,
					Name = await GetMarkName(addMark.mark_index),
					Phone = addMark.phone
				});
				if (string.IsNullOrWhiteSpace(id))
					return false;
				return true;
			}
			catch (System.Exception ex)
			{
				_logger.Error(ex);
			}

			return false;
		}

		/// <summary>
		/// 获取标记名称
		/// </summary>
		/// <param name="index">标记索引</param>
		/// <returns></returns>
		private async Task<string> GetMarkName(int index)
		{
			var list_mark = await _marktypeRepository.GetList(m => m.Index == index);

			if (list_mark != null && list_mark.Count > 0)
			{
				return list_mark[0].Name;
			}
			return "";
		}

		/// <summary>
		/// 获取手机号码标记
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		public async Task<List<PhoneMark>> GetPhoneMark(string phone)
		{
			var data = new List<PhoneMark>();
			var userMark = await _userMarkRepository.GetList(u => u.Phone == phone);
			if (userMark != null)
			{
				var dic_count = new Dictionary<string, int>();
				foreach (var item in userMark)
				{
					int count = 1;
					if (dic_count.ContainsKey(item.Name))
					{
						count = dic_count[item.Name];
						count++;
						dic_count[item.Name] = count;
					}
					else
					{
						dic_count.Add(item.Name, count);
					}

				}
				// 将统计结果输出到list
				foreach (var item in dic_count)
				{
					data.Add(new PhoneMark()
					{
						count = item.Value,
						Name = item.Key

					});
				}
			}

			return data;

		}
	}
}
