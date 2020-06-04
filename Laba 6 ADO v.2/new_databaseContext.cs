using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laba_6_ADO_v._2
{
    public partial class new_databaseContext : DbContext
    {
        public new_databaseContext()
        {
        }

        public new_databaseContext(DbContextOptions<new_databaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chemical> Chemical { get; set; }
        public virtual DbSet<Orderdetails> Orderdetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=new_database;Username=admin;Password=1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chemical>(entity =>
            {
                entity.ToTable("chemical");

                entity.Property(e => e.Chemicalid)
                    .HasColumnName("chemicalid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Application)
                    .HasColumnName("application")
                    .HasMaxLength(100);

                entity.Property(e => e.Companymanufacturer)
                    .HasColumnName("companymanufacturer")
                    .HasMaxLength(30);

                entity.Property(e => e.Kindofchemical)
                    .HasColumnName("kindofchemical")
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(30);

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");
            });

            modelBuilder.Entity<Orderdetails>(entity =>
            {
                entity.HasKey(e => new { e.Chemicalid, e.Orderid })
                    .HasName("pk_order_details");

                entity.ToTable("orderdetails");

                entity.Property(e => e.Chemicalid).HasColumnName("chemicalid");

                entity.Property(e => e.Orderid).HasColumnName("orderid");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Chemical)
                    .WithMany(p => p.Orderdetails)
                    .HasForeignKey(d => d.Chemicalid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_details_chemical");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Orderdetails)
                    .HasForeignKey(d => d.Orderid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_details_orders");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.Orderid)
                    .HasName("orders_pkey");

                entity.ToTable("orders");

                entity.Property(e => e.Orderid)
                    .HasColumnName("orderid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Companybuyer)
                    .HasColumnName("companybuyer")
                    .HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
