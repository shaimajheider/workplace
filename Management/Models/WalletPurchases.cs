using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class WalletPurchases
    {
        public long Id { get; set; }
        public long? WalletId { get; set; }
        public long? SubscriptionsId { get; set; }
        public int? Value { get; set; }
        public int? SubscriptionsPrice { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public short? Status { get; set; }

        public virtual Subscriptions Subscriptions { get; set; }
        public virtual Wallet Wallet { get; set; }
    }
}
