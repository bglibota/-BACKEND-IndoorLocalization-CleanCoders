using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization_API.Models;

public partial class IndoorLocalizationContext : DbContext
{
    public IndoorLocalizationContext()
    {
    }

    public IndoorLocalizationContext(DbContextOptions<IndoorLocalizationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetPositionHistory> AssetPositionHistories { get; set; }

    public virtual DbSet<AssetZoneHistory> AssetZoneHistories { get; set; }

    public virtual DbSet<FloorMap> FloorMaps { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=IndoorLocalization;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Asset_pkey");

            entity.ToTable("Asset");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
            entity.Property(e => e.FloorMapId).HasColumnName("FloorMapID");

            entity.HasOne(d => d.FloorMap).WithMany(p => p.Assets)
                .HasForeignKey(d => d.FloorMapId)
                .HasConstraintName("Asset_FloorMapID_fkey");
        });

        modelBuilder.Entity<AssetPositionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AssetPositionHistory_pkey");

            entity.ToTable("AssetPositionHistory");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
            entity.Property(e => e.AssetId).HasColumnName("AssetID");
            entity.Property(e => e.DateTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.FloorMapId).HasColumnName("FloorMapID");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetPositionHistories)
                .HasForeignKey(d => d.AssetId)
                .HasConstraintName("FK_AssetID");

            entity.HasOne(d => d.FloorMap).WithMany(p => p.AssetPositionHistories)
                .HasForeignKey(d => d.FloorMapId)
                .HasConstraintName("FK_FloorMapID");
        });

        modelBuilder.Entity<AssetZoneHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AssetZoneHistory_pkey");

            entity.ToTable("AssetZoneHistory");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
            entity.Property(e => e.AssetId).HasColumnName("AssetID");
            entity.Property(e => e.EnterDateTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ExitDateTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetZoneHistories)
                .HasForeignKey(d => d.AssetId)
                .HasConstraintName("FK_AssetID");

            entity.HasOne(d => d.Zone).WithMany(p => p.AssetZoneHistories)
                .HasForeignKey(d => d.ZoneId)
                .HasConstraintName("FK_ZoneID");
        });

        modelBuilder.Entity<FloorMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FloorMap_pkey");

            entity.ToTable("FloorMap");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Zone_pkey");

            entity.ToTable("Zone");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
