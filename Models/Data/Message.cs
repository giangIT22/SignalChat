using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public int GroupId { get; set; }
        public int ContentType { get; set; }

        public string Content { get; set; }
        public DateTime LastEditTime { get; set; }
        public DateTime CreationTime { get; set; }
    }
    public class CONTENT_TYPE
    {
        public static int Nu = 0;
        public static int Nam = 1;
    }

    public class MessageFunc
    {


    }

}