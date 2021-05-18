using MCB.VBO.Microservices.Statements.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Extensions
{
    public static class DataPagerExtension
    {
        public static PagedModel<TModel> Paginate<TModel>(
            this IList<TModel> list,
            int page,
            int limit)
            where TModel : class
        {

            var paged = new PagedModel<TModel>();

            page = (page < 0) ? 1 : page;

            paged.CurrentPage = page;
            paged.PageSize = limit;

            var startRow = (page - 1) * limit;
            paged.Items = list
                       .Skip(startRow)
                       .Take(limit)
                       .ToList();

            paged.TotalItems = list.Count;
            paged.TotalPages = (int)Math.Ceiling(paged.TotalItems / (double)limit);

            return paged;
        }
    }
}
