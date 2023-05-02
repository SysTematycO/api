using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DeltaAPI.Models;
/// <summary>
/// Contexto BD
/// </summary>
public partial class QatarContext : DbContext
{
    public QatarContext()
    {
    }

    public QatarContext(DbContextOptions<QatarContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ModeloPricing> ModeloPricings { get; set; }

    public virtual DbSet<ValorXpolitica> ValorXpoliticas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ModeloPricing>(entity =>
        {
            entity.ToTable("ModeloPricing");

            entity.Property(e => e.TasaEace)
                .HasColumnType("numeric(38, 19)")
                .HasColumnName("TasaEACE");
            entity.Property(e => e.TasaEase)
                .HasColumnType("numeric(38, 19)")
                .HasColumnName("TasaEASE");
        });

        modelBuilder.Entity<ValorXpolitica>(entity =>
        {
            entity.ToTable("ValorXPolitica");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReglaNegocio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("numeric(38, 19)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
