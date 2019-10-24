using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel
{
    public class LsStudDisipline
    {
        public int idStudentDisciplines { get; set; }
        public int idDisType { get; set; }
        public int[] idStudent { get; set; }
        public int actDate { get; set; }
        public int Date { get; set; }
        public string des { get; set; }
        public bool justify { get; set; }


    }
}