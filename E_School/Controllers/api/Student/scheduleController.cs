using E_School.Models;
using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.Repositories.api;
using E_School.Models.ViewModel.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace E_School.api.Controllers.api.Student
{
    public class scheduleController : ApiController
    {
        ScheduleRepository bl = new ScheduleRepository();

        [ActionName("StudentSchedule")]
        [HttpPost]
        public List<scheduleModel> StudentSchedule([FromBody] int idClass)
        {
            return bl.StudentSchedule(idClass);
        }



        [ActionName("teacherSchedule")]
        [HttpGet]
        public List<scheduleModel> teacherSchedule([FromUri] int idTeacher)
        {
            return bl.teacherSchedule(idTeacher);
        }



        [ActionName("absentation")]
        [HttpPost]
        public Boolean absentation([FromBody] absentModel entity)
        {
            return bl.absentation(entity);
        }


        [ActionName("a")]
        [HttpGet]
        public string absentation()
        {
            string MessageBody = "گزارش غیبت" + Environment.NewLine + "محمد کرمی" + Environment.NewLine + "1396/08/10";
            return MessageBody;
        }



        [ActionName("absHistory")]
        [HttpGet]
        public HttpResponseMessage absHistory([FromUri] int idClass, [FromUri] int idLesson)
        {
            return bl.absHistory(idClass, idLesson);
        }





        [ActionName("editAbsentation")]
        [HttpPost]
        public Boolean editAbsentation([FromBody] absentModel entity)
        {
            return bl.editAbsentation(entity);
        }



        [ActionName("getDayAbsentation")]
        [HttpGet]
        public List<View_absentation> getDayAbsentation([FromUri] int date, [FromUri] int idClass, [FromUri] int idDataTable)
        {
            return bl.getDayAbsentation(date, idClass, idDataTable);
        }
    }
}
