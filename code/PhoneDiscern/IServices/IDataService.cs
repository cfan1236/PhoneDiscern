using PhoneDiscern.Domain;
using PhoneDiscern.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneDiscern.IServices
{
	/// <summary>
	/// 数据处理服务接口
	/// </summary>
	public interface IDataService
	{
		/// <summary>
		/// 获取所有标记类型
		/// </summary>
		/// <returns></returns>
		Task<List<MarkType>> GetAllMarkType();

		/// <summary>
		/// 用户添加标记
		/// </summary>
		/// <param name="addMark"></param>
		/// <returns></returns>
		Task<bool> AddUserMark(AddMark addMark);

		/// <summary>
		/// 获取电话标记
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		Task<List<PhoneMark>> GetPhoneMark(string phone);


	}
}
