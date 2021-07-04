using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class NeedPost
    {
        public NeedPost()
        {
            PostComments = new HashSet<PostComment>();
            PostImages = new HashSet<PostImage>();
        }

        public string Id { get; set; }
        public string SenderId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual SchoolUser Sender { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
    }
}
