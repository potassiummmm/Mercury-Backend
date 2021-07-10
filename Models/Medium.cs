using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("MEDIA")]
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

        [Key]
        [Column("ID")]
        [StringLength(20)]
        public string Id { get; set; }
        [Column("TYPE")]
        [StringLength(10)]
        public string Type { get; set; }
        [Column("PATH")]
        [StringLength(100)]
        public string Path { get; set; }

        [InverseProperty(nameof(ChatRecord.Media))]
        public virtual ICollection<ChatRecord> ChatRecords { get; set; }
        [InverseProperty(nameof(Commodity.Video))]
        public virtual ICollection<Commodity> Commodities { get; set; }
        [InverseProperty(nameof(CommodityImage.Image))]
        public virtual ICollection<CommodityImage> CommodityImages { get; set; }
        [InverseProperty(nameof(PostImage.Image))]
        public virtual ICollection<PostImage> PostImages { get; set; }
        [InverseProperty(nameof(SchoolUser.Avatar))]
        public virtual ICollection<SchoolUser> SchoolUsers { get; set; }
    }
}
