using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductManagementApp.Models;

namespace ProductManagementApp.Services
{
    public class ExternalProductService : IExternalProductService
    {
        private readonly HttpClient _httpClient;

        public ExternalProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExternalProduct> GetProductDataAsync(int productId)
        {
            var url = $"https://fakestoreapi.com/products/{productId}";
            var response = await _httpClient.GetStringAsync(url);
            var productData = JsonConvert.DeserializeObject<ExternalProduct>(response);
            return productData;
        }
    }

   
}
