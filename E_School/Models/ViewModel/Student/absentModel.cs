using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class absentModel
    {
        public int[] idAbsents { get; set; }
        public int[] idPresents { get; set; }
        public int idDataTable { get; set; }
        public int date { get; set; }
    }
}