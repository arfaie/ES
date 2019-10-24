using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel
{
    public class LsRegCourse
    {
        public int idRegcourse { get; set; }
        public string title { get; set; }
        public int idYear { get; set; }
        public int[] idClass { get; set; }
        public int value { get; set; }
        public int startDateTime { get; set; }
        public int endDateTime { get; set; }
        public string des { get; set; }
    }
}