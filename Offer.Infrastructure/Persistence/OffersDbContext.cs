using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.Entities;

namespace Offers.Infrastructure.Persistence;

public partial class OffersDbContext : DbContext
{
    public OffersDbContext()
    {
    }

    public OffersDbContext(DbContextOptions<OffersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Offer> Offers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ecommerce;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("offers_pkey");

            entity.ToTable("offers", "negotiation");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.OfferedPrice)
                .HasPrecision(10, 2)
                .HasColumnName("offered_price");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
