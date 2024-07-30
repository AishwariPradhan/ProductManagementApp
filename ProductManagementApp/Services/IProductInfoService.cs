using ProductManagementApp.Services;

namespace ProductManagementApp.Services
{
    public interface IProductInfoService
    {
        Task<ProductDetails[]> GetAllProductsAsync();
        Task<ProductDetails> GetProductDetailsAsync(int id);
    }

    public class ProductDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
