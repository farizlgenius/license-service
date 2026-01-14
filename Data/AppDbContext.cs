using System;
using LicenseService.Entity;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LicenseService.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Entity.SignKeyAudit> sign_key { get; set; }
  public DbSet<Entity.License> license { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Entity.SignKeyAudit>()
        .Property(e => e.sign_pub)
        .HasColumnType("bytea");

    modelBuilder.Entity<Entity.SignKeyAudit>()
        .Property(e => e.sign_priv)
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

    modelBuilder.Entity<SignKeyAudit>()
        .HasMany(k => k.licenses)
        .WithOne(s => s.sign_key)
        .HasForeignKey(s => s.sign_key_uuid)
        .HasPrincipalKey(k => k.sign_key_uuid);

  }
}
