using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.Management
{
    public class newsModel
    {
        public List<string> images = new List<string>();
        public int id { get; set; }
        public int date { get; set; }
        public int receiverType { get; set; }
        public int visitCount { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}