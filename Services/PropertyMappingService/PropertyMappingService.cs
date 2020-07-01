using DTOs.AuthorDTOs;
using DTOs.CourseDTOs;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.PropertyMappingService
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> authorPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
                {"Id",new PropertyMappingValue(new List<string>(){"Id"}) },
                {"MainCategory",new PropertyMappingValue(new List<string>(){"MainCategory"}) },
                {"Age",new PropertyMappingValue(new List<string>(){"DateOfBirth"},true) },
                {"Name",new PropertyMappingValue(new List<string>(){"FirstName","LastName"}) },

        };
        private readonly Dictionary<string, PropertyMappingValue> coursePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>{"Id"}) },
                {"AuthorId",new PropertyMappingValue(new List<string>{ "AuthorId"})},
                {"Title",new PropertyMappingValue(new List<string>{ "Title"}) },
                {"Description",new PropertyMappingValue(new List<string>{ "Description"}) },
            };
        private IList<IPropertyMap> propertyMaps = new List<IPropertyMap>();
        public PropertyMappingService()
        {
            propertyMaps.Add(new PropertyMap<AuthorDto,Author>( authorPropertyMapping));
            propertyMaps.Add(new PropertyMap<CourseDto, Course>(coursePropertyMapping));

        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var result = propertyMaps.OfType<PropertyMap<TSource, TDestination>>();
            if(result==null || result.Count()==0)
            {
                throw new ArgumentException($"Cannot find property map for {typeof(TSource)} and {typeof(TDestination)}");
            }
            return result.First().PropertyDictionary;
        }

        public bool ValidMappingFor<TSource, TDestination>(string fields)
        {
            var mappingDictionary = GetPropertyMapping<TSource, TDestination>();
            string[] stringsAfterSplit = fields.Split(",");
            foreach(var value in stringsAfterSplit)
            {
                string field = value.Trim();
                int spacedIndex = field.IndexOf(" ");
                string fieldWithoutSpace = spacedIndex == -1 ? field: field.Remove(spacedIndex);
                if (!mappingDictionary.ContainsKey(fieldWithoutSpace))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
