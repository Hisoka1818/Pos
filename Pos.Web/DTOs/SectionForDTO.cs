using Pos.Web.Data.Entities;
namespace Pos.Web.DTOs
{
    public class SectionForDTO : Section
    {
        public bool Selected { get; set; } = false;
    }
}
