using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mercury_Backend.Models
{
    [Table("SCHOOL_USER")]
    [Index(nameof(Nickname), Name = "SCHOOL_USER_NICKNAME_UINDEX", IsUnique = true)]
    [Index(nameof(Phone), Name = "SCHOOL_USER_PHONE_UINDEX", IsUnique = true)]
    public partial class SchoolUser
    {
        public SchoolUser()
        {
            ChatRecordReceivers = new HashSet<ChatRecord>();
            ChatRecordSenders = new HashSet<ChatRecord>();
            Commodities = new HashSet<Commodity>();
            Likes = new HashSet<Like>();
            NeedPosts = new HashSet<NeedPost>();
            Orders = new HashSet<Order>();
            PostComments = new HashSet<PostComment>();
            Ratings = new HashSet<Rating>();
            ReportUserInformants = new HashSet<ReportUser>();
            ReportUserReporters = new HashSet<ReportUser>();
            ShoppingCarts = new HashSet<ShoppingCart>();
            UserBannedAdmins = new HashSet<UserBanned>();
            UserBannedOriginRoleNavigations = new HashSet<UserBanned>();
            UserBannedUsers = new HashSet<UserBanned>();
            Views = new HashSet<View>();
        }

        [Key]
        [Column("SCHOOL_ID")]
        [StringLength(10)]
        public string SchoolId { get; set; }
        [Column("NICKNAME")]
        [StringLength(30)]
        public string Nickname { get; set; }
        [Column("REAL_NAME")]
        [StringLength(30)]
        public string RealName { get; set; }
        [Column("PHONE")]
        [StringLength(11)]
        public string Phone { get; set; }
        [Column("PASSWORD")]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [Column("MAJOR")]
        [StringLength(40)]
        public string Major { get; set; }
        [Column("CREDIT")]
        public byte? Credit { get; set; }
        [Column("ROLE")]
        [StringLength(15)]
        public string Role { get; set; }
        [Column("GRADE")]
        public byte Grade { get; set; }
        [Required]
        [Column("BRIEF")]
        [StringLength(150)]
        public string Brief { get; set; }
        [Required]
        [Column("AVATAR_ID")]
        [StringLength(20)]
        public string AvatarId { get; set; }

        [ForeignKey(nameof(AvatarId))]
        [InverseProperty(nameof(Medium.SchoolUsers))]
        public virtual Medium Avatar { get; set; }
        [ForeignKey(nameof(Role))]
        [InverseProperty(nameof(RoleState.SchoolUsers))]
        public virtual RoleState RoleNavigation { get; set; }
        [InverseProperty(nameof(ChatRecord.Receiver))]
        public virtual ICollection<ChatRecord> ChatRecordReceivers { get; set; }
        [InverseProperty(nameof(ChatRecord.Sender))]
        public virtual ICollection<ChatRecord> ChatRecordSenders { get; set; }
        [InverseProperty(nameof(Commodity.Owner))]
        public virtual ICollection<Commodity> Commodities { get; set; }
        [InverseProperty(nameof(Like.User))]
        public virtual ICollection<Like> Likes { get; set; }
        [InverseProperty(nameof(NeedPost.Sender))]
        public virtual ICollection<NeedPost> NeedPosts { get; set; }
        [InverseProperty(nameof(Order.Buyer))]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty(nameof(PostComment.Sender))]
        public virtual ICollection<PostComment> PostComments { get; set; }
        [InverseProperty(nameof(Rating.User))]
        public virtual ICollection<Rating> Ratings { get; set; }
        [InverseProperty(nameof(ReportUser.Informant))]
        public virtual ICollection<ReportUser> ReportUserInformants { get; set; }
        [InverseProperty(nameof(ReportUser.Reporter))]
        public virtual ICollection<ReportUser> ReportUserReporters { get; set; }
        [InverseProperty(nameof(ShoppingCart.User))]
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        [InverseProperty(nameof(UserBanned.Admin))]
        public virtual ICollection<UserBanned> UserBannedAdmins { get; set; }
        [InverseProperty(nameof(UserBanned.OriginRoleNavigation))]
        public virtual ICollection<UserBanned> UserBannedOriginRoleNavigations { get; set; }
        [InverseProperty(nameof(UserBanned.User))]
        public virtual ICollection<UserBanned> UserBannedUsers { get; set; }
        [InverseProperty(nameof(View.User))]
        public virtual ICollection<View> Views { get; set; }
    }
}
