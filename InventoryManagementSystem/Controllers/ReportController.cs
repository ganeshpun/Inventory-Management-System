using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;
        public ReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: ReportController/PurchaseReport
        public ActionResult PurchaseReport()
        {
            var items = new List<SupplierDetails>();
            var a = _configuration.GetConnectionString("DefaultConnection");
            // Establish a connection to the database
            using (SqlConnection con = new SqlConnection(a))
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
                        items.Add(new SupplierDetails
                        {

                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            SupplierName = reader["SupplierName"].ToString()
                        });
                    }

                    reader.Close();
                }
            }
            PurchaseRepoDropDown purchaseRepoDropDown = new PurchaseRepoDropDown()
            {
                details = new PurchaseReportModel(),
                SupplierList = items.Select
            (u =>
                new SelectListItem
                {
                    Text = u.SupplierName,
                    Value = u.SupplierID.ToString()
                }
            )
            };
            return View(purchaseRepoDropDown);
        }

        // POST: ReportController/PurchaseReport
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult PurchaseReport([FromBody] PurchaseReportModel purchaseRepo)
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
                        cmd.CommandText = "sp_SupplierPurchaseReport";
                        cmd.Parameters.Add(new SqlParameter("@PurchaseId", purchaseRepo.PurchaseId));
                        cmd.Parameters.Add(new SqlParameter("@From", purchaseRepo.From));
                        cmd.Parameters.Add(new SqlParameter("@To", purchaseRepo.To));
                        cmd.Parameters.Add(new SqlParameter("@SupplierId", purchaseRepo.SupplierId));

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                       SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                responseData.Add(new StockModel() {
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

                return Json(responseData);
            }
            catch
            {
                return View();
            }
        }


        // GET: ReportController/SalesReport
        public ActionResult SalesReport()
        {
                     
            return View();
        }

        // POST: ReportController/SalesReport
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult SalesReport([FromBody] PurchaseReportModel sales)
        {
            try
            {
                var responseData = new List<SalesReportDetail>();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_SalesReport";
                        cmd.Parameters.Add(new SqlParameter("@From", sales.From));
                        cmd.Parameters.Add(new SqlParameter("@To", sales.To));

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                responseData.Add(new SalesReportDetail()
                                {
                                    
                                    SalesDate = reader["SalesDate"].ToString(),
                                    CustomerName= reader["CustomerName"].ToString(),
                                    ProductName = reader["Productname"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    PerPrice = reader.GetDecimal(reader.GetOrdinal("Per_price")),
                                    TotalPrice = (reader.GetDecimal(reader.GetOrdinal("TotalPrice"))),
                                });



                            }

                        }


                    }
                    con.Close();
                }

                return Json(responseData);
            }
            catch
            {
                return View();
            }
        }

        //GET: ReportController/ProfitLossReport
        public ActionResult ProfitLossReport()
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
            };
            return View(stockRelatedcomponents);
        }
        // POST: ReportController/ProfitLossReport
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult ProfitLossreport([FromBody] PurchaseReportModel profitloss)
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
                        cmd.CommandText = "SP_PROFITLOSSREPORT";
                        cmd.Parameters.Add(new SqlParameter("@FROMDATE", profitloss.From));
                        cmd.Parameters.Add(new SqlParameter("@TODATE", profitloss.To));
                        cmd.Parameters.Add(new SqlParameter("@PRODUCTID", profitloss.SupplierId));

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                responseData.Add(new StockModel()
                                {
                                    ProductName = reader["PRODUCTNAME"].ToString(),
                                    Count = Convert.ToInt32(reader["QUANTITY"]),
                                    TotalCost = (reader.GetDecimal(reader.GetOrdinal("TOTALPRICE"))),
                                    Date = Convert.ToDateTime(reader["TRANSACTIONDATE"].ToString())
                                });



                            }

                        }


                    }
                    con.Close();
                }

                return Json(responseData);
            }
            catch
            {
                return View();
            }

        }

    }
}
