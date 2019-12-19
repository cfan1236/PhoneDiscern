
using NLog;
using PhoneDiscern.Common;
using PhoneDiscern.IServices;
using PhoneDiscern.Model;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Tesseract;

namespace PhoneDiscern.Services
{
	public class TesseractOcrService : IOcrService
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		/// <summary>
		/// 数字识别
		/// </summary>
		/// <param name="base64_image"></param>
		/// <param name="image_url"></param>
		/// <returns></returns>
		public PhoneDiscernResult DiscernNumber(string base64_image, string image_url)
		{
			PhoneDiscernResult result = new PhoneDiscernResult();
			string imageFile = GetImageFileName();
			if (!string.IsNullOrEmpty(image_url))
			{
				Utils.DownLoadWebImage(image_url, imageFile);
			}
			else
			{
				Utils.SaveBase64Image(base64_image, imageFile);
			}
			if (File.Exists(imageFile))
			{
				string[] taskResult = new string[3];
				// 三个线程同时去处理执行 
				// 每个线程处理的图片都不一样 取结果最好的一个
				Task[] tk = new Task[] {
					Task.Factory.StartNew(()=>
					{
						// 原图识别
						taskResult[0]=Discern(imageFile);
					}),
					Task.Factory.StartNew(()=>
					{
						// 灰度处理后识别
						taskResult[1]=GrayDiscern(imageFile);
					}),
					Task.Factory.StartNew(()=>
					{
						// 二值化处理后识别
						taskResult[2]=BinaryzationDiscern(imageFile);
					}),
				};
				// 超时1分钟
				int timeout = (1000 * 60) * 1;
				Task.WaitAll(tk, timeout);
				var number_str = taskResult[0];
				if (taskResult[1].Length > number_str.Length)
				{
					number_str = taskResult[1];
				}
				if (taskResult[2].Length > number_str.Length)
				{
					number_str = taskResult[2];
				}
				result.text = number_str;
				if (number_str.Length == 11)
				{
					result.message = "识别成功";
				}
				else
				{
					result.message = "当前识别的电话可能有误，请注意辨别";
				}

			}
			return result;
		}


		/// <summary>
		/// 直接识别
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private string Discern(string imageFile)
		{
			string number_str = "";
			// 这里可以选择不同的语言包 可以是自己训练的 可以是Tesseract 训练好的语言包
			TesseractEngine te_ocr = new TesseractEngine(@"tessdata", "chi_sim", EngineMode.TesseractAndLstm);
			var img = Pix.LoadFromFile(imageFile);
			var page = te_ocr.Process(img, PageSegMode.Auto);
			string text = page.GetText().Trim().Replace("\r", "").Replace("\n", "");
			_logger.Info("识别的原始数据:"+text);
			page.Dispose();
			//只提取数字
			number_str = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9]+", "");
			_logger.Info("只提取数字结果:" + number_str);
			return number_str;
		}

		/// <summary>
		/// 灰度识别
		/// </summary>
		/// <param name="imageFile"></param>
		/// <returns></returns>
		private string GrayDiscern(string imageFile)
		{
			string number_str = "";
			using (Bitmap bmp = new Bitmap(imageFile))
			{
				// 灰度处理
				var bmps = Utils.ToGray(bmp);
				var tempFile = GetImageFileName(1);
				bmps.Save(tempFile);
				number_str = Discern(tempFile);
				//File.Delete(tempFile);
			}
			return number_str;
		}
		/// <summary>
		/// 二值化识别
		/// </summary>
		/// <param name="imageFile"></param>
		/// <returns></returns>
		private string BinaryzationDiscern(string imageFile)
		{
			string number_str = "";
			using (Bitmap bmp = new Bitmap(imageFile))
			{
				// 灰度处理
				var bmps = Utils.ToGray(bmp);
				// 处理自动校正
				gmseDeskew sk = new gmseDeskew(bmps);
				double skewangle = sk.GetSkewAngle();
				Bitmap bmpOut = Utils.RotateImage(bmps, -skewangle);
				var tempFile = GetImageFileName(1);
				// 将二值化后的图像保存下 
				Utils.ToBinaryImage(bmpOut).Save(tempFile);
				number_str = Discern(tempFile);
				//File.Delete(tempFile);
			}
			return number_str;
		}


		/// <summary>
		/// 获取文件名
		/// </summary>
		/// <param name="type">0 时间类型 1 guid类型</param>
		/// <returns></returns>
		private static string GetImageFileName(int type = 0)
		{
			var runPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
			runPath += "/image/";
			if (!Directory.Exists(runPath))
			{
				Directory.CreateDirectory(runPath);
			}
			var fileName = string.Empty;
			if (type == 0)
				fileName = runPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
			else
				fileName = runPath + Utils.GetGuid() + ".jpg";
			return fileName;
		}
	}
}
