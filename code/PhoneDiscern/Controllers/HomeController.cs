using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using PhoneDiscern.Domain;
using PhoneDiscern.IServices;
using PhoneDiscern.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneDiscern.Controllers
{
	public class HomeController : Controller
	{
		private readonly IOcrService _ocrService;
		private readonly IDataService _dataService;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		public HomeController(IOcrService ocrService, IDataService dataService)
		{
			this._ocrService = ocrService;
			this._dataService = dataService;
		}
		//显示欢迎信息
		public string Index()
		{
			return "Welcome To Phone Discern API";
		}

		[Route("api/phonediscern")]
		[HttpPost]
		public async Task<JsonData<PhoneDiscernResult>> PhoneDiscern([FromBody]PhoneDiscernReq req)
		{
			var jdata = new JsonData<PhoneDiscernResult>();
			try
			{
				jdata.status = 0;
				jdata.error = "暂未识别到电话";
				if (req != null)
				{
					//优先使用url
					if (!string.IsNullOrWhiteSpace(req.image_url))
					{
						req.base64_image = "";
					}
					var data = _ocrService.DiscernNumber(req.base64_image, req.image_url);
					if (data != null && !string.IsNullOrWhiteSpace(data.text))
					{
						var phone_mark = await _dataService.GetPhoneMark(data.text);
						data.phoneMark = phone_mark;
						jdata.status = 1;
						jdata.data = data;
						jdata.error = "";
					}
				}
				else
				{
					jdata.status = 0;
					jdata.error = "Parameter cannot be empty";
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				jdata.error = ex.Message;
			}
			return jdata;
		}
		/// <summary>
		/// 获取标记
		/// </summary>
		/// <returns></returns>
		[Route("api/getmark")]
		[HttpGet]
		public async Task<JsonData<List<MarkType>>> GetMark()
		{
			var jdata = new JsonData<List<MarkType>>();
			jdata.status = 0;
			jdata.error = "";
			var list = await _dataService.GetAllMarkType();
			if (list != null)
			{
				jdata.data = list;
				jdata.status = 1;
			}
			return jdata;
		}

		/// <summary>
		/// 获取手机号标记
		/// </summary>
		/// <returns></returns>
		[Route("api/getphonemark")]
		[HttpGet]
		public async Task<JsonData<List<PhoneMark>>> GetPhoneMark(string phone)
		{
			var jdata = new JsonData<List<PhoneMark>>();
			jdata.status = 0;
			jdata.error = "";
			var list = await _dataService.GetPhoneMark(phone);
			if (list != null)
			{
				jdata.data = list;
				jdata.status = 1;
			}
			return jdata;
		}

		/// <summary>
		/// 添加用户标记
		/// </summary>
		/// <returns></returns>
		[Route("api/addusermark")]
		[HttpPost]
		public async Task<JsonData<bool>> AddUserMark([FromBody]AddMark req)
		{
			var jdata = new JsonData<bool>();
			jdata.status = 0;
			if (req != null)
			{
				var flag = await _dataService.AddUserMark(req);
				jdata.data = flag;
				jdata.status = 1;
			}
			else
			{
				jdata.status = 0;
				jdata.error = "Parameter cannot be empty";
			}
			return jdata;
		}





	}
}
