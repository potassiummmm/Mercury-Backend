using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("SHOPPING_CART")]
    public class ShoppingCart
    {
        [Column("COMMODITY_ID")]
        public string CommodityId { get; set; }
        [Column("USER_ID")]
        public string UserId { get; set; }
        [Column("COUNT")]
        public string Count { get; set; }
        [Column("ADD_TIME")]
        public long AddTime { get; set; }
    }
}