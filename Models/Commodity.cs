using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("COMMODITY")]
    public class Commodity
    {
        [Column("ID")]
        public string Id { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("OWNER_ID")]
        public string OwnerId { get; set; }
        [Column("VIDEO_ID")]
        public string VideoId { get; set; }
        [Column("CONDITION")]
        public string Condition { get; set; }
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        [Column("PRICE")]
        public int Price { get; set; }
        [Column("STOCK")]
        public int Stock { get; set; }
        [Column("FOR_RENT")]
        public int ForRent { get; set; }
        [Column("CLICKS")]
        public int Clicks { get; set; }
        [Column("LIKES")]
        public int Likes { get; set; }
        [Column("POPULARITY")]
        public int Popularity { get; set; }
        [Column("CLASSIFICATION")]
        public int Classification { get; set; }
        [Column("UNIT")]
        public string Unit { get; set; }
    }
}