using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;
using PrivatePos.Web.DTOs;

namespace Pos.Web.Helpers
{
    public interface IConverterHelper
    {
        public AccountUserDTO ToAccountDTO(User user);
        public PrivatePosRole ToRole(PrivatePosRoleDTO dto);
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

        public PrivatePosRole Torole(PrivatePosRoleDTO dto)
        {
            return new PrivatePosRole
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }
    }
}
