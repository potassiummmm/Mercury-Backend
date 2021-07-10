using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("SHOPPING_CART")]
    public partial class ShoppingCart
    {
        [Key]
        [Column("COMMODITY_ID")]
        [StringLength(12)]
        public string CommodityId { get; set; }
        [Key]
        [Column("USER_ID")]
        [StringLength(10)]
        public string UserId { get; set; }
        [Column("COUNT")]
        public byte? Count { get; set; }
        [Column("ADD_TIME")]
        public DateTime? AddTime { get; set; }

        [ForeignKey(nameof(CommodityId))]
        [InverseProperty("ShoppingCarts")]
        public virtual Commodity Commodity { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SchoolUser.ShoppingCarts))]
        public virtual SchoolUser User { get; set; }
    }
}
