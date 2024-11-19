using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WebAppHotelManagement.ViewModel
{
    public class ServiceViewModel
    {

        [Required(ErrorMessage = "Customer's Service name is required.")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Customer's service price is required.")]
        [Range(300, 1000, ErrorMessage = "Price must range between 300 and 1000")]
        public Nullable<decimal> ServicePrice { get; set; }
    }
}