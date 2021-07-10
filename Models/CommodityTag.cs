using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("COMMODITY_TAG")]
    public partial class CommodityTag
    {
        [Key]
        [Column("COMMODITY_ID")]
        [StringLength(12)]
        public string CommodityId { get; set; }
        [Key]
        [Column("TAG")]
        [StringLength(30)]
        public string Tag { get; set; }

        [ForeignKey(nameof(CommodityId))]
        [InverseProperty("CommodityTags")]
        public virtual Commodity Commodity { get; set; }
    }
}
