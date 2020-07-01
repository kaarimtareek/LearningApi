using System;
using System.Collections.Generic;
using System.Text;

namespace Services.PropertyMappingService
{
   public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingFor<TSource, TDestination>(string fields);
    }
}
