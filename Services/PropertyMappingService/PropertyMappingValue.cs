using System;
using System.Collections.Generic;
using System.Text;

namespace Services.PropertyMappingService
{
    public class PropertyMappingValue
    {
        public List<string> Porperteis { get; set; }
        public bool IsRevert { get; set; }

        public PropertyMappingValue(List<string> properties,bool isRevert=false)
        {
            this.Porperteis = properties;
            this.IsRevert = isRevert;
        }
    }
}
