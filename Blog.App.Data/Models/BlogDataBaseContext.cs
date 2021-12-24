using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Blog.App.Data.Models
{
    public partial class BlogDataBaseContext : DbContext
    {
        public BlogDataBaseContext()
        {
        }

        public BlogDataBaseContext(DbContextOptions<BlogDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApprovePost> ApprovePosts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=Desktop-6dn1b2n;database=blogdatabase;trusted_connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ApprovePost>(entity =>
            {
                entity.ToTable("ApprovePost");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproverId).HasColumnName("ApproverID");

                entity.Property(e => e.DateApprove).HasColumnType("datetime");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.HasOne(d => d.Approver)
                    .WithMany(p => p.ApprovePosts)
                    .HasForeignKey(d => d.ApproverId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ApprovePost_User");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ApprovePosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ApprovePost_Post");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatId)
                    .HasName("PK__Catergor__6A1C8ADAB099E678");

                entity.ToTable("Category");

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CatName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CatParentId).HasColumnName("CatParentID");

                entity.Property(e => e.LayoutDescription)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.CatParent)
                    .WithMany(p => p.InverseCatParent)
                    .HasForeignKey(d => d.CatParentId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.AuthorName).HasMaxLength(50);

                entity.Property(e => e.CommentParentId).HasColumnName("CommentParentID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.CommentParent)
                    .WithMany(p => p.InverseCommentParent)
                    .HasForeignKey(d => d.CommentParentId)
                    .HasConstraintName("FK_Comment_Comment");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Comment_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ImagePath)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Image_Post");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.AuthorName).HasMaxLength(50);

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('Pending')");

                entity.Property(e => e.Thumb)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Post_User");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CatId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Post_Category");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleDescription).HasMaxLength(200);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534244118B5")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Passwd)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Active')");

                entity.Property(e => e.Thumb)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_User_Role1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
