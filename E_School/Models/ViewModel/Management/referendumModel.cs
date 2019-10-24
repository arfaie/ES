using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.Management
{
    public class referendumModel
    {
        public int id { get; set; }
        public int startDate { get; set; }
        public int endDate { get; set; }
        public string op1 { set; get; }
        public string op2 { set; get; }
        public string op3 { set; get; }
        public string op4 { set; get; }

        public string question { get; set; }
        public bool visibility { get; set; }
        public float res1 { get; set; }
        public float res2 { get; set; }
        public float res3 { get; set; }
        public float res4 { get; set; }
    }
}