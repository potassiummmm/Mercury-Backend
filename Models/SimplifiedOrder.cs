using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mercury_Backend.Models
{
    public class SimplifiedOrder
    {
        public string Id { get; set; }
        public string BuyerId { get; set; }
        public SimplifiedCommodity Commodity { get; set; }

    }
}
