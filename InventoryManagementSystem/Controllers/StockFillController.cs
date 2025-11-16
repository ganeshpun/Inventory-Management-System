using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class StockFillController : Controller
    {
        private readonly IConfiguration _configuration;

        public StockFillController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: StockFillController
        public ActionResult StockList()
        {
            try
            {
                var responseData = new List<StockModel>();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "Purchase_List";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                responseData.Add(new StockModel()
                                {
                                    PurchaseId = Convert.ToInt32(reader["Purchase_Id"]),
                                    SupplierName = reader["SupplierName"].ToString(),
                                    ProductName = reader["Prod_Name"].ToString(),
                                    Count = Convert.ToInt32(reader["Count"]),
                                    TotalCost = (reader.GetDecimal(reader.GetOrdinal("per_Product_Cost"))),
                                    Date = Convert.ToDateTime(reader["Date"].ToString())
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

        // GET: StockFillController/Details/5
        public ActionResult Details(int id)
        {
            StockModel responseData = GetStockById(id);

            return View(responseData);

        }
        private StockModel GetStockById(int id)
        {
            var responseData = new StockModel();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select Purchase_Id,SupplierId,ProductId,Count,per_Product_Cost,Date from Purchase_tb where Purchase_Id=@PurchaseId";
                    cmd.Parameters.Add(new SqlParameter("@PurchaseId", id));

                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            responseData.PurchaseId = Convert.ToInt32(reader["Purchase_Id"]);
                            responseData.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                            responseData.ProductId = Convert.ToInt32(reader["ProductId"]);
                            responseData.Count = Convert.ToInt32(reader["Count"]);
                            responseData.TotalCost = (reader.GetDecimal(reader.GetOrdinal("per_Product_Cost")));
                            responseData.Date = Convert.ToDateTime(reader["Date"].ToString());



                        }

                    }

                }
                con.Close();

            }

            return responseData;
        }

        // GET:StockFillController/Create
        public ActionResult Create()
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
            StockRelatedComponents stockRelatedcomponents = new StockRelatedComponents()
            {
                details = new StockModel(),
                ProductList = items.Select
            (u =>
                new SelectListItem
                {
                    Text = u.Prod_Name,
                    Value = u.ProductId.ToString()
                }
            ),
                SupplierList = item.Select
            (u =>
                new SelectListItem
                {
                    Text = u.SupplierName,
                    Value = u.SupplierID.ToString()
                }
            )

            };
            return View(stockRelatedcomponents);
        }

        // POST: StockFillController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockRelatedComponents stockRelatedComponents)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_Insert_stock";
                        cmd.Parameters.Add(new SqlParameter("@SupplierId", stockRelatedComponents.details.SupplierId));
                        cmd.Parameters.Add(new SqlParameter("@ProductId", stockRelatedComponents.details.ProductId));
                        cmd.Parameters.Add(new SqlParameter("@Count", stockRelatedComponents.details.Count));
                        cmd.Parameters.Add(new SqlParameter("@Cost", stockRelatedComponents.details.TotalCost));
                        cmd.Parameters.Add(new SqlParameter("@Date", stockRelatedComponents.details.Date));

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();
                }

                return RedirectToAction(nameof(StockList));
            }
            catch
            {
                return View();
            }
        }
        // GET: StockFillController/GetProductBySupplierID
        public ActionResult GetProductBySupplierID(int supplierId)
        {
            var items = new List<ProductDetails>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    // SQL query to retrieve data
                    cmd.Connection = con;
                    cmd.CommandText = "select ProductId,Prod_Name from Product_tb where supplier_id=@SupplierId";
                    cmd.Parameters.Add(new SqlParameter("@SupplierId", supplierId));
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
            return Json(items);

        }



        // GET: StockFillController/Edit/5
        public ActionResult Edit(int id)
        {
            StockModel responseData = GetStockById(id);

            return View(responseData);
        }

        // POST: StockFillController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockModel data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_update_stock";
                        cmd.Parameters.Add(new SqlParameter("@PurchaseId", data.PurchaseId));
                        cmd.Parameters.Add(new SqlParameter("@SupplierId", data.SupplierId));
                        cmd.Parameters.Add(new SqlParameter("@ProductId", data.ProductId));
                        cmd.Parameters.Add(new SqlParameter("@Count", data.Count));
                        cmd.Parameters.Add(new SqlParameter("@Cost", data.TotalCost));
                        cmd.Parameters.Add(new SqlParameter("@Date", data.Date));
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                    }
                    con.Close();

                }
                return RedirectToAction(nameof(StockList));
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        // GET: StockFillController/Delete/5
        public ActionResult Delete(int id)
        {
            StockModel responseData = GetStockById(id);

            return View(responseData);
        }

        // POST: StockFillController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, StockModel collection)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "delete from purchase_tb where Purchase_Id= @PurchaseId";
                        cmd.Parameters.Add(new SqlParameter("@PurchaseId", id));
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();

                }
                return RedirectToAction(nameof(StockList));
            }
            catch
            {
                return View();
            }


        }

        
    }
}

