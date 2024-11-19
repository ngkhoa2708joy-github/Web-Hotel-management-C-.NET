using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppHotelManagement.Models;
using WebAppHotelManagement.ViewModel;

namespace WebAppHotelManagement.Controllers
{
    [Authorize(Roles = ("Admin,Staff"))]
    public class InvoiceController : Controller
    {
        private HotelDBEntities objHotelDBEntities;

        public InvoiceController()
        {
            objHotelDBEntities = new HotelDBEntities();
        }
        // GET: Invoice

        public ActionResult Index(int i = -1)
        {
            
            var iplInvoice = new InvoiceViewModel();
            var model = iplInvoice.ListAll(); 
            if (i == 1) 
            {
                model = iplInvoice.ListAll_1();
            }
            if (i == 0)
            {
                model = iplInvoice.ListAll_0();
            }
                
            return View(model);
        }


        public ActionResult ChangeActive(int id) {
            
            return View(objHotelDBEntities.Invoids.Find(id));
        }

        [HttpPost]
        public ActionResult ChangeActive(int id, Invoid invoid)
        {
            var iplInvoice = new InvoiceViewModel();
            iplInvoice.ChangeActive(id);
            return RedirectToAction("Index");
        }

        
    }
}