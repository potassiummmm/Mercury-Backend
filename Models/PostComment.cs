using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class PostComment
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public DateTime? Time { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }

        public virtual NeedPost Post { get; set; }
        public virtual SchoolUser Sender { get; set; }
    }
}