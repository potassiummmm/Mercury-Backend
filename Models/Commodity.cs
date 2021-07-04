using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
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

        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string VideoId { get; set; }
        public string Condition { get; set; }
        public decimal? Price { get; set; }
        public byte? Stock { get; set; }
        public bool? ForRent { get; set; }
        public decimal? Clicks { get; set; }
        public decimal? Likes { get; set; }
        public byte? Popularity { get; set; }
        public byte? Classification { get; set; }
        public string Unit { get; set; }

        public virtual Classification ClassificationNavigation { get; set; }
        public virtual SchoolUser Owner { get; set; }
        public virtual Medium Video { get; set; }
        public virtual ICollection<CommodityImage> CommodityImages { get; set; }
        public virtual ICollection<CommodityTag> CommodityTags { get; set; }
        public virtual ICollection<Like> LikesNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<View> Views { get; set; }
    }
}
