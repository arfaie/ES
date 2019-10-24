using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories
{
    public class ComponentRequest
    {
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string pagenumber { get; set; }
        public string pagesize { get; set; }
    }
}