using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Wallet
    {
        public Wallet()
        {
            WalletPurchases = new HashSet<WalletPurchases>();
            WalletTransactions = new HashSet<WalletTransactions>();
        }

        public long Id { get; set; }
        public long? UserId { get; set; }
        public int? Value { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public short? Status { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<WalletPurchases> WalletPurchases { get; set; }
        public virtual ICollection<WalletTransactions> WalletTransactions { get; set; }
    }
}
