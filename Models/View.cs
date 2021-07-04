using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class View
    {
        public string CommodityId { get; set; }
        public DateTime? Time { get; set; }
        public string UserId { get; set; }

        public virtual Commodity Commodity { get; set; }
        public virtual SchoolUser User { get; set; }
    }
}
