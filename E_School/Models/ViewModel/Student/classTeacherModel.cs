using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class classTeacherModel
    {
        public int idTeacher { get; set; }
        public int idClass { get; set; }
        public String teacherName { get; set; }
        public String className { get; set; }
    }
}