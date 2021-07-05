using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class Rating
    {
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public bool? IsBuyer { get; set; }
        public byte? Rating1 { get; set; }
        public string Comment { get; set; }
        public DateTime? Time { get; set; }

        public virtual Order Order { get; set; }
        public virtual SchoolUser User { get; set; }
    }
}