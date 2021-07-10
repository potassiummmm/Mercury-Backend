using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("VIEW")]
    public partial class View
    {
        [Key]
        [Column("COMMODITY_ID")]
        [StringLength(12)]
        public string CommodityId { get; set; }
        [Column("TIME")]
        public DateTime? Time { get; set; }
        [Key]
        [Column("USER_ID")]
        [StringLength(10)]
        public string UserId { get; set; }

        [ForeignKey(nameof(CommodityId))]
        [InverseProperty("Views")]
        public virtual Commodity Commodity { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SchoolUser.Views))]
        public virtual SchoolUser User { get; set; }
    }
}
