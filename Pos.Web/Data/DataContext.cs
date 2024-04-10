﻿using Microsoft.EntityFrameworkCore;
using Pos.Web.Data.Entities;

namespace Pos.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<Categories> Categories { get; set; }
    }
}
