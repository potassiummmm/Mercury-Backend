using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("REPORT_UER")]
    public class ReportUser
    {
        [Column("REPORTER_ID")]
        public string ReporterId { get; set; }
        [Column("INFORMANT_ID")]
        public string InformantId { get; set; }
        [Column("TIME")]
        public long Time { get; set; }
        [Column("COMMENT")]
        public string Comment { get; set; }
        [Column("STATUS")]
        public char Status { get; set; }
    }
}