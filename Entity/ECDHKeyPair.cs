using System;
using System.ComponentModel.DataAnnotations;
using LicenseService.Model;

namespace LicenseService.Entity;

public sealed class ECDHKeyPair : IDatetime
{
  [Key]
  public int id { get; set; }
  public Guid key_uuid { get; set; } = Guid.NewGuid();
  public required byte[] public_key { get; set; }
  public required byte[] secret_key { get; set; }
  public DateTime created_date { get; set; }
  public DateTime expire_date { get; set; }
  public bool is_revoked { get; set; }
  public ICollection<DeriveSecretAudit>? secrets { get; set; }
}
