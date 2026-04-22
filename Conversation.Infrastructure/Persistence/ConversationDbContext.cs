using System;
using System.Collections.Generic;
using Conversation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conversation.Infrastructure.Persistence;

public partial class ConversationDbContext : DbContext
{
    public ConversationDbContext()
    {
    }

    public ConversationDbContext(DbContextOptions<ConversationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ConversationSystem> ConversationSystems { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ecommerce;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConversationSystem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("conversation_system_pkey");

            entity.ToTable("conversation_system", "conversation");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(0)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("messages_pkey");

            entity.ToTable("messages", "conversation");

            entity.HasIndex(e => e.SenderId, "IX_messages_SenderId");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.MessageText).HasColumnName("message");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.SenderName).HasMaxLength(255);
            entity.Property(e => e.SenderType).HasColumnName("sender_type");
            entity.Property(e => e.Status).HasDefaultValue(0);

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("fk_conversation");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
