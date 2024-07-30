using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Data;
using ProductManagementApp.Models;
using ProductManagementApp.Services;
using ProductManagementApp.Helpers;
using ProductManagementApp.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using ProductManagementApp.Models.ViewModels;

namespace ProductManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductInfoService _productInfoService;

        public ProductsApiController(ApplicationDbContext context, IProductInfoService productInfoService)
        {
            _context = context;
            _productInfoService = productInfoService;
        }

        // GET: api/products?page={pageNumber}&size={pageSize}
        [HttpGet]
        public async Task<IActionResult> GetProducts(int? pageNumber, int pageSize = 10)
        {
            var products = _context.Products.Include(p => p.Price).AsNoTracking();
            int currentPage = pageNumber ?? 1;
            var paginatedList = await PaginatedList<allProduct>.CreateAsync(products, currentPage, pageSize);
            return Ok(paginatedList);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Price)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Fetch additional information from the Fake Store API
            var productDetails = await _productInfoService.GetProductDetailsAsync(id);

            if (productDetails == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Description = productDetails.Description,
                Reviews = "Reviews not available"
            };

            return Ok(viewModel);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductLocaleViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Locales.Add(model.Locale);
            await _context.SaveChangesAsync();

            _context.Prices.Add(model.Product.Price);
            await _context.SaveChangesAsync();

            model.Product.PriceId = model.Product.Price.Id;
            _context.Products.Add(model.Product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = model.Product.Id }, model.Product);
        }

        // GET: api/prices/{productId}
        [HttpGet("prices/{productId}")]
        public async Task<IActionResult> GetPrice(int productId)
        {
            var price = await _context.Prices.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (price == null)
            {
                return NotFound();
            }

            return Ok(price);
        }
    }
}
