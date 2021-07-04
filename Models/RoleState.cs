using System.ComponentModel.DataAnnotations.Schema;

namespace Mercury_Backend.Models
{
    [Table("ROLE_STATE")]
    public class RoleState
    {
        [Column("ROLE_NAME")]
        public string RoleName { get; set; }
        [Column("CAN_BAN")]
        public int CanBan { get; set; }
        [Column("CAN_POST")]
        public int CanPost { get; set; }
        [Column("CAN_PUBLISH")]
        public int CanPublish { get; set; }
        [Column("CAN_TRADE")]
        public int CanTrade { get; set; }
        [Column("CAN_COMMENT")]
        public int CanComment { get; set; }
        [Column("CAN_CHAT")]
        public int CanChat { get; set; }
        [Column("CAN_LOGIN")]
        public int CanLogin { get; set; }
    }
}