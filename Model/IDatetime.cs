using System;

namespace LicenseService.Model;

public interface IDatetime
{
  DateTime created_date { get; set; }
  DateTime expire_date { get; set; }
}
