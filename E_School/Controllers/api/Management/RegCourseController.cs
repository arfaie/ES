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
using E_School.Helpers.Utitlies;
using System.Web.Script.Serialization;
using E_School.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace E_School.Controllers.api.Management
{
    public class RegCourseController : ApiController
    {
        RegCourseRepository bl = new RegCourseRepository();

        [ActionName("select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri] int idYear)
        {
            schoolEntities db = new schoolEntities();

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

            new JProperty("", from r3 in Result3.OrderByDescending(x => x.idRegcourse)
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

        [ActionName("Add")]
        [HttpPost]
        public int Add(LsRegCourse entity)
        {
            int j = entity.idClass.Count();

            tbl_registrationCourses tbl = new tbl_registrationCourses();
            string val = "";

            tbl.des = entity.des;
            tbl.endDateTime = entity.endDateTime;
            tbl.idYear = entity.idYear;
            tbl.startDateTime = entity.startDateTime;
            tbl.title = entity.title;
            tbl.value = entity.value;
            int idLast = bl.GetLastIdentity() + 1;
            if (idLast == 0)
            {
                idLast = 1;
            }
            int idRegCourse = idLast;
            tbl.idRegcourse = idRegCourse;

            if (bl.Add(tbl) == false)
            {
                val += 0;
            }

            for (int i = 0; i < j; i++)
            {
                tbl_regCourseLists List = new tbl_regCourseLists();

                List.idClass = entity.idClass[i];
                List.idRegCourse = idRegCourse;
                List.idRegCourseList = bl.GetListLastIdentity() + 1;

                if (bl.AddList(List) == false)
                {
                    val += 0;
                }
            }
            if (val.Contains("0"))
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        [ActionName("Update")]
        [HttpPost]
        public int Update([FromBody]LsRegCourse entity)
        {
            try
            {

                if (entity == null)
                {
                    return -1;
                }
                else
                {
                    tbl_registrationCourses tbl = new tbl_registrationCourses();
                    tbl.des = entity.des;
                    tbl.endDateTime = entity.endDateTime;
                    tbl.idRegcourse = entity.idRegcourse;
                    tbl.idYear = entity.idYear;
                    tbl.startDateTime = entity.startDateTime;
                    tbl.title = entity.title;
                    tbl.value = entity.value;
                    if (bl.Update(tbl))
                        if (bl.DeleteList(entity.idRegcourse))
                            if (bl.AddLisit(entity.idRegcourse, entity.idClass))
                                return 1;
                            else
                                return 0;
                }



            }
            catch (Exception EX)
            {
                return -2;
            }

            return -3;
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
                    if (bl.DeleteList(id) && bl.DeleteStudentList(id) && bl.Delete(id))
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
        [ActionName("DeleteList")]
        [HttpPost]
        public bool DeleteList([FromBody]int id)
        {
            try
            {

                if (id == null)
                {
                    return false;
                }
                else
                {
                    if (bl.DeleteList(id))
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
    }
}