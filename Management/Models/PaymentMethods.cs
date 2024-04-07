using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class PaymentMethods
    {
        public PaymentMethods()
        {
            WalletTransactions = new HashSet<WalletTransactions>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public short? Status { get; set; }

        public virtual ICollection<WalletTransactions> WalletTransactions { get; set; }
    }
}
