using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Cosmos.Query.Internal;

namespace Mercury_Backend.Models
{
    [Table("VIEW")]
    public class View
    {
        [Column("COMMODITY_ID")]
        public string CommodityId { get; set; }
        [Column("TIME")]
        public long Time { get; set; }
        [Column("USER_ID")]
        public string UserId { get; set; }
    }
}