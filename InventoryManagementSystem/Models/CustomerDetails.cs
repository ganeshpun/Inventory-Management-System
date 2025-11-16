using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class CustomerDetails
    {
        public int CustomerID { get; set; }

        [StringLength(20, ErrorMessage = "The field must be a string with a maximum length of 20.")]
        [Required(ErrorMessage = "CustomerName field is required.")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "CustomerName can only contain letters.")]
        [DisplayName("Customer")]
        public string CustomerName { get; set; }

        [StringLength(20, ErrorMessage = "The field must be a string with a maximum length of 20.")]
        [Required(ErrorMessage = "Address field is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email field is required.")]
        public string Email { get; set; }

        [StringLength(10, ErrorMessage = "The field must be a string with a maximum length of 10.")]
        [Required(ErrorMessage = "Contact field is required.")]
        public string Phone { get; set; }
    }
}
