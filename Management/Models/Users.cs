using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Users
    {
        public Users()
        {
            Companies = new HashSet<Companies>();
            Subscriptions = new HashSet<Subscriptions>();
            UserSuspends = new HashSet<UserSuspends>();
            Wallet = new HashSet<Wallet>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public short? UserType { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public short? Gender { get; set; }
        public DateTime? LoginTryAttemptDate { get; set; }
        public short? LoginTryAttempts { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public short? Level { get; set; }
        public short? Status { get; set; }

        public virtual ICollection<Companies> Companies { get; set; }
        public virtual ICollection<Subscriptions> Subscriptions { get; set; }
        public virtual ICollection<UserSuspends> UserSuspends { get; set; }
        public virtual ICollection<Wallet> Wallet { get; set; }
    }
}
