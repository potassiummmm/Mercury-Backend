using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("COMMODITY")]
    public partial class Commodity
    {
        public Commodity()
        {
            CommodityImages = new HashSet<CommodityImage>();
            CommodityTags = new HashSet<CommodityTag>();
            LikesNavigation = new HashSet<Like>();
            Orders = new HashSet<Order>();
            ShoppingCarts = new HashSet<ShoppingCart>();
            Views = new HashSet<View>();
        }

        [Key]
        [Column("ID")]
        [StringLength(12)]
        public string Id { get; set; }
        // [Key]
        [Column("OWNER_ID")]
        [StringLength(10)]
        public string OwnerId { get; set; }
        [Column("VIDEO_ID")]
        [StringLength(20)]
        public string VideoId { get; set; }
        [Column("CONDITION")]
        [StringLength(20)]
        public string Condition { get; set; }
        [Column("PRICE", TypeName = "NUMBER(5,2)")]
        public decimal? Price { get; set; }
        [Column("STOCK")]
        public byte? Stock { get; set; }
        [Column("FOR_RENT")]
        public bool? ForRent { get; set; }
        [Column("CLICKS", TypeName = "NUMBER(20)")]
        public decimal? Clicks { get; set; }
        [Column("LIKES", TypeName = "NUMBER(20)")]
        public decimal? Likes { get; set; }
        [Column("POPULARITY")]
        public byte? Popularity { get; set; }
        [Column("CLASSIFICATION")]
        public byte? Classification { get; set; }
        [Column("UNIT")]
        [StringLength(10)]
        public string Unit { get; set; }
        [Column("NAME")]
        [StringLength(60)]
        public string Name { get; set; }

        [ForeignKey(nameof(Classification))]
        [InverseProperty("Commodities")]
        public virtual Classification ClassificationNavigation { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(SchoolUser.Commodities))]
        public virtual SchoolUser Owner { get; set; }
        [ForeignKey(nameof(VideoId))]
        [InverseProperty(nameof(Medium.Commodities))]
        public virtual Medium Video { get; set; }
        [InverseProperty(nameof(CommodityImage.Commodity))]
        public virtual ICollection<CommodityImage> CommodityImages { get; set; }
        [InverseProperty(nameof(CommodityTag.Commodity))]
        public virtual ICollection<CommodityTag> CommodityTags { get; set; }
        [InverseProperty(nameof(Like.Commodity))]
        public virtual ICollection<Like> LikesNavigation { get; set; }
        [InverseProperty(nameof(Order.Commodity))]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty(nameof(ShoppingCart.Commodity))]
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        [InverseProperty(nameof(View.Commodity))]
        public virtual ICollection<View> Views { get; set; }
    }
}
