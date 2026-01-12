using System;
using LicenseService.Entity;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LicenseService.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Entity.DeriveSecretAudit> Secrets { get; set; }
  public DbSet<Entity.License> Licenses { get; set; }
  public DbSet<Entity.ECDHKeyPair> KeyPairs { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Entity.DeriveSecretAudit>()
        .Property(e => e.public_key)
        .HasColumnType("bytea");

    modelBuilder.Entity<Entity.DeriveSecretAudit>()
        .Property(e => e.shared_secret)
        .HasColumnType("bytea");

    modelBuilder.Entity<Entity.ECDHKeyPair>()
        .Property(e => e.public_key)
        .HasColumnType("bytea");

    modelBuilder.Entity<Entity.ECDHKeyPair>()
        .Property(e => e.secret_key)
        .HasColumnType("bytea");

    modelBuilder.Entity<Entity.License>()
        .Property(e => e.license)
        .HasColumnType("bytea");



    // put this inside OnModelCreating(ModelBuilder modelBuilder)
    // var datetimeInterface = typeof(IDatetime);

    // foreach (var et in modelBuilder.Model.GetEntityTypes()
    //              .Where(t => t.ClrType != null && datetimeInterface.IsAssignableFrom(t.ClrType)))
    // {
    //   // get the builder for the concrete CLR type (e.g. location, ArEvent, ...)
    //   var builder = modelBuilder.Entity(et.ClrType);

    //   // configure created_date
    //   builder.Property<DateTime>(nameof(IDatetime.created_date))
    //       .HasColumnType("timestamp without time zone")
    //       .HasDefaultValueSql("now()")
    //       .ValueGeneratedOnAdd();

    //   builder.Property<DateTime>(nameof(IDatetime.expire_date))
    //       .HasColumnType("timestamp without time zone")
    //       .HasDefaultValueSql("now()")
    //       .ValueGeneratedOnAdd();

    // }

    modelBuilder.Entity<ECDHKeyPair>()
        .HasMany(k => k.secrets)
        .WithOne(s => s.key_pair)
        .HasForeignKey(s => s.key_uuid)
        .HasPrincipalKey(k => k.key_uuid);

    modelBuilder.Entity<DeriveSecretAudit>()
        .HasMany(k => k.licenses)
        .WithOne(l => l.secret_key)
        .HasForeignKey(l => l.secret_id)
        .HasPrincipalKey(s => s.id);
  }
}
