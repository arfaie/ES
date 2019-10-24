using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Helpers.Utitlies
{
    public class LsDataTable
    {
        public int idDataTable { get; set; }
        public string dayName { get; set; }
        public string BellName { get; set; }
        public string className { get; set; }
        public string lessonName { get; set; }
        public string teacherName { get; set; }
        public string bellStart { get; set; }
        public string bellEnd { get; set; }
        public bool isTak { get; set; }
        public string LessonName2 { get; set; }
        public string teacherName2 { get; set; }
        public string takStart { get; set; }
        public string takEnd { get; set; }
        public string yearName { get; set; }
        public int idDay { get; set; }
        public int idBell { get; set; }
        public int idClass { get; set; }
        public int idTeacher { get; set; }
        public int idLesson { get; set; }
        public int idTeacher2 { get; set; }
        public int idLesson2 { get; set; }
        public int idYear { get; set; }
    }
}