using System;
using System.Linq;

namespace Invisual.Libraries.Mvc.ModelBinding.TypeConversion
{
  public class EnumTypeConverter<T> : ITypeConverter<T> where T : struct
  {
    public T Convert(string input)
    {
      return ToEnum(input);
    }

    private static T ToEnum(string name)
    {
      return Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(v => GetPropertyName(v) == name);
    }

    private static string GetPropertyName(object value)
    {
      var fi = value.GetType().GetField(value.ToString());

      var attributes = (BindPropertyAttribute[])fi.GetCustomAttributes(
        typeof(BindPropertyAttribute),
        false
        );

      return attributes.Any() ? attributes[0].Name : value.ToString();
    }
  }
}
