using System;

namespace Invisual.Libraries.Mvc.ModelBinding.TypeConversion
{
  public class UnixTimeStampDateTimeConverter : ITypeConverter<DateTime>
  {
    public DateTime Convert(string input)
    {
      long timestamp;
      if (!long.TryParse(input, out timestamp))
        return DateTime.MinValue;

      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return epoch.AddSeconds(timestamp);
    }
  }
}