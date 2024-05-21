using Microsoft.EntityFrameworkCore;
using Pos.Web.Core.Extensions;

namespace Pos.Web.Core.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int RecordsPerPage { get; private set; }
        public int TotalCount { get; private set; }

        public PagedList(List<T> items, int count, int pageNumber, int recordsperPage)
        {
            TotalCount = count;
            RecordsPerPage = recordsperPage;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)recordsperPage);
            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, PaginationRequest request)
        {
            int count = await source.CountAsync();

            List<T> items = await source.Paginate<T>(request)
                                        .ToListAsync();

            return new PagedList<T>(items, count, request.Page, request.RecordsPerPage);
        }

    }
}
