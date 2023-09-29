using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Models.Requests.Get
{
    public class LinkPayCreateModel
    {
        public string vnp_Version { get; set; }
        public string vnp_Command { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_Amount { get; set; }
        public string vnp_CreateDate { get; set; }
        public string vnp_CurrCode { get; set; }
        public string vnp_IpAddr { get; set; }
        public string vnp_Locale { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_ReturnUrl { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_OrderType { get; set; }
    }
}
