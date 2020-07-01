using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Services.PropertyMappingService
{
    public class PropertyMap<TSource, TDestination> : IPropertyMap
    {
        public Dictionary<string, PropertyMappingValue> PropertyDictionary { get; set; }
        public PropertyMap(Dictionary<string, PropertyMappingValue> pairs)
        {
            PropertyDictionary = pairs;
        }
       
           
    }
}
