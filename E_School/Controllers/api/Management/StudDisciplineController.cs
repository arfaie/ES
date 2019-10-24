using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace E_School.Controllers.api.Management
{
    public class StudDisciplineController : ApiController
    {
        StudDisciplineRepository bl = new StudDisciplineRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri] int Date,int idClass)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from sd in db.tbl_studentsDisciplines

                          join d in db.tbl_disciplineTypes
                          on sd.idDisType equals d.idDisType

                          join sr in db.tbl_studentRegister
                          on sd.idStudent equals sr.idStudent
                          where sr.idClass==idClass

                          join s in db.tbl_students
                          on sd.idStudent equals s.idStudent
                         where s.idStudent > 0 //&& s.idStudent == idStud

                          join t in db.tbl_terms
                          on sd.idTerm equals t.idTerm
                          where t.termStart<Date && t.termEnd>Date

                          select new
                          {
                              sd.actDate,
                              sd.des,
                              sd.idDisType,
                              sd.idStudent,
                              sd.idStudentDisciplines,
                              sd.idTerm,
                              sd.justified,
                              s.FName,
                              s.LName,
                              d.disName,
                              t.termName,
                              d.action,
                              d.value,

                          }).ToList().Select(sd => new
                          {
                              actDatesd=sd.actDate.ToSlashDate(),
                              sd.des,
                              sd.idDisType,
                              sd.idStudent,
                              sd.idStudentDisciplines,
                              sd.idTerm,
                              sd.FName,
                              sd.LName,
                              sd.disName,
                              sd.termName,
                              sd.action,
                              sd.value,
                              sd.justified

                          }).ToList().OrderByDescending(x=>x.idStudentDisciplines);

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
        public int Add(LsStudDisipline entity)
        {
            int j = entity.idStudent.Count();
            string val = "";
            TermRepository term = new TermRepository();
            var select=term.Select().Where(x=>x.termStart<entity.Date&&x.termEnd>entity.Date).Single();

            //var term = TermController
            for (int i = 0; i < j; i++)
            {
                tbl_studentsDisciplines tbl = new tbl_studentsDisciplines();
                tbl.actDate = entity.actDate;
                tbl.des = entity.des;
                tbl.idDisType = entity.idDisType;
                tbl.idTerm = select.idTerm;
                tbl.idStudent = entity.idStudent[i];
                tbl.justified = true;
                tbl.idStudentDisciplines = bl.GetLastIdentity() + 1;
                if (bl.Add(tbl) == false)
                {
                    val += 0;
                }
                else
                {
                    val += 1;
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

        [ActionName("Justify")]
        [HttpPost]
        public bool Justify([FromUri]int idStudentDisciplines, int Date)
        {
            try
            {
                var entity=bl.Where(x => x.idStudentDisciplines == idStudentDisciplines).FirstOrDefault();

                TermRepository term = new TermRepository();
                var select = term.Select().Where(x => x.termStart < Date && x.termEnd > Date).Single();
                entity.idTerm = select.idTerm;
                if (entity.justified == false)
                {
                    entity.justified = true;
                }
                else
                {
                    entity.justified = false;
                }
                    if (bl.Update(entity))
                        return true;
                    else
                        return false;
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
