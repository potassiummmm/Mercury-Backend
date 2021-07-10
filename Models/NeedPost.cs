using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("NEED_POST")]
    public partial class NeedPost
    {
        public NeedPost()
        {
            PostComments = new HashSet<PostComment>();
            PostImages = new HashSet<PostImage>();
        }

        [Key]
        [Column("ID")]
        [StringLength(12)]
        public string Id { get; set; }
        [Column("SENDER_ID")]
        [StringLength(10)]
        public string SenderId { get; set; }
        [Column("TITLE")]
        [StringLength(50)]
        public string Title { get; set; }
        [Column("CONTENT")]
        [StringLength(2000)]
        public string Content { get; set; }
        [Column("TIME")]
        public DateTime? Time { get; set; }

        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(SchoolUser.NeedPosts))]
        public virtual SchoolUser Sender { get; set; }
        [InverseProperty(nameof(PostComment.Post))]
        public virtual ICollection<PostComment> PostComments { get; set; }
        [InverseProperty(nameof(PostImage.Post))]
        public virtual ICollection<PostImage> PostImages { get; set; }
    }
}
