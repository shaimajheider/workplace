using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Users
    {
        public long Id { get; set; }
        public short? PortId { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public short? UserType { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public short? LoginTryAttempts { get; set; }
        public DateTime? LoginTryAttemptDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Status { get; set; }
    }
}
