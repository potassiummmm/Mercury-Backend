using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("CLASSIFICATION")]
    [Index(nameof(Name), Name = "CLASSIFICATION_NAME_UINDEX", IsUnique = true)]
    public partial class Classification
    {
        public Classification()
        {
            Commodities = new HashSet<Commodity>();
        }

        [Key]
        [Column("ID")]
        public byte Id { get; set; }
        [Column("NAME")]
        [StringLength(30)]
        public string Name { get; set; }

        [InverseProperty(nameof(Commodity.ClassificationNavigation))]
        public virtual ICollection<Commodity> Commodities { get; set; }
    }
}
