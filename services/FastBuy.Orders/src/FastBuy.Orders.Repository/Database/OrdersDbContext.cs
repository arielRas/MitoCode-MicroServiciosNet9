﻿using System;
using System.Collections.Generic;
using FastBuy.Orders.Repository.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastBuy.Orders.Repository.Database;

public partial class OrdersDbContext : DbContext
{
    public OrdersDbContext()
    {
    }

    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderItem> OrderItem { get; set; }

    public virtual DbSet<OrderState> OrderState { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(o => o.OrderId).HasDefaultValueSql("(newid())");
            entity.Navigation(o => o.OrderItem).AutoInclude();
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany(p => p.OrderItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER_ITEM_ORDER");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER_ITEM_PRODUCT");
        });

        modelBuilder.Entity<OrderState>(entity =>
        {
            entity.Property(e => e.CorrelationId).ValueGeneratedNever();

            entity.HasOne(d => d.Order).WithOne(p => p.OrderState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER_STATE_ORDER");

            entity.Navigation(os => os.Order).AutoInclude();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
