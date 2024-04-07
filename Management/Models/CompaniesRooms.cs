using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class CompaniesRooms
    {
        public CompaniesRooms()
        {
            ClassRoomReservations = new HashSet<ClassRoomReservations>();
            CompainesRoomAttachments = new HashSet<CompainesRoomAttachments>();
            Offers = new HashSet<Offers>();
        }

        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public short? Type { get; set; }
        public string Discriptions { get; set; }
        public string Notes { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual Companies Company { get; set; }
        public virtual ICollection<ClassRoomReservations> ClassRoomReservations { get; set; }
        public virtual ICollection<CompainesRoomAttachments> CompainesRoomAttachments { get; set; }
        public virtual ICollection<Offers> Offers { get; set; }
    }
}
