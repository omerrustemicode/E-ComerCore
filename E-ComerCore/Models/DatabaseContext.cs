using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace E_ComerCore.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleAccount> RoleAccount { get; set; }
        public virtual DbSet<SlideShow> SlideShows { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceDetails> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoleAccount>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.AccountId });

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.RoleAccount)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleAccount_Account");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleAccount)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleAccount_Role");
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");

            });
            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Photo_Product");

            });
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");

            });
            modelBuilder.Entity<InvoiceDetails>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceId, e.ProductId });
                entity.HasOne(d => d.Invoices)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetails_Invoice");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InvoiceDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetails_Product");

            });

        }
    }
}
