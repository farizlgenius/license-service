using System;

namespace LicenseService.Model;

public sealed class AppConfigSetting
{
  public required DemoLicenseSetting DemoLicense { get; set; }
  public required EncryptionSetting Encryption { get; set; }
}

public sealed class EncryptionSetting
{
  public int KeyExpireInMonth { get; set; }
}

public sealed class DemoLicenseSetting
{
  public short nHardware { get; set; }
  public short nModule { get; set; }
  public short nOperator { get; set; }
  public short nLocation { get; set; }
  public short nControl { get; set; }
  public short nMonitor { get; set; }
  public short nMonitorGroup { get; set; }
  public short nDoor { get; set; }
  public short nAccessLevle { get; set; }
  public short nTimezone { get; set; }
  public short nCard { get; set; }
  public short nCardHolder { get; set; }
  public short nTrigger { get; set; }
  public short nHoliday { get; set; }
  public short DurationInDays { get; set; }
  public short GracePeriodInDays { get; set; }
}
