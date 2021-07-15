using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("ORDER")]
    public partial class Order
    {
        public Order()
        {
            Ratings = new HashSet<Rating>();
        }

        [Key]
        [Column("ID")]
        [StringLength(20)]
        public string Id { get; set; }
        [Column("BUYER_ID")]
        [StringLength(10)]
        public string BuyerId { get; set; }
        [Column("COMMODITY_ID")]
        [StringLength(12)]
        public string CommodityId { get; set; }
        [Column("COUNT")]
        public byte? Count { get; set; }
        [Column("TIME")]
        public DateTime? Time { get; set; }
        [Required]
        [Column("LOCATION")]
        [StringLength(100)]
        public string Location { get; set; }
        [Column("RETURN_TIME")]
        public DateTime ReturnTime { get; set; }
        [Column("RETURN_LOCATION")]
        [StringLength(100)]
        public string ReturnLocation { get; set; }
        [Column("STATUS")]
        [StringLength(10)]
        public string Status { get; set; }

        [ForeignKey(nameof(BuyerId))]
        [InverseProperty(nameof(SchoolUser.Orders))]
        public virtual SchoolUser Buyer { get; set; }
        [ForeignKey(nameof(CommodityId))]
        [InverseProperty("Orders")]
        public virtual Commodity Commodity { get; set; }
        [InverseProperty(nameof(Rating.Order))]
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
