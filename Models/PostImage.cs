using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("POST_IMAGE")]
    public partial class PostImage
    {
        [Key]
        [Column("IMAGE_ID")]
        [StringLength(20)]
        public string ImageId { get; set; }
        [Key]
        [Column("POST_ID")]
        [StringLength(12)]
        public string PostId { get; set; }
        [Column("POSITION")]
        [StringLength(10)]
        public string Position { get; set; }

        [ForeignKey(nameof(ImageId))]
        [InverseProperty(nameof(Medium.PostImages))]
        public virtual Medium Image { get; set; }
        [ForeignKey(nameof(PostId))]
        [InverseProperty(nameof(NeedPost.PostImages))]
        public virtual NeedPost Post { get; set; }
    }
}
