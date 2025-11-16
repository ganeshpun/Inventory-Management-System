using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class SupplierDetails
    {
        public int SupplierID { get; set; }

        [StringLength(100, ErrorMessage = "The field must be a string with a maximum length of 20.")]
        [Required(ErrorMessage = "SupplierName field is required.")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Supplier name can only contain letters.")]
        [DisplayName("Supplier")]
        public string SupplierName { get; set; }

        [StringLength(20, ErrorMessage = "The field must be a string with a maximum length of 20.")]
        [Required(ErrorMessage = "Address field is required.")]
        public string Address { get; set; }
      
        [Required(ErrorMessage = "Email field is required.")]
        public string Email { get; set; }

        [StringLength(10, ErrorMessage = "The field must be a string with a maximum length of 10.")]
        [Required(ErrorMessage = "Contact field is required.")]
        public string Contact { get; set; }
    }
}
