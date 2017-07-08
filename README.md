# InvisualMvcBinder

[![NuGet](https://img.shields.io/nuget/v/Invisual.Libraries.Mvc.Binder.svg)](https://www.nuget.org/packages/Invisual.Libraries.Mvc.Binder)
[![License](https://img.shields.io/github/license/tombowers/InvisualMvcBinder.svg)](https://github.com/tombowers/InvisualMvcBinder/blob/master/LICENSE)

Bind MVC model properties by name. Includes configurable type conversion.

* [Basic Usage](#basic-usage)
* [Type Conversion](#type-conversion)
  * [Custom Type Converters](#custom-type-converters)
* [Installation](#installation)

### Basic Usage
_Skip to the [bottom](#installation) for how to setup InvisualMvcBinder in your solution._

Add the BindProperty attribute to a property and request values will automatically be bound using your custom name.

```C#
public class ProcessPaymentViewModel
{
    [BindProperty("payment_method_nonce")]
    public string PaymentMethodNonce { get; set; }

    public string TransactionId { get; set; }
}
```

### Type Conversion
InvisualMvcBinder includes an advanced customisable type conversion system.
For instance, use the `UnixTimeStampDateTimeConverter` to automatically convert unix time to DateTime instances.

```C#
[BindProperty(typeof(UnixTimeStampDateTimeConverter))]
public DateTime TimeStamp { get; set; }
```

Or use the `EnumTypeConverter` to allow the use of enums in your models.

```C#
[BindProperty(typeof(EnumTypeConverter<EventType>))]
public EventType Event { get; set; }
```

And, type conversion can be combined with custom argument names.
```C#
[BindProperty("time_stamp", typeof(UnixTimeStampDateTimeConverter))]
public DateTime TimeStamp { get; set; }
```

#### Custom Type Converters
To create a custom type converter, implement the `ITypeConverter<T>` interface.
Here is an example which uses Json.NET to convert a JSON string to a concrete type.
```C#
public class JsonTypeConverter<T> : ITypeConverter<T>
{
    public T Convert(string input)
    {
        return JsonConvert.DeserializeObject<T>(input);
    }
}
```
```C#
[BindProperty(typeof(JsonTypeConverter<CustomType>))]
public CustomType Event { get; set; }
```


### Installation
To install InvisualMvcBinder, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)
```
PM> Install-Package Invisual.Libraries.Mvc.Binder
```

Then, simply register the InvisualMvcBinder on startup. Your application may differ, but your startup code may look like one of the following. 
```C#
// global.asax.cs
protected void Application_Start()
{
    // ...

    ModelBinders.Binders.DefaultBinder = new PropertyBindingModelBinder();
}
```

```C#
// OWIN Startup class, e.g. App_Start/Startup.cs
public void Configuration(IAppBuilder app)
{
    // ...

    ModelBinders.Binders.DefaultBinder = new PropertyBindingModelBinder();
}
```