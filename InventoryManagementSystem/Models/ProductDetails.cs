using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class ProductDetails
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        [StringLength(100, ErrorMessage = "The field must be a string with a maximum length of 20.")]
        [Required(ErrorMessage = "Prod_Name field is required.")]
        [DisplayName("Product")]
        public string Prod_Name { get; set; }
        [Required(ErrorMessage = "Prod_Quantity field is required.")]
        [DisplayName("Quantity")]
        public int Prod_Quantity { get; set; }
        [Required(ErrorMessage = "Prod_Price field is required.")]
        [DisplayName("Cost per product")]
        public decimal PerProdCostPrice { get; set; }
        public decimal PerProdSellingPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
    public class ProductRelatedComponents
    {
        public ProductDetails details { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }

        public IEnumerable<SelectListItem> SupplierList { get; set; }


    }
}
