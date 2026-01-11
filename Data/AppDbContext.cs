using System;
using LicenseService.Model;
using Microsoft.EntityFrameworkCore;

namespace LicenseService.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Entity.KeyPair> Keys { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // put this inside OnModelCreating(ModelBuilder modelBuilder)
    var datetimeInterface = typeof(IDatetime);

    foreach (var et in modelBuilder.Model.GetEntityTypes()
                 .Where(t => t.ClrType != null && datetimeInterface.IsAssignableFrom(t.ClrType)))
    {
      // get the builder for the concrete CLR type (e.g. location, ArEvent, ...)
      var builder = modelBuilder.Entity(et.ClrType);

      // configure created_date
      builder.Property<DateTime>(nameof(IDatetime.CreatedDate))
          .HasColumnType("timestamp without time zone")
          .HasDefaultValueSql("now()")
          .ValueGeneratedOnAdd();

    }
  }
}
