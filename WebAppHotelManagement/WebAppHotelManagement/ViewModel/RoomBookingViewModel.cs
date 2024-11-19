using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WebAppHotelManagement.ViewModel
{
    public class RoomBookingViewModel
    {
       
        public int BookingId { get; set; }

       
        public string CustomerName { get; set; }

       
        public string CustomerPhone { get; set; }

        public string CustomerAddress { get; set; }

        
        public int NoOfMembers { get; set; }

       

        public decimal RoomPrice { get; set; }

       
        public DateTime BookingFrom { get; set; }

     
        
        public DateTime BookingTo { get; set; }

      
        public String RoomNumber { get; set; }
  
        public decimal TotalAmount { get; set; }

       
        public int NumberOfDays { get; set; }

        
    }
}