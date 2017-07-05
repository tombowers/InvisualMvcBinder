namespace Invisual.Libraries.Mvc.ModelBinding.TypeConversion
{
  public interface ITypeConverter<out T>
  {
    T Convert(string input);
  }
}
