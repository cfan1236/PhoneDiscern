
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneDiscern.Model
{
    public class PhoneDiscernResult
    {
        /// <summary>
        /// 识别结果
        /// </summary>
        public string  text { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }

		/// <summary>
		/// 手机标记
		/// </summary>
		public List<PhoneMark> phoneMark { get; set; }
        
    }
}
