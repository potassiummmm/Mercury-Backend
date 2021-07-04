using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("CHAT_RECORD")]
    public class ChatRecord
    {
        [Column("SENDER_ID")]
        public string SenderId { get; set; }
        [Column("RECEIVER_ID")]
        public string ReceiverId { get; set; }
        [Column("INDEX")]
        public int Index { get; set; }
        [Column("CONTENT")]
        public string Content { get; set; }
        [Column("MEDIA_ID")]
        public string MediaId { get; set; }
        [Column("TIME")]
        public long Time { get; set; }
        [Column("STATUS")]
        public char Status { get; set; }
    }
}