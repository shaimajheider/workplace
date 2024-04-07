using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Subscriptions
    {
        public Subscriptions()
        {
            ClassRoomReservations = new HashSet<ClassRoomReservations>();
            WalletPurchases = new HashSet<WalletPurchases>();
        }

        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? OfferId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? PaiedValue { get; set; }
        public int? RemindValue { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Level { get; set; }
        public short? Status { get; set; }

        public virtual Offers Offer { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<ClassRoomReservations> ClassRoomReservations { get; set; }
        public virtual ICollection<WalletPurchases> WalletPurchases { get; set; }
    }
}
