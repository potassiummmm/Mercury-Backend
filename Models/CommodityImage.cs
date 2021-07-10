using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("COMMODITY_IMAGE")]
    public partial class CommodityImage
    {
        [Key]
        [Column("COMMODITY_ID")]
        [StringLength(12)]
        public string CommodityId { get; set; }
        [Key]
        [Column("IMAGE_ID")]
        [StringLength(20)]
        public string ImageId { get; set; }
        [Column("ORDER")]
        public bool? Order { get; set; }

        [ForeignKey(nameof(CommodityId))]
        [InverseProperty("CommodityImages")]
        public virtual Commodity Commodity { get; set; }
        [ForeignKey(nameof(ImageId))]
        [InverseProperty(nameof(Medium.CommodityImages))]
        public virtual Medium Image { get; set; }
    }
}
