using System;
using System.ComponentModel.DataAnnotations;
using LicenseService.Model;

namespace LicenseService.Entity;

public sealed class KeyPair : IDatetime
{
  [Key]
  public int Id { get; set; }
  public string KeyId { get; set; } = string.Empty;
  public string PrivateKey { get; set; } = string.Empty;
  public string PublicKey { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; }
  public bool IsRevoked { get; set; }
}
