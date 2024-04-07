
using Humanizer;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.Helpers;


namespace Pos.Web.Services
{
    public interface ISalesService
    {
        //public Task<Response<List<Sales>>> GetListAsync();

        //public Task<Response<Sales>> CreateAsync(Sales model);

        public class SalesService : ISalesService
        {
           
        }
    }
}
