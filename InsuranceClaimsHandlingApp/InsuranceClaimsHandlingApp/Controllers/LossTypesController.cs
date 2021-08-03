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
    public class LossTypesController : Controller
    {
        // GET: LossTypesController
        public ActionResult Index()
        {
            if (TempData["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            List<LossTypes> losstypes = new List<LossTypes>();
            SqlConnection connection = null;
            SqlCommand sqlCommand = null;
            SqlDataReader results = null;

            try
            {
                connection = SQLConnectionHelper.GetSQLConnection();
                sqlCommand = new SqlCommand("SELECT * FROM LossTypes", connection);
                connection.Open();
                results = sqlCommand.ExecuteReader();

                while (results.Read())
                {
                    losstypes.Add(new LossTypes()
                    {
                        LossTypeID = (int)results["LossTypeID"],
                        LossTypeCode = (string)results["LossTypeCode"],
                        LossTypeDescription = (string)results["LossTypeDescription"]
                    });
                }

                TempData.Keep();
                return View(losstypes);
            }
            catch
            {
                return Json(new
                {
                    status = "Not found"
                });
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

        public ActionResult LogOut()
        {
            TempData.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
