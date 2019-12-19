using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneDiscern.Model
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class PhoneDiscernReq
    {
        public string image_url { get; set; }

        public string base64_image { get; set; }
    }
}
