using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class WalletTransactions
    {
        public long Id { get; set; }
        public long? WalletId { get; set; }
        public long? PaymentMethodId { get; set; }
        public int? Value { get; set; }
        public short? ProcessType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public short? Status { get; set; }

        public virtual PaymentMethods PaymentMethod { get; set; }
        public virtual Wallet Wallet { get; set; }
    }
}
