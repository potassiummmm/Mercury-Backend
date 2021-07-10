using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("REPORT_USER")]
    public partial class ReportUser
    {
        [Key]
        [Column("REPORTER_ID")]
        [StringLength(10)]
        public string ReporterId { get; set; }
        [Key]
        [Column("INFORMANT_ID")]
        [StringLength(10)]
        public string InformantId { get; set; }
        [Column("TIME")]
        public DateTime? Time { get; set; }
        [Column("STATUS")]
        [StringLength(1)]
        public string Status { get; set; }

        [ForeignKey(nameof(InformantId))]
        [InverseProperty(nameof(SchoolUser.ReportUserInformants))]
        public virtual SchoolUser Informant { get; set; }
        [ForeignKey(nameof(ReporterId))]
        [InverseProperty(nameof(SchoolUser.ReportUserReporters))]
        public virtual SchoolUser Reporter { get; set; }
    }
}
