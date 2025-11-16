using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class StockModel
    {
        public int PurchaseId { get; set; }
        [DisplayName("Supplier")]
        public string SupplierName { get; set; }
      
        public int SupplierId{ get; set; }
        [DisplayName("Product")]
        public string ProductName { get; set; }
       

        public int ProductId { get; set; }
        [DisplayName("Quantity")]
        public int Count{ get; set; }
        [DisplayName("Total Cost")]
        public decimal TotalCost { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
