using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace Invisual.Libraries.Mvc.ModelBinding
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
  public class BindPropertyAttribute : Attribute
  {
    private readonly Type _typeConverter;

    public BindPropertyAttribute(string name)
    {
      if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Argument is null or whitespace", nameof(name));

      Name = name;
    }

    public BindPropertyAttribute(Type typeConverter)
    {
      if (typeConverter == null) throw new ArgumentNullException(nameof(typeConverter));

      _typeConverter = typeConverter;
    }

    public BindPropertyAttribute(string name, Type typeConverter)
    {
      if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Argument is null or whitespace", nameof(name));
      if (typeConverter == null) throw new ArgumentNullException(nameof(typeConverter));

      Name = name;
      _typeConverter = typeConverter;
    }

    public string Name { get; set; }

    public bool BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
    {
      var shouldPerformRequestValidation = controllerContext.Controller.ValidateRequest && bindingContext.ModelMetadata.RequestValidationEnabled;

      ValueProviderResult value = GetValueFromValueProvider(bindingContext, propertyDescriptor, shouldPerformRequestValidation);

      try
      {
        if (_typeConverter != null)
        {
          object converter = Activator.CreateInstance(_typeConverter);
          object convertedValue = _typeConverter.GetMethod("Convert").Invoke(converter, new object[] { value.AttemptedValue });

          propertyDescriptor.SetValue(bindingContext.Model, convertedValue);
        }
        else
        {
          propertyDescriptor.SetValue(bindingContext.Model, Convert.ChangeType(value.AttemptedValue, propertyDescriptor.PropertyType));
        }
      }
      catch (Exception)
      {
        return false;
      }

      return true;
    }

    private ValueProviderResult GetValueFromValueProvider(ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, bool performRequestValidation)
    {
      var unvalidatedValueProvider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
      return unvalidatedValueProvider != null
        ? unvalidatedValueProvider.GetValue(Name ?? propertyDescriptor.Name, !performRequestValidation)
        : bindingContext.ValueProvider.GetValue(Name ?? propertyDescriptor.Name);
    }
  }
}