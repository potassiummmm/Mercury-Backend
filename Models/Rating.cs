using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("RATING")]
    public class Rating
    {
        [Column("USER_ID")]
        public string UserId { get; set; }
        [Column("ORDER_ID")]
        public string OrderId { get; set; }
        [Column("IS_BUYER")]
        public int IsBuyer { get; set; }
        [Column("RATING")]
        public int RatingValue { get; set; }
        [Column("COMMENT")]
        public string Comment { get; set; }
        [Column("TIME")]
        public long Time { get; set; }
    }
}