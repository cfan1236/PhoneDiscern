using PhoneDiscern.Model;

namespace PhoneDiscern.IServices
{
	/// <summary>
	/// ocr 处理接口
	/// </summary>
	public interface IOcrService
	{

		/// <summary>
		/// 数字识别
		/// </summary>
		/// <param name="base64_image"></param>
		/// <param name="image_url"></param>
		/// <returns></returns>
		PhoneDiscernResult DiscernNumber(string base64_image, string image_url);
	}
}
