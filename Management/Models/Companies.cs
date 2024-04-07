using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Companies
    {
        public Companies()
        {
            CompainesAttachmenets = new HashSet<CompainesAttachmenets>();
            CompaniesRooms = new HashSet<CompaniesRooms>();
            CompaniesSchedule = new HashSet<CompaniesSchedule>();
        }

        public long Id { get; set; }
        public long? UserId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string LocationDescriptions { get; set; }
        public string LocationLink { get; set; }
        public int? FloorCount { get; set; }
        public int? ClassRoomCount { get; set; }
        public int? MeetingRoomCount { get; set; }
        public int? TraningRoomCount { get; set; }
        public int? PrivateRoomCount { get; set; }
        public int? OfficeCount { get; set; }
        public string Notes { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<CompainesAttachmenets> CompainesAttachmenets { get; set; }
        public virtual ICollection<CompaniesRooms> CompaniesRooms { get; set; }
        public virtual ICollection<CompaniesSchedule> CompaniesSchedule { get; set; }
    }
}
