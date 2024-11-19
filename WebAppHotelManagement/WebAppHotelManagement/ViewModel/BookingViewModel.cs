using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAppHotelManagement.ViewModel
{
    public class BookingViewModel
    {

        [Display(Name = "Customer's Name")]
        [Required(ErrorMessage = "Customer's Name is required.")]
        public String CustomerName { get; set; }

        [Display(Name = "Customer's Address")]
        [Required(ErrorMessage = "Customer's Address is required.")]
        public string CustomerAddress { get; set; }
        [Display(Name = "Customer's Phone")]
        [Required(ErrorMessage = "Customer's Phone is required.")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Booking From")]
        [Required(ErrorMessage = "Booking From is required.")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingFrom { get; set; }

        [Display(Name = "Booking To")]
        [Required(ErrorMessage = "Booking To is required.")]
        [DataType(DataType.Date)]

        public DateTime BookingTo { get; set; }

        [Display(Name = "Assign Room")]
        [Required(ErrorMessage = "Assign Room is required.")]
        public int AssignRoomId { get; set; }

        [Display(Name = "Number of Member")]
        [Required(ErrorMessage = "Number of Member is required.")]
        public int NumberOfMembers { get; set; }
        public IEnumerable<SelectListItem> ListOfRooms { get; set; }
    }
}