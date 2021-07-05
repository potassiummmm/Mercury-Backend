using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("MEDIA")]
    public class Media
    {
        [Column("ID")] 
        public string Id { get; set; }
        [Column("TYPE")]
        public string Type { get; set; }
        [Column("PATH")]
        public string Path { get; set; }
    }
}