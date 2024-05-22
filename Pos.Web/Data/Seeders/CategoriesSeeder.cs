using Microsoft.EntityFrameworkCore;
using Pos.Web.Data.Entities;

namespace Pos.Web.Data.Seeders
{
    internal class CategoriesSeeder
    {
        private readonly DataContext _context;

        public CategoriesSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Categories> categories = new List<Categories>
            {
                 new Categories
                 {
                     categoryName = "Licores",
                     categoryDescription ="."
                 },
                 new Categories
                 {
                     categoryName = "Electrónica",
                     categoryDescription= "Esta categoría incluye una amplia gama de dispositivos electrónicos, desde teléfonos inteligentes y computadoras portátiles hasta televisores y sistemas de sonido. Los productos de electrónica ofrecen soluciones para el entretenimiento, la comunicación y la productividad en el hogar y en entornos profesionales."
                 },
                 new Categories
                 {
                     categoryName=  "Ropa",
                     categoryDescription= "Esta categoría abarca una variedad de prendas de vestir para hombres, mujeres y niños, incluyendo camisetas, pantalones, vestidos, chaquetas y más. La ropa está disponible en una amplia gama de estilos, tallas y colores para adaptarse a diferentes gustos y ocasiones."
                 },

                 new Categories
                 {
                     categoryName = "Hogar y Jardín",
                     categoryDescription ="Esta categoría comprende una selección de productos para decorar y mejorar el hogar, así como para mantener y embellecer el jardín. Desde muebles y decoración interior hasta herramientas de jardinería y accesorios para exteriores, estos productos ofrecen soluciones para crear espacios cómodos y atractivos."

                 },

                 new Categories
                 {
                     categoryName ="Libros y medios",
                     categoryDescription ="Esta categoría abarca una amplia gama de libros impresos y medios digitales, incluyendo novelas, libros de no ficción, música, películas y videojuegos. Los productos de libros y medios ofrecen entretenimiento, educación y enriquecimiento personal para personas de todas las edades e intereses."


                 },
                 new Categories
                 {
                     categoryName ="Alimentos y Bebidas",
                     categoryDescription ="Esta categoría abarca una variedad de alimentos y bebidas, desde productos frescos como frutas y verduras hasta alimentos envasados como cereales, conservas y dulces. Además, incluye una amplia gama de bebidas, como agua, refrescos, café, té y bebidas alcohólicas."
                 },

                 new Categories
                 {
                     categoryName ="Artículos de cocina",
                     categoryDescription = " Esta categoría comprende una variedad de utensilios, herramientas y accesorios diseñados para facilitar la preparación de alimentos y mejorar la experiencia culinaria en el hogar. Desde cuchillos y tablas de cortar hasta batidoras y sartenes, los artículos de cocina están diseñados con materiales duraderos y ergonómicos que hacen que cocinar sea más eficiente y seguro"


                 },



            };

            foreach (Categories category in categories)
            {
                bool exists = await _context.Categories.AnyAsync(s => s.categoryName == category.categoryName);

                if (!exists)
                {
                    await _context.Categories.AddAsync(category);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
