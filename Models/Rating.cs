using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("RATING")]
    public partial class Rating
    {
        [Key]
        [Column("USER_ID")]
        [StringLength(10)]
        public string UserId { get; set; }
        [Key]
        [Column("ORDER_ID")]
        [StringLength(20)]
        public string OrderId { get; set; }
        [Column("IS_BUYER")]
        public bool? IsBuyer { get; set; }
        [Column("RATING")]
        public byte? Rating1 { get; set; }
        [Required]
        [Column("COMMENT")]
        [StringLength(500)]
        public string Comment { get; set; }
        [Column("TIME")]
        public DateTime? Time { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty("Ratings")]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SchoolUser.Ratings))]
        public virtual SchoolUser User { get; set; }
    }
}
