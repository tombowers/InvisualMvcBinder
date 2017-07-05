using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Invisual.Libraries.Mvc.ModelBinding
{
  public class PropertyBindingModelBinder : DefaultModelBinder
  {
    protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
    {
      BindPropertyAttribute propBindAttr = propertyDescriptor.Attributes.OfType<BindPropertyAttribute>().FirstOrDefault();

      if (propBindAttr != null && propBindAttr.BindProperty(controllerContext, bindingContext, propertyDescriptor))
        return;

      base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
    }
  }
}