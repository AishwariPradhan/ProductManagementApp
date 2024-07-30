using ProductManagementApp.Models.ViewModels;
namespace ProductManagementApp.Models.ViewModels
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public decimal InitialPrice { get; set; }
        public string LocaleName { get; set; }  // Add this property for locale information

    }

}
