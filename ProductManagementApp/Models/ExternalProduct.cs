using ProductManagementApp.Models;


namespace ProductManagementApp.Models
{
    public class ExternalProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
