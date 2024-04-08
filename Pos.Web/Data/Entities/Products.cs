namespace Pos.Web.Data.Entities
{
    public class Products
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string price { get; set; }

        public string reference { get; set; }

        public string area { get; set; }

        public int CategoriesId { get; set; }

        //propiedades de navegación 
        public  Categories Categories { get; set;}

    }
}
