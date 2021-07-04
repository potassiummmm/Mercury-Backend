using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("SCHOOL_USER")]
    public class User
    {
        [Column("SCHOOL_ID")]
        public String Id { get; set; }
        [Column("NICKNAME")]
        public String NickName { get; set; }
        [Column("REAL_NAME")]
        public String RealName { get; set; }
        [Column("PHONE")]
        public String Phone { get; set; }
        [Column("PASSWORD")]
        public String Password { get; set; }
        [Column("MAJOR")]
        public String Major { get; set; }
        [Column("CREDIT")]
        public int Credit { get; set; }
        [Column("ROLE")]
        public String Role { get; set; }
        [Column("GRADE")]
        public int Grade { get; set; }
        [Column("BRIEF")]
        public String Brief { get; set; }
        [Column("AVATAR_ID")]
        public String AvatarId { get; set; }
    }
}