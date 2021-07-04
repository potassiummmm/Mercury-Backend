using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class CommodityImage
    {
        public string CommodityId { get; set; }
        public string ImageId { get; set; }
        public bool? Order { get; set; }

        public virtual Commodity Commodity { get; set; }
        public virtual Medium Image { get; set; }
    }
}
