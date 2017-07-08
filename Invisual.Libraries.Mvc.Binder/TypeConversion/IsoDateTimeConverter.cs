using Invisual.Libraries.Mvc.ModelBinding.TypeConversion;
using System;

namespace Invisual.Libraries.Mvc.Binder.TypeConversion
{
  public class IsoDateTimeConverter : ITypeConverter<DateTime>
  {
    public DateTime Convert(string input)
    {
      return DateTime.Parse(input);
    }
  }
}
