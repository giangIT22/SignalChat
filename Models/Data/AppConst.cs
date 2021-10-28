using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SignalRChat.Models.Data
{
    public class AppConst
    {
        public static string SessionCurrentUserKey = "AshChat.Models.Data.AppState.CurrentUser";
        public User CurrentUser = new User();
        
    }
}