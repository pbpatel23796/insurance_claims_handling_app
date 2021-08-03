using InsuranceClaimsHandlingApp.DataContext;
using InsuranceClaimsHandlingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceClaimsHandlingApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users collection)
        {
            SqlConnection connection = null;
            SqlCommand sqlCommand = null;

            try
            {
                connection = SQLConnectionHelper.GetSQLConnection();
                sqlCommand = new SqlCommand("SELECT UserID FROM Users WHERE UserName = '" + collection.UserName + "' AND Password = '" + collection.Password + "'", connection);
                connection.Open();
                var result = sqlCommand.ExecuteScalar();

                if (result != null && (int)result > 0)
                {
                    Users loginUser = this.GetData().FirstOrDefault(x => x.UserName == collection.UserName);
                    TempData["error"] = null;
                    TempData["user"] = loginUser.DisplayName;
                    TempData.Keep();

                    if (loginUser.Active)
                    {
                        return RedirectToAction("Index", "LossTypes");
                    } else
                    {
                        TempData["error"] = "User is inactive";
                        TempData.Keep();
                        return RedirectToAction(nameof(Index));
                    }                    
                }
                else
                {   
                    TempData["error"] = "Incorrect credentials";
                    TempData.Keep();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                return View("Error", e.Message);
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    sqlCommand.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        [HttpGet]
        public List<Users> GetData()
        {
            List<Users> users = new List<Users>();
            SqlConnection connection = null;
            SqlCommand sqlCommand = null;
            SqlDataReader results = null;

            try
            {
                connection = SQLConnectionHelper.GetSQLConnection();
                sqlCommand = new SqlCommand("SELECT * FROM Users", connection);
                connection.Open();
                results = sqlCommand.ExecuteReader();

                while (results.Read())
                {
                    users.Add(new Users()
                    {
                        UserID = (int)results["UserID"],
                        UserName = (string)results["UserName"],
                        Password = (string)results["Password"],
                        DisplayName = (string)results["DisplayName"],
                        Active = (bool)results["Active"]
                    });
                }

                return users;
            }
            catch
            {
                return users;
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    sqlCommand.Dispose();
                    results.Close();
                    results.DisposeAsync();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}
