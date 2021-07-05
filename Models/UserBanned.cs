using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class UserBanned
    {
        public string UserId { get; set; }
        public string AdminId { get; set; }
        public string CaseId { get; set; }
        public DateTime? TillTime { get; set; }
        public DateTime? HandleTime { get; set; }
        public string OriginRole { get; set; }

        public virtual SchoolUser Admin { get; set; }
        public virtual SchoolUser OriginRoleNavigation { get; set; }
        public virtual SchoolUser User { get; set; }
    }
}