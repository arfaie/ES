using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace E_School.Controllers.api.Management
{
    public class HomeWorkController : ApiController
    {
        HomeWorkRepository bl = new HomeWorkRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri] int idClass, int Date)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from v in db.View_homeWork

                          join c in db.tbl_classes
                          on v.idClass equals c.idClass
                          where v.idClass==idClass

                          join y in db.tbl_years
                          on c.idYear equals y.idYear

                          join l in db.tbl_lessons
                          on v.idLesson equals l.idLesson

                          select new
                          {
                              v.answerBody,
                              v.answerFile,
                              v.body,
                              v.className,
                              v.exDateTime,
                              v.FName,
                              v.LName,
                              v.title,
                              v.UploadDate,
                              v.idClass,
                              v.idHomeWork,
                              v.HomeWorkFile,
                              y.yearName,
                              y.idYear,
                              y.yearStart,
                              y.yearEnd,
                              l.lessonName
                              
                              

                          }).ToList().Where(y => y.yearStart < Date && y.yearEnd > Date).Select(v => new
                          {
                              v.lessonName,
                              v.HomeWorkFile,
                              v.answerBody,
                              v.answerFile,
                              v.body,
                              v.className,
                              v.idHomeWork,
                              exDateTime = v.exDateTime.ToSlashDate(),
                              v.FName,
                              v.LName,
                              v.idClass,
                              v.title,
                              UploadDate = v.UploadDate.ToSlashDate()
                          })
                                 .ToList().OrderByDescending(x=>x.idHomeWork);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result);
            return new HttpResponseMessage()
            {
                Content = new StringContent(json)
            };
        }

    }
}