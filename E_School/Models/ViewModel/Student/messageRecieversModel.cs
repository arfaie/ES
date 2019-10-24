using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class messageRecieversModel
    {
        public int idReciever { get; set; }
        public int[] idRecievers { get; set; } 
        public int idMessageType { get; set; }
        public int idMessage { get; set; }
        public int idSender { get; set; }
        public int idSenderType { get; set; }
        public int idRecieverType { get; set; }
        public String title { get; set; }
        public String body { get; set; }
        public int date { get; set; }
    }
}