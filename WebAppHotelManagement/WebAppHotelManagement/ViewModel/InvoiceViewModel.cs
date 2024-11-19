using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppHotelManagement.Models;

namespace WebAppHotelManagement.ViewModel
{

    public class InvoiceViewModel
    {
        private HotelDBEntities objectDB;

        public InvoiceViewModel()
        {
            objectDB = new HotelDBEntities();
        }

        public List<Invoid> ListAll()
        {
            var list =  objectDB.Database.SqlQuery<Invoid>("sp_Invoice_ListAll").ToList();
            return list;
        }

        public List<Invoid> ListAll_0()
        {
            var list = objectDB.Database.SqlQuery<Invoid>("sp_Invoice_ListAll_0").ToList();
            return list;
        }
        public List<Invoid> ListAll_1()
        {
            var list = objectDB.Database.SqlQuery<Invoid>("sp_Invoice_ListAll_1").ToList();
            return list;
        }

        public int ChangeActive(int id)
        {
            Invoid inv = objectDB.Invoids.Single(model => model.InvoidID == id);
            inv.IsActive = false;
            RoomBooking rb = objectDB.RoomBookings.Single(model => model.BookingId == inv.BookingID);
            Room r = objectDB.Rooms.Single(model => model.RoomId== rb.AssignRoomId);
            r.IsActive = false;
            r.BookingStatusId = 1;
            objectDB.SaveChanges();
            return 1;
        }

        public int InvoidID { get; set; }
        public Nullable<int> BookingID { get; set; }
        public Nullable<decimal> BookingAmount { get; set; }
        public Nullable<decimal> ServiceAmount { get; set; }
        public Nullable<bool> IsActive { get; set; }

    }
}