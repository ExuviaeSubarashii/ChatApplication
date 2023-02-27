using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Chat.Domain.Models
{
    public partial class ChatContext : DbContext
    {
        public ChatContext()
        {
        }

        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Servers> Servers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-12NGJ7T;Initial Catalog=Chat;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Message1)
                    .HasMaxLength(310)
                    .IsUnicode(false)
                    .HasColumnName("Message")
                    .IsFixedLength();

                entity.Property(e => e.SenderName)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Sender_Name")
                    .IsFixedLength();

                entity.Property(e => e.SenderTime).HasColumnName("Sender_Time");

                entity.Property(e => e.Server)
                    .HasMaxLength(310)
                    .IsUnicode(false)
                    .HasColumnName("Server")
                    .IsFixedLength();
                entity.Property(e => e.Channel)
                   .HasMaxLength(310)
                   .IsUnicode(false)
                   .HasColumnName("Channel")
                   .IsFixedLength();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.HasPassword)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Username)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Image).HasColumnName("Image");

                entity.Property(e => e.Server)
                    .HasMaxLength(7300)
                    .IsUnicode(false)
                    .HasColumnName("Server")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Servers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.ServerName)
                    .HasMaxLength(310)
                    .IsUnicode(false)
                    .HasColumnName("ServerName")
                    .IsFixedLength();
                entity.Property(e => e.Channels)
                   .HasMaxLength(310)
                   .IsUnicode(false)
                   .HasColumnName("Channels")
                   .IsFixedLength();
                entity.Property(e => e.UserNames)
                   .HasMaxLength(310)
                   .IsUnicode(false)
                   .HasColumnName("UserNames")
                   .IsFixedLength();

            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
