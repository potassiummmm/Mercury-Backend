using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class Medium
    {
        public Medium()
        {
            ChatRecords = new HashSet<ChatRecord>();
            Commodities = new HashSet<Commodity>();
            CommodityImages = new HashSet<CommodityImage>();
            PostImages = new HashSet<PostImage>();
            SchoolUsers = new HashSet<SchoolUser>();
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }

        public virtual ICollection<ChatRecord> ChatRecords { get; set; }
        public virtual ICollection<Commodity> Commodities { get; set; }
        public virtual ICollection<CommodityImage> CommodityImages { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
        public virtual ICollection<SchoolUser> SchoolUsers { get; set; }
    }
}
