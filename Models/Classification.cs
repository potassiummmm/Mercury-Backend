using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("CLASSIFICATION")]
    public class Classification
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
    }
}