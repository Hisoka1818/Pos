using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Data;

namespace Pos.Web.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboPrivateBlogRolesAsync();
        public Task<IEnumerable<SelectListItem>> GetComboSections();
    }

    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboPrivateBlogRolesAsync()
        {
            List<SelectListItem> list = await _context.PrivatePosRoles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString(),
            }).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un rol...]",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboSections()
        {
            List<SelectListItem> list = await _context.Roles.Select(s => new SelectListItem //otra cosa
            {
                Text = s.Name,
                Value = s.Id.ToString(),
            }).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Rol...]",
                Value = "0"
            });

            return list;
        }
    }
}
