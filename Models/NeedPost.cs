using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("NEED_POST")]
    public class NeedPost
    {
        [Column("ID")]
        public string Id { get; set; }
        [Column("SENDER_ID")]
        public string SenderId { get; set; }
        [Column("TITLE")]
        public string Title { get; set; }
        [Column("CONTENT")]
        public string Content { get; set; }
        [Column("SEND_TIME")]
        public long SendTime { get; set; }
    }
}