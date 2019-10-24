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
    public class AbsController : ApiController
    {
        AbsRepository bl = new AbsRepository();

        [ActionName("select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri] int idYear, int idClass, int idLesson)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from a in db.tbl_absentation

                          join s in db.tbl_students
                          on a.idStudent equals s.idStudent
                          //where a.status == false

                          join dt in db.tbl_dataTable
                          on a.idDataTable equals dt.idDataTable
                          where dt.idClass == idClass & dt.idLesson == idLesson & dt.idYear == idYear

                          join t in db.tbl_teachers
                          on dt.idTeacher equals t.idTeacher

                          join l in db.tbl_lessons
                          on dt.idLesson equals l.idLesson

                          join d in db.tbl_days
                          on dt.idDay equals d.idDay

                          join b in db.tbl_bells
                          on dt.idBell equals b.idBells

                          select new
                          {
                              a.idAbsentation,
                              a.date,
                              a.status,
                              a.idStudent,
                              a.idDataTable,
                              s.FName,
                              s.LName,
                              dt.bellStart,
                              dt.bellEnd,
                              dt.idClass,
                              dt.idLesson,
                              dt.idYear,
                              dt.isTak,
                              TeacherFName = t.FName,
                              TeacherLName = t.LName,
                              l.lessonName,
                              d.dayName,
                              b.BellName
                          }).ToList().Select(a => new
                          {
                              a.idAbsentation,
                              a.idDataTable,
                              date = a.date.ToSlashDate(),
                              a.status,
                              a.idStudent,
                              a.FName,
                              a.LName,
                              End = a.bellEnd.Hours + ":" + a.bellEnd.Minutes,
                              Start = a.bellStart.Hours + ":" + a.bellStart.Minutes,
                              a.idClass,
                              a.idLesson,
                              a.idYear,
                              a.isTak,
                              a.TeacherFName,
                              a.TeacherLName,
                              a.lessonName,
                              a.dayName,
                              a.BellName
                          })
                                 .ToList();

            int k = 0;
            LsAbs model;
            List<LsAbs> listAbs = new List<LsAbs>();
            foreach (var c in Result)
            {
                var a = listAbs.Where(x => x.Date == c.date).FirstOrDefault();
                var b = listAbs.Where(x => x.idDt == c.idDataTable).FirstOrDefault();

                if (a == null || b == null)
                {
                    k++;
                    model = new LsAbs();
                    model.Date = c.date;
                    model.idDt = c.idDataTable;
                    model.Session = k;
                    listAbs.Add(model);
                }
            }
            string Sessions = "";
            string Dates = "";
            string myJson = "";
            int idStud=0;//برای مواقعی است که دانش آموز اصلا غیبتی ندارد  و در لیست ادد نمیکند و باغث تکرار نامش میشود
            int flag = 0;
            List<int> LsID = new List<int>();
            List<string> LsDate = new List<string>();
            List<string> LsSessionDate = new List<string>();
            List<int> LsIdDT = new List<int>();
            foreach (var b in Result)
            {
                Sessions = "";
                if (!LsID.Contains(b.idStudent))
                {
                    LsID.Add(b.idStudent);
                    LsDate.Clear();
                    //var s = new
                    //{
                    //    idStud = b.idStudent,
                    //    Name = b.FName + " " + b.LName,
                    //    Sessions = (from lA in listAbs
                    //                from res in Result
                    //                where res.idStudent == b.idStudent && !res.status
                    //                where lA.Date == res.date && res.idDataTable == lA.idDt
                    //                select lA
                    //                )
                    //};
                    myJson += "{\"idStud\":";
                    myJson += b.idStudent + ",";
                    myJson += "\"Name\":";
                    myJson += "\"" + b.FName + " " + b.LName + "\",";
                    myJson += "\"Sessions\":[";
                    foreach (var a in Result.Where(x => x.idStudent == b.idStudent && x.status==false))
                    {
                        if (!LsDate.Contains(a.date) || a.idDataTable != b.idDataTable)
                        {
                            var select = listAbs.Where(x => x.Date == a.date && x.idDt == a.idDataTable).Single();
                            Sessions += select.Session.ToString();
                            LsDate.Add(a.date);
                            Sessions += ",";
                            idStud = b.idStudent;
                        }
                        //LsID.Add(a.idStudent);
                    }
                    
                    myJson += Sessions + "],";
                    myJson += "\"Dates\":[";
                    if (flag == 0)//Dates array fill once and others is like first
                    {
                        foreach (var a in Result)
                        {
                            flag = 1;
                            string Mydate = "\"" + a.date.Substring(5, 5).ToString() + "\"";
                            //if (!Dates.Contains(Mydate))
                            if (!LsSessionDate.Contains(a.date) && !LsIdDT.Contains(a.idDataTable))
                            {
                                LsSessionDate.Add(a.date);
                                LsIdDT.Add(a.idDataTable);
                                Dates += Mydate;
                                if (!object.ReferenceEquals(a, Result.Last()))
                                {
                                    Dates += ",";
                                }
                            }


                        }
                    }
                    myJson += Dates + "]}";
                    if (!object.ReferenceEquals(b, Result.Last()))
                    {
                        myJson += ",";
                    }
                }
            }

            myJson = "[" + myJson + "]";
            myJson = myJson.Replace(",]", "]");
            myJson = myJson.Replace("\"Sessions\":[]", "\"Sessions\":[-2]");
           // var aa =

           //new JProperty("", from r in Result//all
           //                                  //group r by r3.idRegcourse into r3
           //                  select new JObject(
           //       new JProperty("Date", r.date),
           //       new JProperty("idStudent",
           //           new JArray(from r2 in Result//student
           //                                       //where r2.idDataTable == 1
           //                      select (r2.idStudent))))).ToString();
           // aa = aa.Substring(4);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result);
            return new HttpResponseMessage()
            {
                Content = new StringContent(myJson)
            };
        }


        [ActionName("TodayAbs")]
        [HttpGet]
        public HttpResponseMessage TodayAbs([FromUri] int Date)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from a in db.tbl_absentation

                          join s in db.tbl_students
                          on a.idStudent equals s.idStudent
                          where a.date==Date && a.status==false

                          join dt in db.tbl_dataTable
                          on a.idDataTable equals dt.idDataTable

                          join c in db.tbl_classes
                          on dt.idClass equals c.idClass

                          select new 
                          {
                              Name=s.FName+" "+ s.LName,
                              c.className
                          });
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result);
            return new HttpResponseMessage()
            {
                Content = new StringContent(json)
            };
        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]tbl_absentation entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idAbsentation = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idAbsentation.ToString());
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
        public bool Update([FromBody]tbl_absentation entity)
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
    }
}