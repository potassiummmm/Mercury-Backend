using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
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

        public string SchoolId { get; set; }
        public string Nickname { get; set; }
        public string RealName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Major { get; set; }
        public byte? Credit { get; set; }
        public string Role { get; set; }
        public byte Grade { get; set; }
        public string Brief { get; set; }
        public string AvatarId { get; set; }

        public virtual Medium Avatar { get; set; }
        public virtual RoleState RoleNavigation { get; set; }
        public virtual ICollection<ChatRecord> ChatRecordReceivers { get; set; }
        public virtual ICollection<ChatRecord> ChatRecordSenders { get; set; }
        public virtual ICollection<Commodity> Commodities { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<NeedPost> NeedPosts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ReportUser> ReportUserInformants { get; set; }
        public virtual ICollection<ReportUser> ReportUserReporters { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<UserBanned> UserBannedAdmins { get; set; }
        public virtual ICollection<UserBanned> UserBannedOriginRoleNavigations { get; set; }
        public virtual ICollection<UserBanned> UserBannedUsers { get; set; }
        public virtual ICollection<View> Views { get; set; }
    }
}
