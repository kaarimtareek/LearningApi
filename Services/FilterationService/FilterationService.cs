using NLog;
using Services.PropertyMappingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.FilterationService
{
    public class FilterationService : IFilterationService
    {
        private readonly IPropertyMappingService propertyMappingService;
        //need to remove it and make log service
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public FilterationService(IPropertyMappingService propertyMappingService)
        {
            this.propertyMappingService = propertyMappingService;
        }
        // the main function to ge the filtration string that will be used in applying sorting
        public string GetFiltrationString<TSource, TDestination>(string queryString)
        {
           var propertyResult = propertyMappingService.GetPropertyMapping<TSource, TDestination>();
            var QueryFields = queryString.Split(",",StringSplitOptions.RemoveEmptyEntries);
            var filterationString = "";
            //loop over the query fields from the query string
            foreach(var value in QueryFields.Reverse())
            {
                //separate the property name from the filteration method
               var filteration = getCorrectFilteration(value);
                if(!propertyResult.ContainsKey(filteration.Field))
                {
                    throw new ArgumentException($"no key found for {filteration.Field}");
                }
                var mappingValue = propertyResult[filteration.Field];
                if (mappingValue == null)
                {
                    throw new ArgumentNullException($"value of {filteration.Field} not found");
                }
                //revert the filteration method if it's true (from ascending to descinding or vise virsa)
                if (mappingValue.IsRevert)
                {
                    filteration.RevertFilterationMethod();
                }
                foreach (var propertyField in mappingValue.Porperteis)
                {
                    filterationString += propertyField + " " + filteration.FilterationMethod + ",";
                }
            }
            //removes the last comma from the filteration string
            filterationString = filterationString.Remove(filterationString.Length - 1);
            logger.Info("filteration string in filteration service" + filterationString);
            return filterationString;
        }
        private FilterationObject getCorrectFilteration(string field)
        {
            field = field.Trim();
            int spacedIndex = field.IndexOf(" ");
            bool isDescending = field.EndsWith(" desc");
            string  filerationMethod = isDescending? "descending" :"ascending" ;
            string correctField = spacedIndex == -1 ? field : field.Remove(spacedIndex);
            return new FilterationObject { Field = correctField, FilterationMethod = filerationMethod };
        }
    }
    //class to hold the field and the method of sorting( if it's ascending or descending)
    class FilterationObject
    {
        private const string ascending = "ascending";
        private const string descending = "descending";
        public string Field { get; set; }
        public string FilterationMethod { get; set; }
        public void RevertFilterationMethod()
        {
            FilterationMethod = FilterationMethod == ascending ? descending : ascending;
        }
    }
}
