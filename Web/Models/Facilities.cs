using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Facilities
    {
        public Facilities()
        {
            Applications = new HashSet<Applications>();
        }

        public short Id { get; set; }
        public short? CityId { get; set; }
        public string Name { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual Cities City { get; set; }
        public virtual ICollection<Applications> Applications { get; set; }
    }
}
