using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("USER_BANNED")]
    public partial class UserBanned
    {
        [Column("USER_ID")]
        [StringLength(10)]
        public string UserId { get; set; }
        [Column("ADMIN_ID")]
        [StringLength(10)]
        public string AdminId { get; set; }
        [Key]
        [Column("CASE_ID")]
        [StringLength(20)]
        public string CaseId { get; set; }
        [Column("TILL_TIME")]
        public DateTime? TillTime { get; set; }
        [Column("HANDLE_TIME")]
        public DateTime? HandleTime { get; set; }
        [Column("ORIGIN_ROLE")]
        [StringLength(15)]
        public string OriginRole { get; set; }

        [ForeignKey(nameof(AdminId))]
        [InverseProperty(nameof(SchoolUser.UserBannedAdmins))]
        public virtual SchoolUser Admin { get; set; }
        [ForeignKey(nameof(OriginRole))]
        [InverseProperty(nameof(SchoolUser.UserBannedOriginRoleNavigations))]
        public virtual SchoolUser OriginRoleNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SchoolUser.UserBannedUsers))]
        public virtual SchoolUser User { get; set; }
    }
}
