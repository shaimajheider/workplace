using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class CompaniesSchedule
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public short? Day { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual Companies Company { get; set; }
    }
}
