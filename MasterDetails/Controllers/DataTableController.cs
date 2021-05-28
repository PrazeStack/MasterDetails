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
        public DataTable CustomerTable;
        public DataTable DetailsTable;

        public DataTableController()
        {
            
            db = new MasterDataEntities();
            CustomerTable = GetCustomerTable();
            DetailsTable = GetDetailsTable();

        }

        public DataTable GetDetailsTable()
        {
            DataTable DetailsTable = new DataTable();
            DetailsTable.Columns.Add("MasterId");
            DetailsTable.Columns.Add("DetailsId");
            DetailsTable.Columns.Add("ProductName");
            DetailsTable.Columns.Add("Quantity");
            DetailsTable.Columns.Add("Amount");

            var details = db.OrderDetails.ToList();
            foreach(var item in details)
            {
                DetailsTable.Rows.Add(item.MasterId,item.DetailsId, item.ProductName, item.Quantity, item.Amount);
            }
            return DetailsTable;

        }
        public DataTable GetCustomerTable()
        {

            DataTable CustomerTable = new DataTable();
            CustomerTable.Columns.Add("MasterId");
            CustomerTable.Columns.Add("CustomerName");
            CustomerTable.Columns.Add("Adddress");
            CustomerTable.Columns.Add("OrderDate");

            var master = db.OrderMasters.ToList();
            foreach (var customer in master)
            {
                CustomerTable.Rows.Add(customer.MasterId, customer.CustomerName, customer.Address, customer.OrderDate);
            }


            return CustomerTable;
        }

        // GET: DataTable
        public ActionResult Index()
        {

  
                return View(CustomerTable);
           
        }
        public ActionResult GetOrderList(Guid id)
        {
            var items = DetailsTable.Select($"MasterId = '{id}'").ToList();

            
            return PartialView("_Order",items);

        }


        [HttpPost]
        public ActionResult updateDetails(OrderDetailViewModel item)
        {
            if (item != null)
            {


                DetailsTable.Rows.Add(item.MasterId, item.DetailsId, item.ProductName, item.Quantity, item.Amount);
                return null;
            }
            else
            {
                return null;
            }

        }

        public ActionResult NewOrder()
        {
           
            return PartialView("_NewOrder");
        }
    }
    
}
