using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Entities
{
    public class Wallet
    {
        public Guid WalletId { get; set; }
        public Guid? MemberId { get; set; }
        public decimal? Balance { get; set; }
        public DateTime ?LastBalanceUpdate { get; set; }
        public Member? Member { get; set; }

        // Mối quan hệ với bảng Transaction
        public List<Transaction> Transactions { get; set; }
    }
}
