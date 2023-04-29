using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Expressions;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        //this pageList constructor will be triggered from static method which will pass the count, pageNumber and pageSize along with paginated items
        //fetching of data is already done before creating this pageList object creation. this is only to return back a custom List type
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            //AddRange is a method inside List class which we are calling during pageList 
            AddRange(items);  //Adds the elements of the specified collection to the end of the List<T>
        }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        //So we call this static method in class and pass a query with pageNumber and pageSize
        //It will return a pageList type back which is derived from List type along with pagination info. 
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync(); //query against database runs here
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();  //query against database runs here


            return new PagedList<T>(items, count, pageNumber, pageSize);



        }


    }
}