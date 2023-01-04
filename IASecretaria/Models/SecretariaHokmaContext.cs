using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IASecretaria.Models;

public partial class SecretariaHokmaContext : DbContext
{
    public SecretariaHokmaContext()
    {
    }

    public SecretariaHokmaContext(DbContextOptions<SecretariaHokmaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Intencione> Intenciones { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HOKMA-010\\SQLEXPRESS;Database=SecretariaHokma;User ID=hokma;Password=dylanmajo78;TrustServerCertificate=true;Encrypt=True;Trusted_Connection=True");
    //Server=HOKMA-010\\SQLEXPRESS;Database=SecretariaHokma;User ID=hokma;Password=dylanmajo78;TrustServerCertificate=true;Encrypt=True;Trusted_Connection=True

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Intencione>(entity =>
        {
            entity.HasKey(e => e.IntencionesId).HasName("PK__intencio__345254424AB10CA4");

            entity.ToTable("intenciones");

            entity.Property(e => e.IntencionesId)
                .ValueGeneratedNever()
                .HasColumnName("intencionesId");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(125)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__videos__14B0F5B6367B58B8");

            entity.ToTable("videos");

            entity.HasIndex(e => e.IntencionesId, "uk_videos_intenciones").IsUnique();

            entity.Property(e => e.VideoId)
                .ValueGeneratedNever()
                .HasColumnName("videoId");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IntencionesId).HasColumnName("intencionesId");

            entity.HasOne(d => d.Intenciones).WithOne(p => p.Video)
                .HasForeignKey<Video>(d => d.IntencionesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_videos_intenciones");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
