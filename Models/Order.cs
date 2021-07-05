using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class Order
    {
        public Order()
        {
            Ratings = new HashSet<Rating>();
        }

        public string Id { get; set; }
        public string BuyerId { get; set; }
        public string CommodityId { get; set; }
        public byte? Count { get; set; }
        public DateTime? Time { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public DateTime ReturnTime { get; set; }
        public string ReturnLocation { get; set; }

        public virtual SchoolUser Buyer { get; set; }
        public virtual Commodity Commodity { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}