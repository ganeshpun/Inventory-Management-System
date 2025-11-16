using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult ProductCountFororDashboard() 
        {
            List<ProductDetails> responseData = GetProductList();
            return Json(responseData); 
        }


        // GET: ProductController
        public ActionResult ProductList()
        {
            try
            {
                List<ProductDetails> responseData = GetProductList();

                return View(responseData);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private List<ProductDetails> GetProductList()
        {
            var responseData = new List<ProductDetails>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select ProductId,Prod_Name,Prod_Quantity,isnull(Cost_Price,0)Cost_Price from Product_tb";
                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            responseData.Add(new ProductDetails()
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Prod_Name = (reader["Prod_Name"].ToString()),
                                Prod_Quantity = Convert.ToInt32(reader["Prod_Quantity"]),
                                PerProdCostPrice = Convert.ToDecimal(reader["Cost_Price"])

                            });

                        }

                    }

                }
                con.Close();

            }

            return responseData;
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            ProductDetails responseData = GetProductById(id);

            return View(responseData);
        }

        private ProductDetails GetProductById(int id)
        {
            var responseData = new ProductDetails();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select ProductId,Prod_Name,Prod_Quantity,isnull(Cost_Price,0)Cost_Price from Product_tb where ProductId=@ProductId";
                    cmd.Parameters.Add(new SqlParameter("@ProductId", id));
                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            responseData.ProductId = Convert.ToInt32(reader["ProductId"]);
                            responseData.Prod_Name = (reader["Prod_Name"].ToString());
                            responseData.Prod_Quantity = Convert.ToInt32(reader["Prod_Quantity"]);
                            responseData.PerProdCostPrice = (reader.GetDecimal(reader.GetOrdinal("Cost_Price")));
                           


                        }

                    }

                }
                con.Close();

            }

            return responseData;
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var item = new List<SupplierDetails>();

            // Establish a connection to the database
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    // SQL query to retrieve data
                    cmd.Connection = con;
                    cmd.CommandText = "select SupplierID,SupplierName from supplier_tb";
                    cmd.CommandType = System.Data.CommandType.Text;
                    // Execute the query and read the data
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Add items to the list
                        item.Add(new SupplierDetails
                        {

                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            ProductRelatedComponents productRelatedcomponents = new ProductRelatedComponents()
            {
                details = new ProductDetails(),

                SupplierList = item.Select
                  (u =>
                      new SelectListItem
                      {
                          Text = u.SupplierName,
                          Value = u.SupplierID.ToString()
                      }
                  )

            };
            return View(productRelatedcomponents);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductRelatedComponents data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_Insert_product";
                        cmd.Parameters.Add(new SqlParameter("@Prod_Name", data.details.Prod_Name));
                        cmd.Parameters.Add(new SqlParameter("@SupplierId", data.details.SupplierId));
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();

                }
                return RedirectToAction(nameof(ProductList));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            ProductDetails responseData = GetProductById(id);

            return View(responseData);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDetails data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_update_product";

                        cmd.Parameters.Add(new SqlParameter("@ProductId", data.ProductId));
                        cmd.Parameters.Add(new SqlParameter("@Prod_Name", data.Prod_Name));
                        // cmd.Parameters.Add(new SqlParameter("@Prod_Quantity", data.Prod_Quantity));
                        //cmd.Parameters.Add(new SqlParameter("@Prod_Price", data.Prod_Price));
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();

                }
                return RedirectToAction(nameof(ProductList));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            ProductDetails responseData = GetProductById(id);

            return View(responseData);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ProductDetails collection)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "delete from Product_tb where ProductId = @ProductId";
                        cmd.Parameters.Add(new SqlParameter("@ProductId", id));
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();

                }
                return RedirectToAction(nameof(ProductList));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ProductSellingPriceAdd() 
        {
            var items = new List<ProductDetails>();

            // Establish a connection to the database
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    // SQL query to retrieve data
                    cmd.Connection = con;
                    cmd.CommandText = "select ProductId,Prod_Name from Product_tb";
                    cmd.CommandType = System.Data.CommandType.Text;
                    // Execute the query and read the data
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Add items to the list
                        items.Add(new ProductDetails
                        {

                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Prod_Name = reader["Prod_Name"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            ProductRelatedComponents productRelatedcomponents = new ProductRelatedComponents()
            {
                details = new ProductDetails(),
                ProductList = items.Select
            (u =>
                new SelectListItem
                {
                    Text = u.Prod_Name,
                    Value = u.ProductId.ToString()
                }
            )

            };
            return View(productRelatedcomponents); 
        }

    }
}
