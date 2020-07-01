using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.PaginationService
{
    public class PaginationList<T> : List<T>
    {
        public IEnumerable<T> ListData{ get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        private PaginationList(IEnumerable<T> list,int pageNumber,int pageSize,int totalPages,int totalCount)
        {
            TotalPages =totalPages;
            TotalCount = totalCount;
            CurrentPage = pageNumber > TotalPages ? TotalPages : pageNumber;
            ListData = list;
            PageSize = pageSize;
        }
        public static  PaginationList<T> CreatePagination(IQueryable<T> collection, int pageSize=10, int pageNumber=1)
        {
            int totalCount = collection.Count();
            int totalPages =(int) Math.Ceiling(totalCount / (double)pageSize);
            int skipNumber = pageNumber > totalPages ? (totalPages-1) * pageSize : (pageNumber-1) * pageSize;
            collection = collection.Skip(skipNumber).Take(pageSize);
            var newList = collection.ToList();
            return new PaginationList<T>(newList, pageNumber, pageSize, totalPages,totalCount);

        }
    }
}
