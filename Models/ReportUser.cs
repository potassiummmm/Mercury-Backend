using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class ReportUser
    {
        public string ReporterId { get; set; }
        public string InformantId { get; set; }
        public DateTime? Time { get; set; }
        public string Status { get; set; }

        public virtual SchoolUser Informant { get; set; }
        public virtual SchoolUser Reporter { get; set; }
    }
}