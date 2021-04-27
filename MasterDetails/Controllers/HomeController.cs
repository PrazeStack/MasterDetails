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
            if (order.MasterId == Guid.Empty)
            {
                var masterId = Guid.NewGuid();
                var orderMaster = new OrderMaster()
                {
                    MasterId = masterId,
                    CustomerName = order.CustomerName,
                    Address = order.Address,
                    OrderDate = DateTime.Now
                };
                db.OrderMasters.Add(orderMaster);
                

                if (order.OrderDetails.Any())
                {


                    foreach (var item in order.OrderDetails)
                    {
                        //var detailsId = Guid.NewGuid();
                        var orderDetails = new OrderDetail()
                        {
                            DetailsId = item.DetailsId,
                            MasterId = masterId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            Amount = item.Amount


                        };
                        db.OrderDetails.Add(orderDetails);
                    }
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
            else
            {
                var orderMaster = db.OrderMasters.FirstOrDefault(x => x.MasterId == order.MasterId);
                orderMaster.CustomerName = order.CustomerName;
                orderMaster.Address = order.Address;
                if (order.OrderDetails.Any())
                {
                    
                    foreach (var item in order.OrderDetails)
                    {
                        if (item.State == "New")
                        {
                            var orderDetail = new OrderDetail()
                            {
                                DetailsId = item.DetailsId,
                                MasterId = order.MasterId,
                                ProductName = item.ProductName,
                                Quantity = item.Quantity,
                                Amount = item.Amount,

                            };
                            db.OrderDetails.Add(orderDetail);
                        }
                    }
                }

                try
                {
                    if (db.SaveChanges() > 0)
                    {
                        return Json(new { error = false, message = "Order Updated Successfully" });
                    }
                }
                catch (Exception e)
                {
                    return Json(new { error = true, message = e.Message });
                }
                return Json(new { error = true, message = "An Unknown Error has Occured" });
            }

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
        public ActionResult getSingleOrderDetail(Guid orderId)
        {
            var orderDetails = db.OrderDetails.FirstOrDefault(x => x.DetailsId == orderId);
           
            var model = new OrderDetailViewModel()
            {
                 DetailsId = orderDetails.DetailsId,
                 Amount = orderDetails.Amount,
                 ProductName = orderDetails.ProductName,
                 Quantity = orderDetails.Quantity
                
            };

            return Json((model), JsonRequestBehavior.AllowGet);

        }
        public ActionResult updateSingleOrderDetail(OrderDetailViewModel data)
        {
            var model = db.OrderDetails.FirstOrDefault(x => x.DetailsId == data.DetailsId);
            model.Amount =  data.Amount;
            model.ProductName = data.ProductName;
            model.Quantity = data.Quantity;


            try
            {
                if (db.SaveChanges() > 0)
                {
                    return Json(new { error = false, message = "Order Updated Successfully" });
                }
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = e.Message });
            }
            return Json(new { error = true, message = "An Unknown Error has Occured" });

        }


        public ActionResult deleteOrderdetail(Guid id)
        {
            var order = db.OrderDetails.FirstOrDefault(x => x.DetailsId == id);
            if (order != null)
            {
                db.OrderDetails.Remove(order);
                db.SaveChanges();
                try
                {
                    if (db.SaveChanges() > 0)
                    {
                        return Json(new { error = false, message = "Order Deleted Successfully" });
                    }
                }
                catch (Exception e)
                {
                    return Json(new { error = true, message = e.Message });
                }
                return Json(new { error = true, message = "An Unknown Error has Occured" });
            }
            else
            {
                return Json(new { error = false, message = "Order Deleted Successfully" });
            }

        }

        public ActionResult deleteOrder(Guid id)
        {
            var orders = db.OrderDetails.Where(x => x.MasterId == id);
            foreach (var order in orders)
            {
                db.OrderDetails.Remove(order);
            }
            var orderMaster = db.OrderMasters.FirstOrDefault(x => x.MasterId == id);
            db.OrderMasters.Remove(orderMaster);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult getOrders()
        {
            //var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var model = (db.OrderMasters.ToList().Select(x => new
            {
                masterId = x.MasterId,
                customerName = x.CustomerName,
                address = x.Address,
                orderDate = x.OrderDate.ToString("D")//
            })).ToList();

            return Json(new
            {
                //draw = draw,
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