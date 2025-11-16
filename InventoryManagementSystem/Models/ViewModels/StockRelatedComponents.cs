using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Models.ViewModels
{
    public class StockRelatedComponents
    {
        public StockModel details { get; set; }

        public IEnumerable<SelectListItem> SupplierList { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }


    }
}
