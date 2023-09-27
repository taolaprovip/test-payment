using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Models.View
{
    public class TransactionViewModel
    {
        public Guid TransactionId { get; set; }
        public Guid? WalletId { get; set; }
        public string? Type { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Timestamp { get; set; }

    }
}
