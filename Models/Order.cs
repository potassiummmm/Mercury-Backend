using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("ORDER")]
    public class Order
    {
        [Column("ID")]
        public string Id { get; set; }
        [Column("BUYER_ID")]
        public string BuyerId { get; set; }
        [Column("COMMODITY_ID")]
        public string CommodityId { get; set; }
        [Column("COUNT")]
        public int Count { get; set; }
        [Column("TIME")]
        public long Time { get; set; }
        [Column("STATUS")]
        public char Status { get; set; }
        [Column("LOCATION")]
        public string Location { get; set; }
        [Column("RETURN_TIME")]
        public long ReturnTime { get; set; }
        [Column("RETURN_LOCATION")]
        public string ReturnLocation { get; set; }
    }
}