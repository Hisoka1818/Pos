﻿using Microsoft.EntityFrameworkCore;

namespace Pos.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            public Dbset<Product> Products { get; set; }
        }
    }
}
