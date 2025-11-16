using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly IConfiguration _configuration;

        public SalesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ActionResult SalesList()
        {
            try
            {
                var responseData = new List<SalesDetails>();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select *from Sales_mast_tb";
                        cmd.CommandType = System.Data.CommandType.Text;
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                responseData.Add(new SalesDetails()
                                {
                                    SalesDate = reader["SalesDate"].ToString(),
                                    CustomerName = reader["CustomerName"].ToString(),
                                    TotalPrice = (reader.GetDecimal(reader.GetOrdinal("TotalPrice"))),
                                    Vat = Convert.ToInt32(reader["Vat"]),
                                    Discount = Convert.ToInt32(reader["Discount"])
                                   
                                });
                            }
                        }
                    }
                    con.Close();
                }

                return View(responseData);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
      
        // GET: SalesController/Create
        public ActionResult ProductsSoldAdd()
        {
            return View();
        }

        //POST: SalesController/Create
       [HttpPost]
       //[ValidateAntiForgeryToken]
        public ActionResult ProductsSoldAdd([FromBody]  SalesDetails inputData)
        {
            var items = new List<SalesDetails>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    // SQL query to retrieve data
                    cmd.Connection = con;
                    cmd.CommandText = "SP_SALES_INSERT";
                    cmd.Parameters.Add(new SqlParameter("@SALESDATE", inputData.SalesDate));
                    cmd.Parameters.Add(new SqlParameter("@CUSTOMERNAME", inputData.CustomerName));
                    cmd.Parameters.Add(new SqlParameter("@VAT", inputData.Vat));
                    cmd.Parameters.Add(new SqlParameter("@TOTALPRICE", inputData.TotalPrice));
                    cmd.Parameters.Add(new SqlParameter("@DISCOUNT", inputData.Discount));
                  
                    cmd.Parameters.Add(new SqlParameter("@ProductList",JsonConvert.SerializeObject(inputData.ProductsSold)));
                    
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    // Execute the query and read the data
                    cmd.ExecuteNonQuery();
                    

                }
            }
            return Json(items);

        }

        public ActionResult GetCustomerByName(string Customer_name)
        {
            var items = new List<CustomerDetails>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    // SQL query to retrieve data
                    cmd.Connection = con;
                    cmd.CommandText = "select CustomerID,CustomerName,Email,Phone from customer_tb where CustomerName like @Customer_name+'%'";
                    cmd.Parameters.Add(new SqlParameter("@Customer_name", Customer_name));
                    cmd.CommandType = System.Data.CommandType.Text;
                    // Execute the query and read the data
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Add items to the list
                        items.Add(new CustomerDetails
                        {

                            CustomerID = Convert.ToInt32(reader["CustomerID"]),
                            CustomerName = reader["CustomerName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            return Json(items);

        }

        public ActionResult GetProductByName(string productname)
        {
            var items = new List<ProductDetails>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    // SQL query to retrieve data
                    cmd.Connection = con;
                    cmd.CommandText = "select ProductId,Prod_Name,Prod_Quantity from Product_tb where Prod_Name like @productname+'%'";
                    cmd.Parameters.Add(new SqlParameter("@productname", productname));
                    cmd.CommandType = System.Data.CommandType.Text;
                    // Execute the query and read the data
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Add items to the list
                        items.Add(new ProductDetails
                        {

                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Prod_Name = reader["Prod_Name"].ToString(),
                            Prod_Quantity= Convert.ToInt32(reader["Prod_Quantity"])

                        });
                    }

                    reader.Close();
                }
            }
            return Json(items);

        }

        // GET: SalesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SalesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SalesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
