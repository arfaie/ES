using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class homeworkModel
    {
        public int idHomeWork { get; set; }
        public int exDateTime { get; set; }
        public int UploadDate { get; set; }
        public int idClass { get; set; }
        public int idTeacher { get; set; }
        public int idLesson { get; set; }
        public int score { get; set; }
        public String title { get; set; }
        public String body { get; set; }
        public String HomeWorkFile { get; set; }
        public String answerBody { get; set; }
        public String answerFile { get; set; }
        public String lessonName { get; set; }
        public String className { get; set; }
    }
}