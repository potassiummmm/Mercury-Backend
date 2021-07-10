using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("POST_COMMENT")]
    public partial class PostComment
    {
        [Key]
        [Column("ID")]
        [StringLength(15)]
        public string Id { get; set; }
        [Column("POST_ID")]
        [StringLength(12)]
        public string PostId { get; set; }
        [Column("TIME")]
        public DateTime? Time { get; set; }
        [Column("CONTENT")]
        [StringLength(300)]
        public string Content { get; set; }
        [Column("SENDER_ID")]
        [StringLength(10)]
        public string SenderId { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty(nameof(NeedPost.PostComments))]
        public virtual NeedPost Post { get; set; }
        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(SchoolUser.PostComments))]
        public virtual SchoolUser Sender { get; set; }
    }
}
