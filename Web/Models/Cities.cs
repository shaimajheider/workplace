using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Cities
    {
        public Cities()
        {
            Applications = new HashSet<Applications>();
            Facilities = new HashSet<Facilities>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual ICollection<Applications> Applications { get; set; }
        public virtual ICollection<Facilities> Facilities { get; set; }
    }
}
