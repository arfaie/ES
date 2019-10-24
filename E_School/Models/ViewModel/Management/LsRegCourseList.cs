using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel
{
    public class LsRegCourseList
    {
        public int idRegList { get; set; }
        public int idRegCourse { get; set; }
        public int idPayed { get; set; }
        public int[] idStudent { get; set; }
    }
}