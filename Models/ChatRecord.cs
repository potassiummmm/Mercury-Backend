using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class ChatRecord
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public byte Index { get; set; }
        public string Content { get; set; }
        public string MediaId { get; set; }
        public DateTime? Time { get; set; }
        public string Status { get; set; }

        public virtual Medium Media { get; set; }
        public virtual SchoolUser Receiver { get; set; }
        public virtual SchoolUser Sender { get; set; }
    }
}