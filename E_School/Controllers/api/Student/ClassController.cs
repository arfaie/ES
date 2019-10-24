using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using Newtonsoft.Json;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Globalization;

namespace E_School.api.Controllers.api.Student
{
    public class ClassController : ApiController
    {
        ClassRepository bl = new ClassRepository();

        [ActionName("Select")]
        [HttpGet]
        public List<view_Classes> select()
        {


            tbl_classes tbl = new tbl_classes();

            return bl.Select().ToList();

        }

        [ActionName("Add")]
        [HttpPost]
        public string AddYear([FromBody]tbl_classes entity)
        {
            try
            {


                if (entity == null)
                {
                    return "is null";
                }
                else
                {
                    entity.idClass = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return "false";
                    }
                    else
                    {
                        return "true";
                    }

                }

            }
            catch (Exception EX)
            {
                return "BadRequest";
            }
        }

        [ActionName("Update")]
        [HttpPost]
        public bool Update([FromBody]tbl_classes entity)
        {
            try
            {

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    if (bl.Update(entity))
                        return true;
                    else
                        return false;
                }

            }
            catch (Exception EX)
            {
                return false;
            }

        }

        [ActionName("Delete")]
        [HttpPost]
        public bool Delete([FromBody]int id)
        {
            try
            {


                if (id == null)
                {
                    return false;
                }
                else
                {
                    if (bl.Delete(id))
                        return true;
                    else
                        return false;
                }

            }
            catch (Exception EX)
            {
                return false;
            }

        }


        [ActionName("getCourseList")]
        [HttpGet]
        public HttpResponseMessage getCourseList()
        {
            schoolEntities db = new schoolEntities();

            int date = getTodayDate();
            int idYear = db.tbl_years.Where(x => x.yearStart <= date && x.yearEnd >= date).FirstOrDefault().idYear;

            var Result = (from rg in db.tbl_registrationCourses//join Years Name
                          join y in db.tbl_years

                          on rg.idYear equals idYear

                          select new

                          {
                              rg.des,
                              rg.endDateTime,
                              rg.idRegcourse,
                              rg.idYear,
                              rg.startDateTime,
                              rg.title,
                              rg.value,
                              y.yearName,

                          }).ToList().Select(rg => new
                          {
                              startDateTime = rg.startDateTime.ToSlashDate(),
                              endDateTime = rg.endDateTime.ToSlashDate(),
                              rg.des,
                              rg.idRegcourse,
                              rg.idYear,
                              rg.title,
                              rg.value,
                              rg.yearName,


                          })
                                 .ToList();

            var Result2 = (from list in db.tbl_regCourseLists//join Classes Name & regTitles 

                           join cl in db.tbl_classes
                           on list.idClass equals cl.idClass

                           join rg in db.tbl_registrationCourses
                           on list.idRegCourse equals rg.idRegcourse

                           select new
                           {
                               list.idClass,
                               list.idRegCourse,
                               list.idRegCourseList,
                               cl.className,
                               rg.title


                           }).ToList();

            var Result3 = (from r in Result
                               //group r by r.idRegcourse into r
                           join r2 in Result2
                           on r.idRegcourse equals r2.idRegCourse

                           select new
                           {
                               r.des,
                               r.endDateTime,
                               r.idRegcourse,
                               r.idYear,
                               r.startDateTime,
                               r.title,
                               r.value,
                               r.yearName,
                               r2.className,
                               r2.idClass

                           }).ToList();

            string aa =

            new JProperty("", from r3 in Result3
                              group r3 by r3.idRegcourse into r3
                              select new JObject(
                   new JProperty("des", r3.FirstOrDefault().des),
                   new JProperty("endDateTime", r3.FirstOrDefault().endDateTime),
                   new JProperty("idRegcourse", r3.FirstOrDefault().idRegcourse),
                   new JProperty("idYear", r3.FirstOrDefault().idYear),
                   new JProperty("startDateTime", r3.FirstOrDefault().startDateTime),
                   new JProperty("title", r3.FirstOrDefault().title),
                   new JProperty("value", r3.FirstOrDefault().value),
                   new JProperty("yearName", r3.FirstOrDefault().yearName),
                   new JProperty("idClass",
                       new JArray(
                            from r2 in Result2
                            where r3.FirstOrDefault().idRegcourse == r2.idRegCourse

                            select (r2.idClass))),
                           new JProperty("className",
                       new JArray(

                           from r2 in Result2
                           where r3.FirstOrDefault().idRegcourse == r2.idRegCourse

                           select (r2.className))))).ToString();

            aa = aa.Substring(4);

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result3);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    aa.ToString()

                )
            };
        }



        private int getTodayDate()
        {
            DateTime d = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string y = pc.GetYear(d).ToString();
            string m = pc.GetMonth(d).ToString();
            if (m.Count() == 1)
                m = "0" + m;
            string day = pc.GetDayOfMonth(d).ToString();
            if (day.Count() == 1)
                day = "0" + day;
            int date = int.Parse(y + m + day);

            return date;
        }
    }
}
