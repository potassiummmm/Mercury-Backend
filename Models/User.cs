using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    public class User
    {
        public String Id { get; set; }
        public String NickName { get; set; }
        public String RealName { get; set; }
        public String Phone { get; set; }
        public String Password { get; set; }
        public String Major { get; set; }
        public int Credit { get; set; }
        public String Role { get; set; }
        public int Grade { get; set; }
        public String Brief { get; set; }
        public String AvatarId { get; set; }
    }
}
