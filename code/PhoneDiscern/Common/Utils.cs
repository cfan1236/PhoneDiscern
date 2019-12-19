using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace PhoneDiscern.Common
{
	public class Utils
	{
		/// <summary>
		/// 图像灰度化
		/// </summary>
		/// <param name="bmp"></param>
		/// <returns></returns>
		public static Bitmap ToGray(Bitmap bmp)
		{
			for (int i = 0; i < bmp.Width; i++)
			{
				for (int j = 0; j < bmp.Height; j++)
				{
					//获取该点的像素的RGB的颜色
					Color color = bmp.GetPixel(i, j);
					//利用公式计算灰度值
					int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
					Color newColor = Color.FromArgb(gray, gray, gray);
					bmp.SetPixel(i, j, newColor);
				}
			}
			return bmp;
		}

		/// <summary>
		/// 图像二值化(迭代法)
		/// </summary>
		/// <param name="bmp"></param>
		/// <returns></returns>
		public static Bitmap ToBinaryImage(Bitmap bmp)
		{
			int[] histogram = new int[256];
			int minGrayValue = 255, maxGrayValue = 0;
			//求取直方图
			for (int i = 0; i < bmp.Width; i++)
			{
				for (int j = 0; j < bmp.Height; j++)
				{
					Color pixelColor = bmp.GetPixel(i, j);
					histogram[pixelColor.R]++;
					if (pixelColor.R > maxGrayValue) maxGrayValue = pixelColor.R;
					if (pixelColor.R < minGrayValue) minGrayValue = pixelColor.R;
				}
			}
			//迭代计算阀值
			int threshold = -1;
			int newThreshold = (minGrayValue + maxGrayValue) / 2;
			for (int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++)
			{
				threshold = newThreshold;
				int lP1 = 0;
				int lP2 = 0;
				int lS1 = 0;
				int lS2 = 0;
				//求两个区域的灰度的平均值
				for (int i = minGrayValue; i < threshold; i++)
				{
					lP1 += histogram[i] * i;
					lS1 += histogram[i];
				}
				int mean1GrayValue = (lP1 / lS1);
				for (int i = threshold + 1; i < maxGrayValue; i++)
				{
					lP2 += histogram[i] * i;
					lS2 += histogram[i];
				}
				int mean2GrayValue = (lP2 / lS2);
				newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
			}

			//计算二值化
			for (int i = 0; i < bmp.Width; i++)
			{
				for (int j = 0; j < bmp.Height; j++)
				{
					Color pixelColor = bmp.GetPixel(i, j);
					if (pixelColor.R > threshold) bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
					else bmp.SetPixel(i, j, Color.FromArgb(0, 0, 0));
				}
			}
			return bmp;
		}

		/// <summary>
		/// 保存base64图像
		/// </summary>
		/// <param name="strbase64"></param>
		public static void SaveBase64Image(string strbase64, string saveFilePath)
		{

			byte[] b = Convert.FromBase64String(strbase64);
			System.IO.File.WriteAllBytes(saveFilePath, b);

		}

		/// <summary>
		/// 获取guid
		/// </summary>
		/// <returns></returns>
		public static string GetGuid()
		{
			return Guid.NewGuid().ToString("N");
		}
		/// <summary>
		/// 下载web图像
		/// </summary>
		/// <param name="url"></param>
		public static void DownLoadWebImage(string url, string saveFilePath)
		{
			WebClient wc = new WebClient();
			wc.DownloadFile(new Uri(url), saveFilePath);
		}

		/// <summary>
		/// 图像旋转
		/// </summary>
		/// <param name="bmp">原始图像</param>
		/// <param name="angle">角度</param>
		/// <returns></returns>
		public static Bitmap RotateImage(Bitmap bmp, double angle)
		{
			Graphics g = null;
			Bitmap tmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppRgb);
			tmp.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
			g = Graphics.FromImage(tmp);
			try
			{
				g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
				g.RotateTransform((float)angle);
				g.DrawImage(bmp, 0, 0);
			}
			finally
			{
				g.Dispose();
			}
			return tmp;
		}


	}
}
