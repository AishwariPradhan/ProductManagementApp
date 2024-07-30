using ProductManagementApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementApp.Models
{
    [Table("allProduct", Schema = "product")]

    public class allProduct
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; } // Add this property
        public int PriceId { get; set; }
        public Price? Price { get; set; }
    }
}
