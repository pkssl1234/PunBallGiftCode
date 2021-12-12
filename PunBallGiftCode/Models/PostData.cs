using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunBallGiftCode.Models
{
    class PostData
    {
        public string userId { get; set; }
        public string giftCode { get; set; }
        public string captcha { get; set; }
        public string captchaId { get; set; }
    }
}
