using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("LIKE")]
    public partial class Like
    {
        [Key]
        [Column("COMMODITY_ID")]
        [StringLength(12)]
        public string CommodityId { get; set; }
        [Key]
        [Column("USER_ID")]
        [StringLength(10)]
        public string UserId { get; set; }

        [ForeignKey(nameof(CommodityId))]
        [InverseProperty("LikesNavigation")]
        public virtual Commodity Commodity { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SchoolUser.Likes))]
        public virtual SchoolUser User { get; set; }
    }
}
