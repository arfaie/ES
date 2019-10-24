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

namespace E_School.Controllers.api.Management
{
    public class ScoreController : ApiController
    {
        ScoreRepository bl = new ScoreRepository();

        [ActionName("select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri] int idYear,int idClass,int idLesson)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from s in db.tbl_scores

                          join e in db.tbl_exams
                          on s.idExam equals e.idExam

                          join ds in db.tbl_descriptiveScores
                          on s.idDescriptiveScore equals ds.idDescriptiveScore
                          //where s.idDescriptiveScore!=1

                          join et in db.tbl_examTypes
                          on e.idExamType equals et.idExamType

                          join c in db.tbl_classes
                          on e.idClass equals c.idClass
                          where e.idClass == idClass

                          join l in db.tbl_lessons
                          on e.idLesson equals l.idLesson
                          where e.idLesson == idLesson

                          join term in db.tbl_terms
                          on e.idTerm equals term.idTerm

                          join y in db.tbl_years
                          on term.idYear equals y.idYear
                          where term.idYear == idYear

                          join t in db.tbl_teachers
                          on e.idTeacher equals t.idTeacher

                          join st in db.tbl_students
                          on s.idStudent equals st.idStudent


                          select new
                          {
                              s.idScore,
                              e.idClass,
                              e.idLesson,
                              e.idTeacher,
                              e.idTerm,
                              s.date,
                              s.des,
                              s.score,
                              ds.desScore,
                              e.examDate,
                              e.examTitle,
                              e.maxScore,
                              et.examType,
                              c.className,
                              l.lessonName,
                              term.termName,
                              t.LName,
                              t.FName,
                              Family = st.LName,
                              Name = st.FName,



                          }).ToList().Select(s => new
                          {
                              date = s.date.ToSlashDate(),
                              s.des,
                              s.idScore,
                              s.score,
                              s.desScore,
                              examDate = s.examDate.ToSlashDate(),
                              s.examTitle,
                              s.idClass,
                              s.idLesson,
                              s.idTeacher,
                              s.idTerm,
                              s.maxScore,
                              s.examType,
                              s.className,
                              s.lessonName,
                              s.termName,
                              TecherName = s.FName,
                              TecherFamily = s.LName,
                              StudentName = s.Name,
                              StudentFamily = s.Family
                          })
                                .ToList();

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };
        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]tbl_scores entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idScore = bl.GetLastIdentity() + 1;
                    if (entity.des == null)
                    {
                        entity.des = "";
                    }
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idScore.ToString());
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
        public bool Update([FromBody]tbl_scores entity)
        {
            try
            {

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    if (entity.des == null)
                    {
                        entity.des = "";
                    }
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