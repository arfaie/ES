using E_School.Helpers.Utitlies;
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
    public class ExamController : ApiController
    {
        ExamRepository bl = new ExamRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri]int idYear, [FromUri]int idClass)
        {
            schoolEntities db = new schoolEntities();
            var Result = (from e in db.tbl_exams

                          join c in db.tbl_classes
                          on e.idClass equals c.idClass
                          where e.idClass == idClass

                          join term in db.tbl_terms
                          on e.idTerm equals term.idTerm
                          where term.idYear == idYear

                          join eType in db.tbl_examTypes
                          on e.idExamType equals eType.idExamType

                          join l in db.tbl_lessons
                          on e.idLesson equals l.idLesson



                          select new
                          {
                              e.examDate,
                              e.examTitle,
                              e.idClass,
                              e.idExam,
                              e.idExamType,
                              e.idLesson,
                              e.idTerm,
                              e.maxScore,
                              eType.examType,
                              l.lessonName,
                              c.className,
                              term.termName,
                              e.des

                          }).OrderByDescending(x=>x.examDate).AsEnumerable().Where(x=>x.idExam>-1).Select(e => new
                          {
                              Date = e.examDate.ToSlashDate(),
                              e.des,
                              e.examTitle,
                              e.idClass,
                              e.idExam,
                              e.idExamType,
                              e.idLesson,
                              e.idTerm,
                              e.maxScore,
                              e.examType,
                              e.lessonName,
                              e.className,
                              e.termName
                          });

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
        public int AddYear([FromBody]tbl_exams entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idExam = bl.GetLastIdentity()+1;
                    if (entity.idExamType == 1)
                    {
                        entity.idTerm = 2;
                    }
                    else if(entity.idExamType == 2)
                    {
                        entity.idTerm = 1;
                    }
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idExam.ToString());
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
        public bool Update([FromBody]tbl_exams entity)
        {
            try
            {

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    Methods ob = new Methods();
                    if (ob.isEditable(entity.idTerm))
                    {
                        if (entity.idExamType == 1)
                        {
                            entity.idTerm = 2;
                        }
                        else if (entity.idExamType == 2)
                        {
                            entity.idTerm = 1;
                        }
                        if (bl.Update(entity))
                            return true;
                        else
                            return false;
                    }

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
                    int idTerm = bl.Where(x => x.idExam == id).Single().idTerm;
                     Methods ob = new Methods();
                     if (ob.isEditable(idTerm))
                     {
                         if (bl.Delete(id))
                             return true;
                         else
                             return false;
                     }
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
