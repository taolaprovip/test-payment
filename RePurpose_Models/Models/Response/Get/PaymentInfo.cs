using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Models.Response.Get
{
    public class PaymentInfo
    {
        public string TmnCode { get; set; }
        public long Amount { get; set; }
        public string BankCode { get; set; }
        public string OrderInfo { get; set; }
        public long TransactionNo { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionStatus { get; set; }
        

        public long TxnRef { get; set; }
        public string SecureHash { get; set; }
    }

}
