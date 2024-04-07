using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Applications
    {
        public Applications()
        {
            ApplicationsAttachments = new HashSet<ApplicationsAttachments>();
        }

        public long Id { get; set; }
        public short? ProgramId { get; set; }
        public short? CityId { get; set; }
        public short? FacilityId { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandFatherName { get; set; }
        public string SirName { get; set; }
        public string FullName { get; set; }
        public string Nid { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public short? Gender { get; set; }
        public string DoctorName { get; set; }
        public long? RejectBy { get; set; }
        public DateTime? RejectOn { get; set; }
        public string RejectResone { get; set; }
        public short? Levels { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual Cities City { get; set; }
        public virtual Facilities Facility { get; set; }
        public virtual ICollection<ApplicationsAttachments> ApplicationsAttachments { get; set; }
    }
}
