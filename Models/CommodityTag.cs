using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("COMMODITY_TAG")]
    public class CommodityTag
    {
        [Column("COMMODITY_ID")]
        public string CommodityId { get; set; }
        [Column("TAG")]
        public string Tag { get; set; }
    }
}
