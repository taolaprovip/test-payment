using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RePurpose_Models.Models.View
{
    public class WalletViewModel
    {
        public Guid WalletId { get; set; }
        public Guid? MemberId { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? LastBalanceUpdate { get; set; } = DateTime.UtcNow;
    }
}
