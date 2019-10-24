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
    public class GroupRegController : ApiController
    {
        GroupRegRepository bl = new GroupRegRepository();

        [ActionName("select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri] int idRegCourse)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from rg in db.tbl_registrationCourses//join Years Name
                          join y in db.tbl_years

                          on rg.idYear equals y.idYear

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

                          }).ToList();

            var Result2 = (from list in db.tbl_registrationList

                           join s in db.tbl_students
                           on list.idStudent equals s.idStudent

                           join rg in db.tbl_registrationCourses
                           on list.idRegcourse equals rg.idRegcourse

                           select new

                           {
                               list.idPayed,
                               list.idRegcourse,
                               list.idRegList,
                               s.FName,
                               s.LName,
                               s.idStudent,

                               rg.title,

                           }).ToList();


            var Result3 = (from r in Result
                           join r2 in Result2
                           on r.idRegcourse equals r2.idRegcourse

                           select new
                           {
                               r2.FName,
                               r2.idPayed,
                               r2.idRegcourse,
                               r2.idRegList,
                               r2.idStudent,
                               r2.LName,
                               r2.title,

                           }).ToList();

            var aa =

           new JProperty("", from r3 in Result3//all
                             where r3.idRegcourse == idRegCourse
                             group r3 by r3.idRegcourse into r3
                             select new JObject(
                  new JProperty("idRegList", r3.FirstOrDefault().idRegList),
                  new JProperty("idPayed", r3.FirstOrDefault().idPayed),
                  new JProperty("idRegcourse", r3.FirstOrDefault().idRegcourse),
                  new JProperty("title", r3.FirstOrDefault().title),
                  new JProperty("idStudent",
                      new JArray(
                           from r2 in Result2//student
                           where r3.FirstOrDefault().idRegcourse == r2.idRegcourse
                           select (r2.idStudent))),
                           new JProperty("Name",
                       new JArray(

                           from r2 in Result2//studente
                           where r3.FirstOrDefault().idRegcourse == r2.idRegcourse
                           select (r2.FName + " " + r2.LName))))).ToString();

            aa = aa.Substring(4);

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(aa);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    aa.ToString()

                )
            };
        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]LsRegCourseList entity)
        {
            try
            {

                string val = "";
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {

                    
                    int j = entity.idStudent.Count();
                    for (int i = 0; i < j; i++)
                    {
                        if (bl.Exist(entity.idStudent[i], entity.idRegCourse) != -3)
                        {
                            tbl_registrationList tbl = new tbl_registrationList();
                            tbl.idPayed = entity.idPayed;
                            tbl.idRegList = bl.GetLastIdentity()+1;
                            tbl.idRegcourse = entity.idRegCourse;
                            tbl.idStudent = entity.idStudent[i];
                            if (bl.Add(tbl) == false)
                            {
                                val += "0";
                            }
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

            }
            catch (Exception EX)
            {
                return int.Parse("-2");
            }
        }

        [ActionName("Update")]
        [HttpPost]
        public bool Update([FromBody]tbl_registrationList entity)
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
        public bool Delete([FromBody] tbl_registrationList entity)
        {
            try
            {
                var ob = bl.Where(x => x.idRegcourse == entity.idRegcourse && x.idStudent == entity.idStudent).Single();
                if (bl.Delete(ob))
                    return true;
                else
                    return false;

            }
            catch (Exception EX)
            {
                return false;
            }

        }
    }
}