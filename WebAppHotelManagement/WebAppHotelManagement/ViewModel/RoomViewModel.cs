using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAppHotelManagement.ViewModel
{
    public class RoomViewModel
    {
        public int RoomId { get; set; }

        [Display(Name ="Room's No.")]
        [Required(ErrorMessage = "Room's number is requried!" )]
        public String RoomNumber { get; set; }

        [Display(Name = "Room's Image")]
        public string RoomImage { get; set; }

        [Display(Name = "Room's Price")]
        [Required(ErrorMessage = "Room's Price is requried!")]
        [Range(500,10000, ErrorMessage ="Room's Price should be equal or greater than {1}$.")]
        public decimal RoomPrice { get; set;}

        [Display(Name = "Booking Status")]
        [Required(ErrorMessage = "Booking Status is requried!")]
        public int BookingStatusId { get; set;}

        [Display(Name = "Room's Type")]
        [Required(ErrorMessage = "Room's Type is requried!")]
        public int RoomTypeId { get; set;}

        [Display(Name = "Room's Capacity")]
        [Required(ErrorMessage = "Room's Capacity is requried!")]
        [Range(1, 10, ErrorMessage = "Room's Capacity should be equal or greater than {1} people.")]
        public int RoomCapacity { get; set; }

        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "Room's Description")]
        [Required(ErrorMessage = "Room's Description is requried!")]
        public string RoomDescription { get; set;}

        public Nullable<bool> IsActive { get; set; }

        public List <SelectListItem> ListOfBookingStatus { get; set;}

        public List<SelectListItem> ListOfRoomType { get; set; }

        


    }
}