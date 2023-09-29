﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Models.Response.Get
{
    public class PaymentInfo
    {
        public string vnp_TmnCode { get; set; }
        public long vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public string vnp_OrderInfo { get; set; }
        public long vnp_TransactionNo { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TransactionStatus { get; set; }
        

        public long vnp_TxnRef { get; set; }
        public string vnp_SecureHash { get; set; }
    }

}
