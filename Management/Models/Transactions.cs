using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Transactions
    {
        public long Id { get; set; }
        public string Operations { get; set; }
        public string Descriptions { get; set; }
        public string Controller { get; set; }
        public string OldObject { get; set; }
        public string NewObject { get; set; }
        public long? ItemId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
