using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("CHAT_RECORD")]
    public partial class ChatRecord
    {
        [Key]
        [Column("SENDER_ID")]
        [StringLength(10)]
        public string SenderId { get; set; }
        [Key]
        [Column("RECEIVER_ID")]
        [StringLength(10)]
        public string ReceiverId { get; set; }
        [Key]
        [Column("INDEX")]
        public byte Index { get; set; }
        [Column("CONTENT")]
        [StringLength(2000)]
        public string Content { get; set; }
        [Required]
        [Column("MEDIA_ID")]
        [StringLength(20)]
        public string MediaId { get; set; }
        [Column("TIME")]
        public DateTime? Time { get; set; }
        [Column("STATUS")]
        [StringLength(1)]
        public string Status { get; set; }

        [ForeignKey(nameof(MediaId))]
        [InverseProperty(nameof(Medium.ChatRecords))]
        public virtual Medium Media { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        [InverseProperty(nameof(SchoolUser.ChatRecordReceivers))]
        public virtual SchoolUser Receiver { get; set; }
        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(SchoolUser.ChatRecordSenders))]
        public virtual SchoolUser Sender { get; set; }
    }
}
