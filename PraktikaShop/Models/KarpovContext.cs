using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PraktikaShop.Models;

public partial class KarpovContext : DbContext
{
    public KarpovContext()
    {
    }

    public KarpovContext(DbContextOptions<KarpovContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basket> Baskets { get; set; }

    public virtual DbSet<BasketProduct> BasketProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserComment> UserComments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=83.166.247.22;Port=5432;Username=karpov;Database=karpov;Password=12345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.BasketId).HasName("basket_pkey");

            entity.ToTable("basket");

            entity.Property(e => e.BasketId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("basket_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("basket_userid_fkey");
        });

        modelBuilder.Entity<BasketProduct>(entity =>
        {
            entity.HasKey(e => e.BasketProductId).HasName("basket_product_pkey");

            entity.ToTable("basket_product");

            entity.Property(e => e.BasketProductId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("basket_product_id");
            entity.Property(e => e.BasketId).HasColumnName("basket_id");
            entity.Property(e => e.ProductCount).HasColumnName("product_count");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Basket).WithMany(p => p.BasketProducts)
                .HasForeignKey(d => d.BasketId)
                .HasConstraintName("basketproduct_basketid_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.BasketProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("basketproduct_productid_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("order_pkey");

            entity.ToTable("order");

            entity.Property(e => e.OrderId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("order_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.OrderProductId).HasName("order_product_pkey");

            entity.ToTable("order_product");

            entity.Property(e => e.OrderProductId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("order_product_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_product_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("order_product_product_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.ProductId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("product_id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.ProductName)
                .HasColumnType("character varying")
                .HasColumnName("product_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.RoleId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("role_id");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Fullname)
                .HasColumnType("character varying")
                .HasColumnName("fullname");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Passport)
                .HasColumnType("character varying")
                .HasColumnName("passport");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("user_roleid_fkey");
        });

        modelBuilder.Entity<UserComment>(entity =>
        {
            entity.HasKey(e => e.UserCommentId).HasName("user_comment_pkey");

            entity.ToTable("user_comment");

            entity.Property(e => e.UserCommentId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_comment_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.UserComments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("usercomment_productid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserComments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("usercomment_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
