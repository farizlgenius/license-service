using System;
using System.ComponentModel.DataAnnotations;
using LicenseService.Model;

namespace LicenseService.Entity;

public sealed class SignKeyAudit : IDatetime
{
  [Key]
  public int id { get; set; }
  public Guid sign_key_uuid { get; set; } = Guid.NewGuid();
  public required byte[] sign_pub { get; set; }
  public required byte[] sign_priv { get; set; }
  public DateTime created_date { get; set; }
  public DateTime expire_date { get; set; }
  public bool is_revoked { get; set; }
  public ICollection<License>? licenses { get; set; }
}
