using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class ClassRoomReservations
    {
        public long Id { get; set; }
        public long? CompaniesRoomId { get; set; }
        public long? SubscriptionsId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CountOfDay { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual CompaniesRooms CompaniesRoom { get; set; }
        public virtual Subscriptions Subscriptions { get; set; }
    }
}
