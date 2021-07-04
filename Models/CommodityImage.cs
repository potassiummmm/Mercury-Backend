using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    public class CommodityImage
    {
        [Column("COMMODITY_ID")]
        public string CommodityId { get; set; }
        [Column("IMAGE_ID")]
        public string ImageId { get; set; }
        [Column("ORDER")]
        public int Order { get; set; }
    }
}