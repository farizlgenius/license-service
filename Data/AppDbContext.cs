using System;
using LicenseService.Entity;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LicenseService.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Entity.KeyPair> Keys { get; set; }
  public DbSet<Entity.License> Licenses { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Entity.KeyPair>()
        .Property(e => e.public_key)
        .HasColumnType("bytea");

    modelBuilder.Entity<Entity.KeyPair>()
        .Property(e => e.private_key)
        .HasColumnType("bytea");


    // put this inside OnModelCreating(ModelBuilder modelBuilder)
    var datetimeInterface = typeof(IDatetime);

    foreach (var et in modelBuilder.Model.GetEntityTypes()
                 .Where(t => t.ClrType != null && datetimeInterface.IsAssignableFrom(t.ClrType)))
    {
      // get the builder for the concrete CLR type (e.g. location, ArEvent, ...)
      var builder = modelBuilder.Entity(et.ClrType);

      // configure created_date
      builder.Property<DateTime>(nameof(IDatetime.created_date))
          .HasColumnType("timestamp without time zone")
          .HasDefaultValueSql("now()")
          .ValueGeneratedOnAdd();

      builder.Property<DateTime>(nameof(IDatetime.expire_date))
          .HasColumnType("timestamp without time zone")
          .HasDefaultValueSql("now()")
          .ValueGeneratedOnAdd();

    }

    modelBuilder.Entity<KeyPair>()
        .HasMany(k => k.Licenses)
        .WithOne(l => l.key_pair)
        .HasForeignKey(l => l.key_uuid)
        .HasPrincipalKey(k => k.key_uuid);
  }
}
