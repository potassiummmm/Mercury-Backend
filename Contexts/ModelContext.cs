using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Mercury_Backend.Models;

#nullable disable

namespace Mercury_Backend.Contexts
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatRecord> ChatRecords { get; set; }
        public virtual DbSet<Classification> Classifications { get; set; }
        public virtual DbSet<Commodity> Commodities { get; set; }
        public virtual DbSet<CommodityImage> CommodityImages { get; set; }
        public virtual DbSet<CommodityTag> CommodityTags { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Medium> Media { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<NeedPost> NeedPosts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PostComment> PostComments { get; set; }
        public virtual DbSet<PostImage> PostImages { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<ReportUser> ReportUsers { get; set; }
        public virtual DbSet<RoleState> RoleStates { get; set; }
        public virtual DbSet<SchoolUser> SchoolUsers { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<UserBanned> UserBanneds { get; set; }
        public virtual DbSet<View> Views { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("User Id=yuanzhang;Password=123456;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=150.158.185.96)(PORT=1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = xe))); ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("YUANZHANG");

            modelBuilder.Entity<ChatRecord>(entity =>
            {
                entity.HasKey(e => new { e.SenderId, e.ReceiverId, e.Index })
                    .HasName("CHAT_RECORD_PK");

                entity.ToTable("CHAT_RECORD");

                entity.Property(e => e.SenderId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SENDER_ID");

                entity.Property(e => e.ReceiverId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RECEIVER_ID");

                entity.Property(e => e.Index)
                    .HasPrecision(4)
                    .HasColumnName("INDEX");

                entity.Property(e => e.Content)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("CONTENT");

                entity.Property(e => e.MediaId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MEDIA_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .IsFixedLength(true);

                entity.Property(e => e.Time)
                    .HasPrecision(6)
                    .HasColumnName("TIME");

                entity.HasOne(d => d.Media)
                    .WithMany(p => p.ChatRecords)
                    .HasForeignKey(d => d.MediaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CHAT_RECORD_MEDIA_ID_FK");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.ChatRecordReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RECORD_USER_SCHOOL_ID_FK_2");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.ChatRecordSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CHAT_RECORD_USER_SCHOOL_ID_FK");
            });

            modelBuilder.Entity<Classification>(entity =>
            {
                entity.ToTable("CLASSIFICATION");

                entity.HasIndex(e => e.Name, "CLASSIFICATION_NAME_UINDEX")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasPrecision(3)
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<Commodity>(entity =>
            {
                entity.ToTable("COMMODITY");

                entity.Property(e => e.Id)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Classification)
                    .HasPrecision(3)
                    .HasColumnName("CLASSIFICATION");

                entity.Property(e => e.Clicks)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("CLICKS");

                entity.Property(e => e.Condition)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CONDITION");

                entity.Property(e => e.ForRent)
                    .HasPrecision(1)
                    .HasColumnName("FOR_RENT");

                entity.Property(e => e.Likes)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("LIKES");

                entity.Property(e => e.OwnerId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OWNER_ID");

                entity.Property(e => e.Popularity)
                    .HasPrecision(3)
                    .HasColumnName("POPULARITY");

                entity.Property(e => e.Price)
                    .HasColumnType("NUMBER(5,2)")
                    .HasColumnName("PRICE");

                entity.Property(e => e.Stock)
                    .HasPrecision(4)
                    .HasColumnName("STOCK");

                entity.Property(e => e.Unit)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("UNIT");

                entity.Property(e => e.VideoId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("VIDEO_ID")
                    .IsFixedLength(true);

                entity.HasOne(d => d.ClassificationNavigation)
                    .WithMany(p => p.Commodities)
                    .HasForeignKey(d => d.Classification)
                    .HasConstraintName("COMMODITY_CLASSIFICATION_ID_FK");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Commodities)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("COMMODITY_USER_SCHOOL_ID_FK");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.Commodities)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COMMODITY_MEDIA_ID_FK");
            });

            modelBuilder.Entity<CommodityImage>(entity =>
            {
                entity.HasKey(e => new { e.CommodityId, e.ImageId })
                    .HasName("COMMODITY_IMAGE_PK");

                entity.ToTable("COMMODITY_IMAGE");

                entity.Property(e => e.CommodityId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("COMMODITY_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.ImageId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Order)
                    .HasPrecision(1)
                    .HasColumnName("ORDER");

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.CommodityImages)
                    .HasForeignKey(d => d.CommodityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COM_IMAGE_COMMODITY_ID_FK");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.CommodityImages)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COM_IMAGE_MEDIA_ID_FK");
            });

            modelBuilder.Entity<CommodityTag>(entity =>
            {
                entity.HasKey(e => new { e.CommodityId, e.Tag })
                    .HasName("COMMODITY_TAG_PK");

                entity.ToTable("COMMODITY_TAG");

                entity.Property(e => e.CommodityId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("COMMODITY_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Tag)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TAG");

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.CommodityTags)
                    .HasForeignKey(d => d.CommodityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("COMMODITY_TAG_COMMODITY_ID_FK");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => new { e.CommodityId, e.UserId })
                    .HasName("LIKE_PK");

                entity.ToTable("LIKE");

                entity.Property(e => e.CommodityId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("COMMODITY_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.LikesNavigation)
                    .HasForeignKey(d => d.CommodityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LIKE_COMMODITY_ID_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LIKE_USER__FK");
            });

            modelBuilder.Entity<Medium>(entity =>
            {
                entity.ToTable("MEDIA");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Path)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PATH");

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MESSAGE");

                entity.Property(e => e.Content)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CONTENT");
            });

            modelBuilder.Entity<NeedPost>(entity =>
            {
                entity.ToTable("NEED_POST");

                entity.Property(e => e.Id)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Content)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("CONTENT");

                entity.Property(e => e.SenderId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SENDER_ID");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TITLE")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.NeedPosts)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("NEED_POST_USER_SCHOOL_ID_FK");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("ORDER");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.BuyerId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BUYER_ID");

                entity.Property(e => e.CommodityId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("COMMODITY_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Count)
                    .HasPrecision(4)
                    .HasColumnName("COUNT");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.ReturnLocation)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RETURN_LOCATION");

                entity.Property(e => e.ReturnTime)
                    .HasPrecision(6)
                    .HasColumnName("RETURN_TIME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .IsFixedLength(true);

                entity.Property(e => e.Time)
                    .HasPrecision(6)
                    .HasColumnName("TIME");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("ORDER_USER_SCHOOL_ID_FK");

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CommodityId)
                    .HasConstraintName("ORDER_COMMODITY_ID_FK");
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.ToTable("POST_COMMENT");

                entity.Property(e => e.Id)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Content)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("CONTENT");

                entity.Property(e => e.PostId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("POST_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.SenderId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SENDER_ID");

                entity.Property(e => e.Time)
                    .HasPrecision(6)
                    .HasColumnName("TIME");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("POST_COMMENT_NEED_POST_ID_FK");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("POST_COMMENT_USER_SCHOOL_ID_FK");
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.HasKey(e => new { e.ImageId, e.PostId })
                    .HasName("POST_IMAGE_PK");

                entity.ToTable("POST_IMAGE");

                entity.Property(e => e.ImageId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.PostId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("POST_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Position)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("POSITION");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("POST_IMAGE_COM_VIDEO_ID_FK");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("POST_IMAGE_NEED_POST_ID_FK");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.OrderId })
                    .HasName("RATING_PK");

                entity.ToTable("RATING");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.IsBuyer)
                    .HasPrecision(1)
                    .HasColumnName("IS_BUYER");

                entity.Property(e => e.Rating1)
                    .HasPrecision(2)
                    .HasColumnName("RATING");

                entity.Property(e => e.Time)
                    .HasPrecision(6)
                    .HasColumnName("TIME");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RATING_ORDER_ID_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RATING_USER_SCHOOL_ID_FK");
            });

            modelBuilder.Entity<ReportUser>(entity =>
            {
                entity.HasKey(e => new { e.ReporterId, e.InformantId })
                    .HasName("REPORT_USER_PK");

                entity.ToTable("REPORT_USER");

                entity.Property(e => e.ReporterId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REPORTER_ID");

                entity.Property(e => e.InformantId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("INFORMANT_ID");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .IsFixedLength(true);

                entity.Property(e => e.Time)
                    .HasPrecision(6)
                    .HasColumnName("TIME");

                entity.HasOne(d => d.Informant)
                    .WithMany(p => p.ReportUserInformants)
                    .HasForeignKey(d => d.InformantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("REPORT_USER_USER_ID_FK_2");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.ReportUserReporters)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("REPORT_USER_USER_ID_FK");
            });

            modelBuilder.Entity<RoleState>(entity =>
            {
                entity.HasKey(e => e.RoleName)
                    .HasName("ROLE_STATE_PK");

                entity.ToTable("ROLE_STATE");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ROLE_NAME");

                entity.Property(e => e.CanBan)
                    .HasPrecision(1)
                    .HasColumnName("CAN_BAN");

                entity.Property(e => e.CanChat)
                    .HasPrecision(1)
                    .HasColumnName("CAN_CHAT");

                entity.Property(e => e.CanComment)
                    .HasPrecision(1)
                    .HasColumnName("CAN_COMMENT");

                entity.Property(e => e.CanLogin)
                    .HasPrecision(1)
                    .HasColumnName("CAN_LOGIN");

                entity.Property(e => e.CanPost)
                    .HasPrecision(1)
                    .HasColumnName("CAN_POST");

                entity.Property(e => e.CanPublish)
                    .HasPrecision(1)
                    .HasColumnName("CAN_PUBLISH");

                entity.Property(e => e.CanTrade)
                    .HasPrecision(1)
                    .HasColumnName("CAN_TRADE");
            });

            modelBuilder.Entity<SchoolUser>(entity =>
            {
                entity.HasKey(e => e.SchoolId)
                    .HasName("SCHOOL_USER_PK");

                entity.ToTable("SCHOOL_USER");

                entity.HasIndex(e => e.Nickname, "SCHOOL_USER_NICKNAME_UINDEX")
                    .IsUnique();

                entity.HasIndex(e => e.Phone, "SCHOOL_USER_PHONE_UINDEX")
                    .IsUnique();

                entity.Property(e => e.SchoolId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SCHOOL_ID");

                entity.Property(e => e.AvatarId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("AVATAR_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.Brief)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("BRIEF");

                entity.Property(e => e.Credit)
                    .HasPrecision(3)
                    .HasColumnName("CREDIT");

                entity.Property(e => e.Grade)
                    .HasPrecision(2)
                    .HasColumnName("GRADE");

                entity.Property(e => e.Major)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("MAJOR");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NICKNAME");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("PHONE")
                    .IsFixedLength(true);

                entity.Property(e => e.RealName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("REAL_NAME");

                entity.Property(e => e.Role)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ROLE");

                entity.HasOne(d => d.Avatar)
                    .WithMany(p => p.SchoolUsers)
                    .HasForeignKey(d => d.AvatarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SCHOOL_USER_MEDIA_ID_FK");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.SchoolUsers)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("USER_ROLE_STATE_ROLE_NAME_FK");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => new { e.CommodityId, e.UserId })
                    .HasName("SHOPPING_CART_PK");

                entity.ToTable("SHOPPING_CART");

                entity.Property(e => e.CommodityId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("COMMODITY_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.AddTime)
                    .HasPrecision(6)
                    .HasColumnName("ADD_TIME");

                entity.Property(e => e.Count)
                    .HasPrecision(4)
                    .HasColumnName("COUNT");

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.CommodityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SHOPPING_CART_COMMODITY_ID_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SHOPPING_CART_USER_ID_FK");
            });

            modelBuilder.Entity<UserBanned>(entity =>
            {
                entity.HasKey(e => e.CaseId)
                    .HasName("USER_BANNED_PK");

                entity.ToTable("USER_BANNED");

                entity.Property(e => e.CaseId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CASE_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.AdminId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ADMIN_ID");

                entity.Property(e => e.HandleTime)
                    .HasPrecision(6)
                    .HasColumnName("HANDLE_TIME");

                entity.Property(e => e.OriginRole)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ORIGIN_ROLE");

                entity.Property(e => e.TillTime)
                    .HasPrecision(6)
                    .HasColumnName("TILL_TIME");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.UserBannedAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("USER_BANNED_USER_ID_FK_2");

                entity.HasOne(d => d.OriginRoleNavigation)
                    .WithMany(p => p.UserBannedOriginRoleNavigations)
                    .HasForeignKey(d => d.OriginRole)
                    .HasConstraintName("USER_BANNED_USER_ROLE_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBannedUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("USER_BANNED_USER_ID_FK");
            });

            modelBuilder.Entity<View>(entity =>
            {
                entity.HasKey(e => new { e.CommodityId, e.UserId })
                    .HasName("VIEW_PK");

                entity.ToTable("VIEW");

                entity.Property(e => e.CommodityId)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("COMMODITY_ID")
                    .IsFixedLength(true);

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.Time)
                    .HasPrecision(6)
                    .HasColumnName("TIME");

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.Views)
                    .HasForeignKey(d => d.CommodityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VIEW_COMMODITY_ID_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Views)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VIEW_USER_SCHOOL_ID_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
