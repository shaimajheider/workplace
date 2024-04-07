using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class UserSuspends
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string Resone { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual Users User { get; set; }
    }
}
