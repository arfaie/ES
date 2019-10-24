using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class financialModel
    {
        public int Date { set; get; }
        public String Name { set; get; }
        public int debtAmount { set; get; }
        public Decimal debtSum { set; get; }
        public int payAmount { set; get; }
        public Decimal paySum { set; get; }
        public String payType { set; get; }
        public String des { set; get; }
    }
}