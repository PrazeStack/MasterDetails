using MasterDetails.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterDetails.Controllers
{
    public class DataTableController : Controller
    {
        public MasterDataEntities db;
        public DataTableController()
        {
            db = new MasterDataEntities();
        }

        // GET: DataTable
        public ActionResult Index()
        {
            SqlConnection Con = new SqlConnection();
            string path = ConfigurationManager.ConnectionStrings["dbPath"].ConnectionString;
            Con.ConnectionString =  path;
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM[dbo].[OrderMaster]", Con);
                adp.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }
             return View(dt.Rows);
           
        }
        public ActionResult NewOrder(Guid MasterId)
        {
            var id = MasterId.ToString().ToUpper();
            SqlConnection Con = new SqlConnection();
            string path = ConfigurationManager.ConnectionStrings["dbPath"].ConnectionString;
            Con.ConnectionString = path;
            DataTable dt = new DataTable();
            try
            {
                string Command = $"SELECT * FROM [dbo].[OrderDetails] WHERE MasterId ='{id}'";
                SqlDataAdapter adp = new SqlDataAdapter( Command, Con);
                adp.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }
            return PartialView("_NewOrder",dt);        }
    }
}