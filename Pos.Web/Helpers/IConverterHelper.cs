using Microsoft.EntityFrameworkCore;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;

namespace Pos.Web.Helpers
{
    public interface IConverterHelper
    {
        public AccountUserDTO ToAccountDTO(User user);
        public PrivatePosRole ToRole(PrivatePosRoleDTO dto);
        public Task<PrivatePosRoleDTO> ToRoleDTOAsync(PrivatePosRole role);

        public User ToUser(UserDTO dto);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;

        public ConverterHelper(DataContext context)
        {
            _context = context;
        }

        public AccountUserDTO ToAccountDTO(User user)
        {
            return new AccountUserDTO
            {
                Id = Guid.Parse(user.Id),
                Document = user.Document,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public PrivatePosRole ToRole(PrivatePosRoleDTO dto)
        {
            return new PrivatePosRole
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }

        public async Task<PrivatePosRoleDTO> ToRoleDTOAsync(PrivatePosRole role)
        {
            List<PermissionForDTO> permissions = await _context.Permissions.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.RoleId == role.Id)

            }).ToListAsync();

            return new PrivatePosRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = permissions,
            };
        }
        public User ToUser(UserDTO dto)
        {
            return new User
            {
                Id = dto.Id.ToString(),
                Document = dto.Document,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PrivatePosRoleId = dto.PrivateBlogRoleId,
                PhoneNumber = dto.PhoneNumber,
            };
        }
    }
}
