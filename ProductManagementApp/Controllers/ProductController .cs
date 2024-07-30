using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Data;
using ProductManagementApp.Models;
using ProductManagementApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductManagementApp.Services;
using ProductManagementApp.Helpers;

namespace ProductManagementApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductInfoService _productInfoService;

        public ProductController(ApplicationDbContext context, IProductInfoService productInfoService)
        {
            _context = context;
            _productInfoService = productInfoService;
        }

        
        [HttpGet]
        [Route("/")]
        //fetch all record from third party api
        public async Task<IActionResult> ExternalProducts()
        {
            // Fetch product data from the external API
            var products = await _productInfoService.GetAllProductsAsync();

            // Pass the product data to the view
            return View(products); // Ensure this view exists and is named ExternalProducts.cshtml
        }


        [HttpGet]
        [Route("product/details/{id}")]

        // for detail page
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Price)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            var productDetails = await _productInfoService.GetProductDetailsAsync(id.Value);

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Description = productDetails?.Description,
                Reviews = "Reviews not available"
            };

            return View(viewModel);
        }

        

        //code for page number
        [HttpGet]
        [Route("product")]
        public async Task<IActionResult> Index(int? pageNumber, int pageSize = 10)
        {
            var products = _context.Products
                .Include(p => p.Price) // Assuming you want to include related price data
                .AsNoTracking(); // For read-only scenarios

            int currentPage = pageNumber ?? 1;
            var paginatedList = await PaginatedList<allProduct>.CreateAsync(products, currentPage, pageSize);
            return View(paginatedList);
        }


        
        //product ceate form get data
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new ProductLocaleViewModel()); // Ensure the correct ViewModel is passed
        }

        //created product 
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductLocaleViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Locales.Add(model.Locale);
                await _context.SaveChangesAsync();

                _context.Prices.Add(model.Product.Price);
                await _context.SaveChangesAsync();

                model.Product.PriceId = model.Product.Price.Id;
                _context.Products.Add(model.Product);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); // Or any other action
            }

            return View(model); // Pass back the same model to the view
        }


       
    }
}
