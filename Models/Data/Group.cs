using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class Group
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }

        public DateTime CreationTime { get; set; }

        public List<User> Members { get; set; }
    }
    public class ROLE
    {
        public static int Member = 0;
        public static int Admin = 1;
    }

    public class GroupFunc
    {
    }

}