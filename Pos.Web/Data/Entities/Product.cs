﻿namespace Pos.Web.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SalesDetail> SalesDetails { get; set; }
    }
}