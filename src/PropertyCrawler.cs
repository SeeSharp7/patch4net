using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeeSharp7.Patch4Net
{
    class PropertyCrawler
    {
        IEnumerable<PropertyInfo> GetDeepProperties(object obj, BindingFlags flags)
        {
            // Get properties of the current object
            foreach (PropertyInfo property in obj.GetType().GetProperties(flags))
            {
                yield return property;

                object propertyValue = property.GetValue(obj, null);
                if (propertyValue == null)
                {
                    // Property is null, but can still get properties of the PropertyType
                    foreach (PropertyInfo subProperty in property.PropertyType.GetProperties(flags))
                    {
                        yield return subProperty;
                    }
                }
                else
                {
                    // Get properties of the value assiged to the property
                    foreach (PropertyInfo subProperty in GetDeepProperties(propertyValue, flags))
                    {
                        yield return subProperty;
                    }
                }
            }
        }
    }
}