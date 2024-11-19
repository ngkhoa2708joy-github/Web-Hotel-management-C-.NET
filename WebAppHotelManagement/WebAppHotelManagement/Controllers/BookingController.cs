using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebAppHotelManagement.Models;
using WebAppHotelManagement.ViewModel;

namespace WebAppHotelManagement.Controllers
{
    [Authorize(Roles = ("Admin,Staff"))]
    public class BookingController : Controller
    {
        private HotelDBEntities objHotelDbEntities;

        public BookingController()
        {
            objHotelDbEntities = new HotelDBEntities();
        }

        public ActionResult Index()
        {
            BookingViewModel objBookingViewModel = new BookingViewModel();
            objBookingViewModel.ListOfRooms = (from objRoom in objHotelDbEntities.Rooms
                                               where objRoom.IsActive == false
                                               select new SelectListItem()
                                               {
                                                   Text = objRoom.RoomNumber,
                                                   Value = objRoom.RoomId.ToString()
                                               }).ToList();
            objBookingViewModel.BookingFrom = DateTime.Now;
            objBookingViewModel.BookingTo = DateTime.Now.AddDays(4);

            return View(objBookingViewModel);
        }

        [HttpPost]
        public ActionResult Index(BookingViewModel objBookingViewModel)
        {
            int numberOfDays = Convert.ToInt32((objBookingViewModel.BookingTo - objBookingViewModel.BookingFrom).TotalDays);
            Room objRoom = objHotelDbEntities.Rooms.Single(model => model.RoomId == objBookingViewModel.AssignRoomId);
            decimal RoomPrice = objRoom.RoomPrice;
            decimal TotalAmount = RoomPrice * numberOfDays;

            RoomBooking roomBooking = new RoomBooking()
            {
                BookingFrom= objBookingViewModel.BookingFrom,
                BookingTo= objBookingViewModel.BookingTo,
                AssignRoomId=objBookingViewModel.AssignRoomId,
                CustomerAddress=objBookingViewModel.CustomerAddress,
                CustomerName =objBookingViewModel.CustomerName,
                CustomerPhone =objBookingViewModel.CustomerPhone,
                NoOfMembers = objBookingViewModel.NumberOfMembers,
                TotalAmount = TotalAmount
            };

            objHotelDbEntities.RoomBookings.Add(roomBooking); 
            objRoom.IsActive= true;
            objHotelDbEntities.SaveChanges();

            objRoom.BookingStatusId = 3;
            objHotelDbEntities.SaveChanges();
            return Json(new {message ="Hotel Booking is successfully Created!", success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetAllBookingHistory() 
        {
            List <RoomBookingViewModel> listOfBookingViewModels = new List<RoomBookingViewModel>();
            listOfBookingViewModels = (from objHotelBooking in objHotelDbEntities.RoomBookings
                                       join objRoom in objHotelDbEntities.Rooms on objHotelBooking.AssignRoomId equals objRoom.RoomId
                                       orderby objHotelBooking.BookingId descending
                                       select new RoomBookingViewModel()
                                       {
                                           BookingFrom = objHotelBooking.BookingFrom,
                                           BookingTo = objHotelBooking.BookingTo,
                                           CustomerName = objHotelBooking.CustomerName,
                                           CustomerPhone = objHotelBooking.CustomerPhone,
                                           CustomerAddress = objHotelBooking.CustomerAddress,
                                           TotalAmount = (decimal)objHotelBooking.TotalAmount,
                                           NoOfMembers = objHotelBooking.NoOfMembers,
                                           BookingId = objHotelBooking.BookingId,
                                           RoomNumber = objRoom.RoomNumber,
                                           RoomPrice= objRoom.RoomPrice,
                                           NumberOfDays = System.Data.Entity.DbFunctions.DiffDays( objHotelBooking.BookingFrom, objHotelBooking.BookingTo).Value
                                       }).ToList();
            return PartialView("_BookingHistoryPartial", listOfBookingViewModels);
        }
    }
}