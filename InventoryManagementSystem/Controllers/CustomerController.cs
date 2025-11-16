using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: CustomerController
        public ActionResult CustomerList()
        {
            try
            {
                var responseData = new List<CustomerDetails>();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select CustomerID,CustomerName,Address,Email,Phone from Customer_tb";
                        cmd.CommandType = System.Data.CommandType.Text;
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                responseData.Add(new CustomerDetails()
                                {
                                    CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                    CustomerName = reader["CustomerName"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"].ToString()
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

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            CustomerDetails responseData = GetCustomerById(id);

            return View(responseData);

        }
        private CustomerDetails GetCustomerById(int id)
        {
            var responseData = new CustomerDetails();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select CustomerID,CustomerName,Address,Email,Phone from Customer_tb where CustomerID=@customerId";
                    cmd.Parameters.Add(new SqlParameter("@customerId", id));

                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            responseData.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                            responseData.CustomerName = reader["CustomerName"].ToString();
                            responseData.Address = reader["Address"].ToString();
                            responseData.Email = (reader["Email"].ToString());
                            responseData.Phone = (reader["Phone"].ToString());


                        }

                    }

                }
                con.Close();

            }

            return responseData;
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerDetails data)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_Insert_customer";
                        cmd.Parameters.Add(new SqlParameter("@CustomerName", data.CustomerName));
                        cmd.Parameters.Add(new SqlParameter("@Address", data.Address));
                        cmd.Parameters.Add(new SqlParameter("@Email", data.Email));
                        cmd.Parameters.Add(new SqlParameter("@Phone", data.Phone));
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();
                }

                return RedirectToAction(nameof(CustomerList));
            }
            catch 
            {
                return View();
            }
        }



        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            CustomerDetails responseData = GetCustomerById(id);

            return View(responseData);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerDetails data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "sp_update_customer";
                        cmd.Parameters.Add(new SqlParameter("@customerId", data.CustomerID));
                        cmd.Parameters.Add(new SqlParameter("@CustomerName", data.CustomerName));
                        cmd.Parameters.Add(new SqlParameter("@Address", data.Address));
                        cmd.Parameters.Add(new SqlParameter("@Email", data.Email));
                        cmd.Parameters.Add(new SqlParameter("@Phone", data.Phone));
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();

                }
                return RedirectToAction(nameof(CustomerList));
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            CustomerDetails responseData = GetCustomerById(id);

            return View(responseData);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CustomerDetails collection)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "delete from Customer_tb where CustomerID= @customerId";
                        cmd.Parameters.Add(new SqlParameter("@customerId", id));
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();


                    }
                    con.Close();

                }
                return RedirectToAction(nameof(CustomerList));
            }
            catch
            {
                return View();
            }
        }
    }
}

