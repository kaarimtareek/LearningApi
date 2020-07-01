using Services.PropertyMappingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.FilterationService
{
    public interface IFilterationService
    {
        string GetFiltrationString<TSource,TDestination>(string queryString);
    }
}
