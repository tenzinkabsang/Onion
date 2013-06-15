using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.Web.UI.Models
{
    public class Message
    {
        public MessageType MessageType { get; set; }
        public string Value { get; set; }
    }
}