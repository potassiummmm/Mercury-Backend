using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Keyless]
    [Table("MESSAGE")]
    public partial class Message
    {
        [Column("CONTENT")]
        [StringLength(20)]
        public string Content { get; set; }
    }
}
