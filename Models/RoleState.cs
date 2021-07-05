using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class RoleState
    {
        public RoleState()
        {
            SchoolUsers = new HashSet<SchoolUser>();
        }

        public string RoleName { get; set; }
        public bool? CanBan { get; set; }
        public bool? CanPost { get; set; }
        public bool? CanPublish { get; set; }
        public bool? CanTrade { get; set; }
        public bool? CanComment { get; set; }
        public bool? CanChat { get; set; }
        public bool? CanLogin { get; set; }

        public virtual ICollection<SchoolUser> SchoolUsers { get; set; }
    }
}