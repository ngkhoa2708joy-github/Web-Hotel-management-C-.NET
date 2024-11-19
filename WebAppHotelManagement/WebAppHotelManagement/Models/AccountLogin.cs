using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebAppHotelManagement.Models
{
    public class AccountLogin
    {
        public bool Login(string userName, string passWord)
        {
            using(OurDbContext db = new OurDbContext())
            {
                var check = (from p in db.userAccount where p.UserName == userName &&
                             p.Password == passWord select p);

                if(check != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}