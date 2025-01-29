using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
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

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Asset_pkey");

            entity.ToTable("Asset");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.FloorMapId).HasColumnName("FloorMapID");

            entity.HasOne(d => d.FloorMap).WithMany(p => p.Assets)
                .HasForeignKey(d => d.FloorMapId)
                .HasConstraintName("FK_FloorMapID");
        });

        modelBuilder.Entity<AssetPositionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AssetPositionHistory_pkey");

            entity.ToTable("AssetPositionHistory");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
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

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
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

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ID");

            entity.ToTable("Role");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UNQ_Username").IsUnique();

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RoleID");
        });

        base.OnModelCreating(modelBuilder);

        // Configure Points property to automatically serialize/deserialize to JSON
        modelBuilder.Entity<Zone>()
            .ToTable("Zone")
            .Property(z => z.Points)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),  // Serialize List<Point> to JSON
                v => JsonConvert.DeserializeObject<List<Point>>(v)  // Deserialize JSON to List<Point>
            );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
