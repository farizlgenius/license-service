using System;
using System.ComponentModel.DataAnnotations;
using LicenseService.Model;

namespace LicenseService.Entity;

public sealed class License : IDatetime
{
  [Key]
  public int Id { get; set; }
  public DateTime CreatedDate { get; set; }
}
