using System;

namespace LicenseService.Model;

public sealed record LicensePayload
(
  Guid Id,
  string Customer,
  string FingerPrint,
  short nHardware,
  short nModule,
  short nOperator,
  short nLocation,
  short nControl,
  short nMonitor,
  short nMonitorGroup,
  short nDoor,
  short nAccessLevel,
  short nTimezone,
  short nCard,
  short nCardholder,
  short nTrigger,
  short nHoliday,
  DateTime Issue,
  DateTime Expire,
  short GracePeriodInDays,
  bool IsOpenLicense
);
