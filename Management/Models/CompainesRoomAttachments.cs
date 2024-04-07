using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class CompainesRoomAttachments
    {
        public long Id { get; set; }
        public long? CompanyRoomId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }

        public virtual CompaniesRooms CompanyRoom { get; set; }
    }
}
