namespace InventoryManagementSystem.Models
{
    public class SalesDetails
    {

        public string SalesDate { get; set; }
        public string CustomerName { get; set; }
        public List<ProductList> ProductsSold { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }

    }
    public class ProductList 
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }

    public class SalesReportDetail
    {
        public string SalesDate { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PerPrice { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
