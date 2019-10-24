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
    public class DebtController : ApiController
    {
        DebtRepository bl = new DebtRepository();

        [ActionName("select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            schoolEntities db = new schoolEntities();

            var Result = (from d in db.tbl_debts

                          join y in db.tbl_years
                          on d.idYear equals y.idYear

                          join s in db.tbl_students
                          on d.idStud equals s.idStudent

                          join c in db.tbl_costs
                          on d.idCost equals c.idCost

                          join rg in db.tbl_registrationCourses
                          on d.idRegCourse equals rg.idRegcourse into ulist from u in ulist.DefaultIfEmpty() 

                          select new
                          {
                              d.Date,
                              
                              d.idCost,
                              d.idDebt,
                              d.idRegCourse,
                              d.idStud,
                              d.idYear,
                              y.yearName,
                              s.LName,
                              s.FName,
                              costTitle=c.Name,
                              costValue=c.Value,
                              regCoreTitle=u.title,
                              regCoreValue=u.value 

                          }).ToList().Select(d => new
                                 {
                                     Date = d.Date.ToSlashDate(),
                                     
                                     d.idCost,
                                     d.idDebt,
                                     d.idRegCourse,
                                     d.idStud,
                                     d.idYear,
                                     d.yearName,
                                     d.LName,
                                     d.FName,
                                     d.regCoreTitle,
                                     d.costTitle,
                                     d.costValue,
                                     d.regCoreValue
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
        public int Add(LsDebt entity)
        {
            int j = entity.idCost.Count();
            string val = "";
            for (int i = 0; i < j; i++)
            {
                tbl_debts tbl = new tbl_debts();
                tbl.Date = entity.Date;
                tbl.idDebt = entity.idDebt;
                tbl.idRegCourse = entity.idRegCourse;
                tbl.idStud = entity.idStud;
                tbl.idYear = entity.idYear;
                tbl.Date = entity.Date;
                tbl.idCost = entity.idCost[i];
                tbl.idDebt = bl.GetLastIdentity(true)+1;
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

        [ActionName("Update")]
        [HttpPost]
        public bool Update([FromBody]tbl_editTransections entity)//Update in this case is not avalible and we add a new record
        {
            try
            {

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    entity.idTrans = bl.GetLastIdentity(false) + 1;
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