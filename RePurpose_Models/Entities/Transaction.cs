using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Entities
{
    public class Transaction
    {
        public long TransactionId { get; set; }
        public Guid? WalletId { get; set; }
        public string? Type { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Timestamp { get; set; }

        // Mối quan hệ với bảng Wallet
        public Wallet Wallet { get; set; }
    }
}
