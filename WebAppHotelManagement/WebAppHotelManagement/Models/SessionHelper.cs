using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppHotelManagement.Models
{
    public class SessionHelper
    {
        public static void SetSession(UserAccount session)
        {
            HttpContext.Current.Session["loginSession"] = session;
        }

        public static UserAccount GetSession()
        {
            var session = HttpContext.Current.Session["loginSession"];
            if (session == null)
                return null;
            else
            {
                return session as UserAccount;
            }
        }
    }
}