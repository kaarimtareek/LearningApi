using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.QueryParamters
{
    public class AuthorsQueryParamters 

    {
        private const int MaxSize = 20;
        public string OrderBy { get; set; }
        public string SearchQuery { get; set; }
        public bool WithCourses { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get => _PageSize; set => _PageSize = value > MaxSize ? MaxSize : value; }
        private int _PageSize = 10;

        public override string ToString()
        {
            string orderBy = OrderBy ?? "No sorting requested";
            string searchQuery = SearchQuery ?? "No searching requested";

            return $"{this} OrderBy:{orderBy}, SearchQuery:{searchQuery} ,PageNumber :{PageNumber},PageSize :{_PageSize}";
        }

        //public override string ToString() => base.ToString();
    }
}
