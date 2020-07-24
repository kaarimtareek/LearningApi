using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Helpers.Mapper
{
    public class MapperHelper : IMapperHelper
    {
        private readonly IMapper mapper;
        
        public MapperHelper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public T MapTo<T>(object entity)
        {
            return mapper.Map<T>(entity);
        }
        public TDestination MapTo<TSource, TDestination>(TSource entity)
        {
            return mapper.Map<TSource, TDestination>(entity);
        }
    }
}
