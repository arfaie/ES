using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.Student
{
    public class getDiscipModel
    {
        public int idStudentDisciplines { get; set; }
        public int idDisType { get; set; }
        public int idStudent { get; set; }
        public int actDate { get; set; }
        public int idTerm { get; set; }
        public string des { get; set; }
        public bool justified { get; set; }
        public int idTeacher { get; set; }
        public int idDataTable { get; set; }
    }
}