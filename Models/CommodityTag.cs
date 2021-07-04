using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class CommodityTag
    {
        public string CommodityId { get; set; }
        public string Tag { get; set; }

        public virtual Commodity Commodity { get; set; }
    }
}
