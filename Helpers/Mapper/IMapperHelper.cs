using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers.Mapper
{
    public interface IMapperHelper
    {
        T MapTo<T>(object entity);
        TDestination MapTo<TSource, TDestination>(TSource entity);
    }
}
