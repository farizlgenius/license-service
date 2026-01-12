using System;
using System.ComponentModel.DataAnnotations;
using LicenseService.Enums;
using LicenseService.Model;

namespace LicenseService.Entity;

public sealed class License : IDatetime
{
  [Key]
  public int id { get; set; }
  public string company { get; set; } = string.Empty;
  public string customer_site { get; set; } = string.Empty;
  public string machine_id { get; set; } = string.Empty;
  public Guid secret_uuid { get; set; } = Guid.Empty;
  public byte[] license { get; set; } = Array.Empty<byte>();
  public LicenseType license_type { get; set; }
  public int secret_id { get; set; }
  public DeriveSecretAudit? secret_key { get; set; }
  public DateTime created_date { get; set; }
  public DateTime expire_date { get; set; }
}
