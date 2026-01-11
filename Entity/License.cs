using System;
using System.ComponentModel.DataAnnotations;
using LicenseService.Enums;
using LicenseService.Model;

namespace LicenseService.Entity;

public sealed class License : IDatetime
{
  [Key]
  public int id { get; set; }
  public string customer { get; set; } = string.Empty;
  public string site { get; set; } = string.Empty;
  public string fingerprint { get; set; } = string.Empty;
  public string key_uuid { get; set; } = string.Empty;
  public LicenseType license_type { get; set; }
  public KeyPair key_pair { get; set; }
  public DateTime created_date { get; set; }
  public DateTime expire_date { get; set; }
}
