using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class PurchaseReportModel
    {
        public int PurchaseId { get; set; }
        public DateTime From { get; set; } = DateTime.Now;
        public DateTime To { get; set; } = DateTime.Now;

        [DisplayName("Supplier")]
        public int SupplierId { get; set; }
       
    }
}
