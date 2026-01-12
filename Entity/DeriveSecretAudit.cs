using System;
using System.ComponentModel.DataAnnotations;
using LicenseService.Model;

namespace LicenseService.Entity;

public sealed class DeriveSecretAudit : IDatetime
{
  [Key]
  public int id { get; set; }
  public Guid secret_uuid { get; set; } = Guid.NewGuid();
  public Guid key_uuid { get; set; } = Guid.NewGuid();
  public string machine_id { get; set; } = string.Empty;
  public required byte[] public_key { get; set; }
  public required byte[] shared_secret { get; set; }
  public DateTime created_date { get; set; }
  public DateTime expire_date { get; set; }
  public bool is_revoked { get; set; }
  public ECDHKeyPair? key_pair { get; set; }
  public ICollection<License>? licenses { get; set; }
}
