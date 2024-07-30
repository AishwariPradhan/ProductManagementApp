using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductManagementApp.Models;

namespace ProductManagementApp.Services
{
    public class ProductInfoService : IProductInfoService
    {
        private readonly HttpClient _httpClient;

        public ProductInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDetails[]> GetAllProductsAsync()
        {
            var response = await _httpClient.GetStringAsync("https://fakestoreapi.com/products");
            var products = JsonConvert.DeserializeObject<ProductDetails[]>(response);
            return products;
        }

        public async Task<ProductDetails> GetProductDetailsAsync(int id)
        {
            var response = await _httpClient.GetStringAsync($"products/{id}");
            return JsonConvert.DeserializeObject<ProductDetails>(response);
        }
    }
}
