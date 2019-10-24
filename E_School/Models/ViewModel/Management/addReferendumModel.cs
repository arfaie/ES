using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.Management
{
    public class addReferendumModel
    {
        public int id { set; get; }
        public string Q { set; get; }
        public int startDate { set; get; }
        public int endDate { set; get; }
        public string op1 { get; set; }
        public string op2 { get; set; }
        public string op3 { get; set; }
        public string op4 { get; set; }
    }
}