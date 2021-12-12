using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunBallGiftCode.Models
{
    public class ResponesData
    {
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }

        public class Data
        {
            public string captchaId { get; set; }
        }
    }
}
