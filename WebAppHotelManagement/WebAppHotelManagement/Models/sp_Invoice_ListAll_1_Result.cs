//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppHotelManagement.Models
{
    using System;
    
    public partial class sp_Invoice_ListAll_1_Result
    {
        public int InvoidID { get; set; }
        public Nullable<int> BookingID { get; set; }
        public Nullable<decimal> BookingAmount { get; set; }
        public Nullable<decimal> ServiceAmount { get; set; }
        public Nullable<decimal> ToTalPayment { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
