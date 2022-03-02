using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> :List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        
        public PagedList(IEnumerable<T> items,int count,int pagenumber,int pagesize)
        {
            TotalCount = count;
            PageSize = pagesize;
            CurrentPage = pagenumber;
            TotalPages = (int)Math.Ceiling(count / (float)pagesize);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,int pagenumber,int pagesize)
        {
            int count = await source.CountAsync();
            var items = await source.Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();
            return new PagedList<T>(items,count,pagenumber,pagesize);
        }
        
        
        
        
        
        
    }
}