using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories.api
{
    public class scheduleModel
    {
        public int idDataTable { set; get; }
        public int idTeacher { set; get; }
        public int idLesson { set; get; }
        public int idTeacher2 { set; get; }
        public int idLesson2 { set; get; }
        public int idDay { set; get; }
        public int idBell { set; get; }
        public int idClass { set; get; }
        public int idClass2 { set; get; }
        public Boolean isTak { set; get; }
        public String teacherName { set; get; }
        public String lessonName { set; get; }
        public String teacherName2 { set; get; }
        public String lessonName2 { set; get; }
        public String className { set; get; }
        public String className2 { set; get; }
    }
}