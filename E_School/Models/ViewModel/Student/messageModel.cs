using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class messageModel
    {
        public int idMsgRecord { get; set; }
        public int idMessage { get; set; }
        public int idMessageType { get; set; }
        public int idSender { get; set; }
        public int idReciever { get; set; }
        public Boolean isRead { get; set; }
        public String title { get; set; }
        public String body { get; set; }
        public String senderName { get; set; }
        public String recieverName { get; set; }
        public int date { get; set; }
    }
}