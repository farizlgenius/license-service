using System;
using System.ComponentModel.DataAnnotations;
using LicenseService.Model;

namespace LicenseService.Entity;

public sealed class KeyPair : IDatetime
{
  [Key]
  public int id { get; set; }
  public string key_uuid { get; set; } = new Guid().ToString();
  public byte[] private_key { get; set; }
  public byte[] public_key { get; set; }
  public DateTime created_date { get; set; }
  public DateTime expire_date { get; set; }
  public bool is_revoked { get; set; }
  public ICollection<License>? Licenses { get; set; }
}
