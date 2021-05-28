using MasterDetails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterDetails.Controllers
{
    public class HomeController : Controller
    {
        public MasterDataEntities db;
        public HomeController()
        {
            db = new MasterDataEntities();
        }

        public ActionResult saveOrder(OrderViewModel order)
        {
            var orderMaster = db.OrderMasters.FirstOrDefault(x => x.MasterId == order.MasterId);
            if (orderMaster != null)
            {
                var orders = db.OrderDetails.Where(x => x.MasterId == orderMaster.MasterId).ToList();

                db.OrderDetails.RemoveRange(orders);
                db.OrderMasters.Remove(orderMaster);
            }

            var newOrderMaster = new OrderMaster();
                newOrderMaster.CustomerName = order.CustomerName;
                newOrderMaster.OrderDate = DateTime.Now;
                newOrderMaster.Address = order.Address;
                newOrderMaster.MasterId = Guid.NewGuid();
            db.OrderMasters.Add(newOrderMaster);

            if (order.OrderDetails.Any())
            {


                var _orderDetail = order.OrderDetails.Select(item => new OrderDetail()
                {
                    DetailsId = Guid.NewGuid(),
                    MasterId = newOrderMaster.MasterId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Amount = item.Amount
                });
               
                db.OrderDetails.AddRange(_orderDetail);
            }
            try
            {
                if (db.SaveChanges() > 0)
                {
                    return Json(new { error = false, message = "Order Saved Successfully" });
                }
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = e.Message });
            }
            return Json(new { error = true, message = "An Unknown Error has Occured" });


        }

        public ActionResult getSingleOrder(Guid orderId)
        {
            var order = db.OrderMasters.FirstOrDefault(x => x.MasterId == orderId);
            var model = new OrderViewModel()
            {
                MasterId = order.MasterId,
                CustomerName = order.CustomerName,
                Address = order.Address
            };

            if (model != null)
            {

                model.OrderDetails = db.OrderDetails.Where(x => x.MasterId == model.MasterId).Select(x => new OrderDetailViewModel()
                {
                    MasterId = orderId,
                    DetailsId = x.DetailsId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    Amount = x.Amount
                }).ToList();
                
               
            }
                
            return Json((model), JsonRequestBehavior.AllowGet);

        }
      
        public ActionResult deleteOrder(Guid id)
        {
            var orders = db.OrderDetails.Where(x => x.MasterId == id).ToList();
            if(orders!= null)
                db.OrderDetails.RemoveRange(orders);

            var orderMaster = db.OrderMasters.FirstOrDefault(x => x.MasterId == id);
            if(orderMaster != null)
                db.OrderMasters.Remove(orderMaster);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult getOrders()
        {
           ;
            var model = (db.OrderMasters.ToList().Select(x => new
            {
                masterId = x.MasterId,
                customerName = x.CustomerName,
                address = x.Address,
                orderDate = x.OrderDate.ToString("D")//
            })).ToList();

            return Json(new
            {
              
                recordsFiltered = model.Count,
                recordsTotal = model.Count,
                data = model
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}