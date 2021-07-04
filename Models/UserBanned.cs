using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("USER_BANNED")]
    public class UserBanned
    {
        [Column("USER_ID")]
        public string UserId { get; set; }
        [Column("ADMIN_ID")]
        public string AdminId { get; set; }
        [Column("CASE_ID")]
        public string CaseId { get; set; }
        [Column("TILL_TIME")]
        public long TillTime { get; set; }
        [Column("HANDLE_TIME")]
        public long HandleTime { get; set; }
        [Column("ORIGIN_ROLE")]
        public string OriginRole { get; set; }
    }
}