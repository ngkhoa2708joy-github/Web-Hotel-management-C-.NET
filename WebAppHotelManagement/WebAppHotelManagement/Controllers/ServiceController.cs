using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebAppHotelManagement.Models;
using WebAppHotelManagement.ViewModel;

namespace WebAppHotelManagement.Controllers
{
    public class ServiceController : Controller
    {
        private HotelDBEntities objHotelDBEntities;

        public ServiceController()
        {
            objHotelDBEntities = new HotelDBEntities();
        }
        // GET: Service
        [Authorize(Roles = ("Admin,Staff"))]
        public ActionResult Index()
        {
            var list = objHotelDBEntities.Database.SqlQuery<CustomerService>("Sp_service_ListAll").ToList();
            return View(list);
        }

        [Authorize(Roles = ("Admin"))]
        // GET: Service/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public ActionResult Create(ServiceViewModel serviceViewModel)
        {
            CustomerService cus = new CustomerService()
            {
                ServiceName = serviceViewModel.ServiceName,
                ServicePrice = serviceViewModel.ServicePrice,
            };

            objHotelDBEntities.CustomerServices.Add(cus);
            objHotelDBEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Service/Edit/5
        [Authorize(Roles = ("Admin"))]
        public ActionResult Edit(int id)
        {
            var Service = objHotelDBEntities.CustomerServices.Single(model => model.ServiceId == id);
            return View(Service);
        }

        // POST: Service/Edit/5
        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public ActionResult Edit(int id, CustomerService customer)
        {
            var customerOld = objHotelDBEntities.CustomerServices.Find(id);

            if (customer.ServicePrice > 1000 || customer.ServicePrice < 300)
            {
                ModelState.AddModelError("ServicePrice", "Price must in range 300 to 1000");
                return View("Edit");
            }
            objHotelDBEntities.CustomerServices.Remove(customerOld);
            objHotelDBEntities.CustomerServices.Add(customer);

            objHotelDBEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Service/Delete/5
        [Authorize(Roles = ("Admin"))]
        public ActionResult Delete(int id)
        {
            var Service = objHotelDBEntities.CustomerServices.Single(model => model.ServiceId == id);
            return View(Service);
        }

        // POST: Service/Delete/5
        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public ActionResult Delete(int id, CustomerService cus)
        {
            cus = objHotelDBEntities.CustomerServices.Find(id);
            objHotelDBEntities.CustomerServices.Remove(cus);
            objHotelDBEntities.SaveChanges();
            return RedirectToAction("Index");

        }



        [Authorize(Roles = ("Admin,Staff"))]
        public ActionResult BookingService(int id)
        {
            var listRoomBooking = objHotelDBEntities.Database.SqlQuery<RoomBooking>("Sp_get_list_roomBooking_id").ToList();

            ViewBag.ServiceId = id;
            return View(listRoomBooking);
        }

        [Authorize(Roles = ("Admin,Staff"))]
        public ActionResult DetailBookingService()
        {
            string readQueryStringIdS = Request.QueryString["idService"];
            string readQueryStringIdB = Request.QueryString["idBooking"];

            int idService = Convert.ToInt32(readQueryStringIdS);
            int idBooking = Convert.ToInt32(readQueryStringIdB);

            ServiceServed serviceServed = new ServiceServed()
            {
                ServiceID= idService,
                BookingId= idBooking,
            };


            return View(serviceServed);
        }


        [HttpPost]
        [Authorize(Roles = ("Admin,Staff"))]
        public ActionResult DetailBookingService(ServiceServed served)
        {
            

            ServiceServed serviceServed = new ServiceServed()
            {
                ServiceID = served.ServiceID,
                BookingId = served.BookingId,
                Amount = served.Amount
            };

            objHotelDBEntities.ServiceServeds.Add(serviceServed);
            objHotelDBEntities.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
