using E_School.Models;
using E_School.Models.DomainModels;
using E_School.Models.Repositories.api;
using E_School.Models.ViewModel.api;
using E_School.Models.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace E_School.api.Controllers.api.Student
{
    public class ExamController : ApiController
    {
        ExamRepository bl = new ExamRepository();

        [ActionName("teacherExam")]
        [HttpGet]
        public List<View_Exam> teacherExam([FromUri] int idTeacher, [FromUri] int idTerm)
        {
            return bl.TeacherExam(idTeacher, idTerm);
        }



        [ActionName("studentExam")]
        [HttpGet]
        public List<View_Exam> studentExam([FromUri] int idTerm, [FromUri] int idClass)
        {
            return bl.StudentExam(idTerm , idClass);
        }





        [ActionName("studentClassExam")]
        [HttpGet]
        public List<View_Exam> studentClassExam([FromUri] int date, [FromUri] int idClass)
        {
            return bl.StudentClassExam(date, idClass);
        }




        [ActionName("teacherClassExam")]
        [HttpGet]
        public List<View_Exam> TeacherClassExam([FromUri] int idTeacher)
        {
            return bl.TeacherClassExam(idTeacher);
        }




        [ActionName("getExamTypes")]
        [HttpGet]
        public List<examModel> getExamTypes()
        {
            return bl.getExamTypes();
        }



        [ActionName("insertClassExam")]
        [HttpPost]
        public int insertClassExam([FromBody] tbl_exams entity)
        {
            return bl.insertExam(entity);
        }




        [ActionName("editClassExam")]
        [HttpPost]
        public bool editClassExam([FromBody] tbl_exams entity)
        {
            return bl.editExam(entity);
        }



        [ActionName("deleteClassExam")]
        [HttpGet]
        public int deleteClassExam([FromUri] int idExam)
        {
            return bl.deleteExam(idExam);
        }





        [ActionName("GetScheduleExams")]
        [HttpGet]
        public List<ScheduleExam> GetScheduleExams([FromUri] int idClass, [FromUri] int idLesson, [FromUri] int idLesson2, [FromUri] int date)
        {
            return bl.GetScheduleExams(idClass, idLesson, idLesson2, date);
        }



        [ActionName("GetExamMaxScore")]
        [HttpGet]
        public int GetExamMaxScore([FromUri] int idExam)
        {
            return bl.GetExamMaxScore(idExam);
        }
    }
}
