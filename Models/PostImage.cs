using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("POST_IMAGE")]
    public class PostImage
    {
        [Column("IMAGE_ID")]
        public string ImageId { get; set; }
        [Column("POST_ID")]
        public string PostId { get; set; }
        [Column("POSITION")]
        public string Position { get; set; }
    }
}