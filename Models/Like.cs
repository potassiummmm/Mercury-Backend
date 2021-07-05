using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("LIKE")]
    public class Like
    {
        [Column("COMMODITY_ID")]
        public string CommodityId { get; set; }
        [Column("USER_ID")]
        public string UserId { get; set; }
    }
}
