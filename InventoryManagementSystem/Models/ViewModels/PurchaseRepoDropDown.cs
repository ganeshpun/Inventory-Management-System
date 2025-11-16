using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Models.ViewModels
{
    public class PurchaseRepoDropDown
    {
        public PurchaseReportModel details { get; set; }

        public IEnumerable<SelectListItem> SupplierList { get; set; }
    }
}
