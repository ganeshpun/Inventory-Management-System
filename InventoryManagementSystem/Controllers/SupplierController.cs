using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {  
        private readonly IConfiguration _configuration;
        public SupplierController(IConfiguration configuration)
        {
                _configuration = configuration;
        }
        // GET: SupplierController
        //[Authorize]
        public ActionResult SupplierList()
        {
            try
            {
                var responseData = new List<SupplierDetails>();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select *from supplier_tb";
                        cmd.CommandType = System.Data.CommandType.Text;
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                responseData.Add(new SupplierDetails()
                                {
                                    SupplierID = Convert.ToInt32(reader["SupplierID"]),
                                    SupplierName = reader["SupplierName"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Email = (reader["Email"].ToString()),
                                    Contact = (reader["Contact"].ToString())

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

                throw ex;
            }
          
        }

        // GET: SupplierController/Details/5
        public ActionResult Details(int id)
        {
            SupplierDetails responseData = GetSupplierById(id);

            return View(responseData);
        }

        private SupplierDetails GetSupplierById(int id)
        {
            var responseData = new SupplierDetails();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select SupplierID,SupplierName,Address,Email,Contact from supplier_tb where SupplierID=@supplierId";
                    cmd.Parameters.Add(new SqlParameter("@supplierId", id));
                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            responseData.SupplierID = Convert.ToInt32(reader["SupplierID"]);
                            responseData.SupplierName = (reader["SupplierName"].ToString());
                            responseData.Address = (reader["Address"].ToString());
                            responseData.Email = (reader["Email"].ToString());
                            responseData.Contact = (reader["Contact"].ToString());


                        }

                    }

                }
                con.Close();

            }

            return responseData;
        }

        // GET: SupplierController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierDetails data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_Insert_supplier";
                        cmd.Parameters.Add(new SqlParameter("@supplierName", data.SupplierName));
                        cmd.Parameters.Add(new SqlParameter("@Address", data.Address));
                        cmd.Parameters.Add(new SqlParameter("@Email", data.Email));
                        cmd.Parameters.Add(new SqlParameter("@Contact", data.Contact));
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                       

                    }
                    con.Close();

                }
                return RedirectToAction(nameof(SupplierList));
            }
            catch
            {
                return View();
            }
        }

        // GET: SupplierController/Edit/5
        public ActionResult Edit(int id)
        {
            SupplierDetails responseData = GetSupplierById(id);

            return View(responseData);
        }

        // POST: SupplierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupplierDetails data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_update_supplier";
                        cmd.Parameters.Add(new SqlParameter("@supplierId", data.SupplierID));
                        cmd.Parameters.Add(new SqlParameter("@supplierName", data.SupplierName));
                        cmd.Parameters.Add(new SqlParameter("@Address", data.Address));
                        cmd.Parameters.Add(new SqlParameter("@Email", data.Email));
                        cmd.Parameters.Add(new SqlParameter("@Contact", data.Contact));
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();

                }
                return RedirectToAction(nameof(SupplierList));
            }
            catch
            {
                return View();
            }
        }

        // GET: SupplierController/Delete/5
        public ActionResult Delete(int id)
        {
            SupplierDetails responseData = GetSupplierById(id);

            return View(responseData);
        }

        // POST: SupplierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, SupplierDetails collection)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "delete from supplier_tb where SupplierID= @supplierId";
                        cmd.Parameters.Add(new SqlParameter("@supplierId", id));
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();

                }
                return RedirectToAction(nameof(SupplierList));
            }
            catch
            {
                return View();
            }
        }
    }
}
