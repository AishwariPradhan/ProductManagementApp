using System.Threading.Tasks;
using ProductManagementApp.Models;

namespace ProductManagementApp.Services
{
    public interface IExternalProductService
    {
        Task<ExternalProduct> GetProductDataAsync(int productId);
    }
}
