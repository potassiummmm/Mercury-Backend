using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mercury_Backend.Models
{
    public class SimplifiedOrder
    {
        public string Id { get; set; }
        public string CommodityName { get; set; }
        public string BuyerId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }
        public string Cover { get; set; }

        public SimplifiedOrder(string id, string commodityName
        , string buyerId, decimal price, int count, string status, string cover)
        {
            Id = id;
            CommodityName = commodityName;
            BuyerId = buyerId;
            Price = price;
            Count = count;
            Status = status;
            Cover = cover;
        }
    }
}
