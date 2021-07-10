using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("ROLE_STATE")]
    public partial class RoleState
    {
        public RoleState()
        {
            SchoolUsers = new HashSet<SchoolUser>();
        }

        [Key]
        [Column("ROLE_NAME")]
        [StringLength(15)]
        public string RoleName { get; set; }
        [Column("CAN_BAN")]
        public bool? CanBan { get; set; }
        [Column("CAN_POST")]
        public bool? CanPost { get; set; }
        [Column("CAN_PUBLISH")]
        public bool? CanPublish { get; set; }
        [Column("CAN_TRADE")]
        public bool? CanTrade { get; set; }
        [Column("CAN_COMMENT")]
        public bool? CanComment { get; set; }
        [Column("CAN_CHAT")]
        public bool? CanChat { get; set; }
        [Column("CAN_LOGIN")]
        public bool? CanLogin { get; set; }

        [InverseProperty(nameof(SchoolUser.RoleNavigation))]
        public virtual ICollection<SchoolUser> SchoolUsers { get; set; }
    }
}
