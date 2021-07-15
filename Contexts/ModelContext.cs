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
                optionsBuilder.UseOracle("Name=ConnectionStrings:Mercury");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("YUANZHANG");

            modelBuilder.Entity<ChatRecord>(entity =>
            {
                entity.HasKey(e => new { e.SenderId, e.ReceiverId, e.Index })
                    .HasName("CHAT_RECORD_PK");

                entity.Property(e => e.SenderId).IsUnicode(false);

                entity.Property(e => e.ReceiverId).IsUnicode(false);

                entity.Property(e => e.Index).HasPrecision(4);

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Time).HasPrecision(6);

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
                entity.Property(e => e.Id).HasPrecision(3);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Commodity>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Classification).HasPrecision(3);

                entity.Property(e => e.Condition).IsUnicode(false);

                entity.Property(e => e.Cover).IsUnicode(false);

                entity.Property(e => e.ForRent).HasPrecision(1);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.OwnerId).IsUnicode(false);

                entity.Property(e => e.Popularity).HasPrecision(3);

                entity.Property(e => e.Stock).HasPrecision(4);

                entity.Property(e => e.Unit).IsUnicode(false);

                entity.Property(e => e.VideoId)
                    .IsUnicode(false)
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
                    .HasConstraintName("COMMODITY_MEDIA_ID_FK");
            });

            modelBuilder.Entity<CommodityImage>(entity =>
            {
                entity.HasKey(e => new { e.CommodityId, e.ImageId })
                    .HasName("COMMODITY_IMAGE_PK");

                entity.Property(e => e.CommodityId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ImageId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Order).HasPrecision(1);

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

                entity.Property(e => e.CommodityId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Tag).IsUnicode(false);

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

                entity.Property(e => e.CommodityId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).IsUnicode(false);

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
                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Path).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Content).IsUnicode(false);
            });

            modelBuilder.Entity<NeedPost>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.SenderId).IsUnicode(false);

                entity.Property(e => e.Time).HasPrecision(6);

                entity.Property(e => e.Title)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.NeedPosts)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("NEED_POST_USER_SCHOOL_ID_FK");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.BuyerId).IsUnicode(false);

                entity.Property(e => e.CommodityId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Count).HasPrecision(4);

                entity.Property(e => e.Location).IsUnicode(false);

                entity.Property(e => e.ReturnLocation).IsUnicode(false);

                entity.Property(e => e.ReturnTime).HasPrecision(6);

                entity.Property(e => e.Status).IsUnicode(false);

                entity.Property(e => e.Time).HasPrecision(6);

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
                entity.Property(e => e.Id)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.PostId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.SenderId).IsUnicode(false);

                entity.Property(e => e.Time).HasPrecision(6);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("POST_COMMENT_NEED_POST_ID_FK");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.PostComments)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("POST_COMMENT_USER_SCHOOL_ID_FK");
            });

            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.HasKey(e => new { e.ImageId, e.PostId })
                    .HasName("POST_IMAGE_PK");

                entity.Property(e => e.ImageId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PostId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Position).IsUnicode(false);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("POST_IMAGE_COM_VIDEO_ID_FK");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostImages)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("POST_IMAGE_NEED_POST_ID_FK");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.RatingId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.IsBuyer).HasPrecision(1);

                entity.Property(e => e.OrderId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Rate).HasPrecision(2);

                entity.Property(e => e.Time).HasPrecision(6);

                entity.Property(e => e.UserId).IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("RATING_ORDER_ID_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("RATING_USER_SCHOOL_ID_FK");
            });

            modelBuilder.Entity<ReportUser>(entity =>
            {
                entity.HasKey(e => new { e.ReporterId, e.InformantId })
                    .HasName("REPORT_USER_PK");

                entity.Property(e => e.ReporterId).IsUnicode(false);

                entity.Property(e => e.InformantId).IsUnicode(false);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Time).HasPrecision(6);

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

                entity.Property(e => e.RoleName).IsUnicode(false);

                entity.Property(e => e.CanBan).HasPrecision(1);

                entity.Property(e => e.CanChat).HasPrecision(1);

                entity.Property(e => e.CanComment).HasPrecision(1);

                entity.Property(e => e.CanLogin).HasPrecision(1);

                entity.Property(e => e.CanPost).HasPrecision(1);

                entity.Property(e => e.CanPublish).HasPrecision(1);

                entity.Property(e => e.CanTrade).HasPrecision(1);
            });

            modelBuilder.Entity<SchoolUser>(entity =>
            {
                entity.HasKey(e => e.SchoolId)
                    .HasName("SCHOOL_USER_PK");

                entity.Property(e => e.SchoolId).IsUnicode(false);

                entity.Property(e => e.AvatarId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Brief).IsUnicode(false);

                entity.Property(e => e.Credit).HasPrecision(3);

                entity.Property(e => e.Grade).HasPrecision(2);

                entity.Property(e => e.Major).IsUnicode(false);

                entity.Property(e => e.Nickname).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.RealName).IsUnicode(false);

                entity.Property(e => e.Role).IsUnicode(false);

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

                entity.Property(e => e.CommodityId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).IsUnicode(false);

                entity.Property(e => e.AddTime).HasPrecision(6);

                entity.Property(e => e.Count).HasPrecision(4);

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

                entity.Property(e => e.CaseId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AdminId).IsUnicode(false);

                entity.Property(e => e.HandleTime).HasPrecision(6);

                entity.Property(e => e.OriginRole).IsUnicode(false);

                entity.Property(e => e.TillTime).HasPrecision(6);

                entity.Property(e => e.UserId).IsUnicode(false);

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

                entity.Property(e => e.CommodityId)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).IsUnicode(false);

                entity.Property(e => e.Time).HasPrecision(6);

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

            modelBuilder.HasSequence("SEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
