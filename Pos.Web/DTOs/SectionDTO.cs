using Pos.Web.Core.Pagination;
using System.Reflection.Metadata;

namespace Pos.Web.DTOs
{
    public class SectionDTO
    {
        public class SectionDTO : Section
        {
            public PaginationResponse<Blog> PaginatedBlogs { get; set; }
        }
    }
}
