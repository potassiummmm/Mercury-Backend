using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mercury_Backend.Models
{
    public class SimplifiedCommodity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Likes { get; set; }
        public string Cover { get; set; }
        public string SellerId { get; set; }
        public string SellerAvatar { get; set; }

        public SimplifiedCommodity(string id, string name, decimal price, int likes, string cover, string sellerId, string sellerAvatar)
        {
            Id = id;
            Name = name;
            Price = price;
            Likes = likes;
            Cover = cover;
            SellerId = sellerId;
            SellerAvatar = sellerAvatar;
        }
    }
}
