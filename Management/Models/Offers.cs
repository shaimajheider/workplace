using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Offers
    {
        public Offers()
        {
            Subscriptions = new HashSet<Subscriptions>();
        }

        public long Id { get; set; }
        public long? CompaniesRoomId { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public short? Target { get; set; }
        public short? LessLenth { get; set; }
        public short? LenthType { get; set; }
        public short? MaxLenth { get; set; }
        public int? Price { get; set; }
        public int? BookingValue { get; set; }
        public int? InitialPaymentPrice { get; set; }
        public short? LastPaymentBefore { get; set; }
        public long? AcceptedBy { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public DateTime? RejectedOn { get; set; }
        public long? RejectedBy { get; set; }
        public string RejectedResone { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual CompaniesRooms CompaniesRoom { get; set; }
        public virtual ICollection<Subscriptions> Subscriptions { get; set; }
    }
}
