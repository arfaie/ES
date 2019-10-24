using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using E_School.Models.Repositories;
using E_School.Helpers.Utitlies;
using E_School.Models.DomainModels;
using MvcInternetShop.Models.Enums;
using System.Web.Script.Serialization;
using E_School.Models.ViewModel;

namespace E_School.Controllers.api.Management
{
    public class StudDataController : ApiController
    {
        StudDataRepository bl = new StudDataRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri] int idStud)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from Md in db.tbl_studentsData
                          where Md.idStudData > 0

                          join y in db.tbl_years
                          on Md.idYear equals y.idYear
                          where y.idYear > 0

                          join s in db.tbl_students
                          on Md.idStudent equals s.idStudent
                          where s.idStudent > 0 && s.idStudent==idStud

                          join t in db.tbl_Types
                          on Md.idType equals t.idType

                          select new
                          {
                              Md.dataDate,
                              Md.dataFile,
                              Md.dataTitle,
                              Md.des,
                              Md.idStudData,
                              Md.idStudent,
                              Md.idYear,
                              s.FName,
                              t.Type,
                              t.idType,
                              s.LName,
                              s.fatherName,
                              y.yearName

                          }).ToList().Select(Md => new
                          {
                              dataDate=Md.dataDate.Value.ToSlashDate(),
                              Md.dataFile,
                              Md.dataTitle,
                              Md.des,
                              Md.idStudData,
                              Md.idStudent,
                              Md.idYear,
                              Md.FName ,
                              Md.LName,
                              Md.fatherName,
                              Md.yearName,
                              Md.Type,
                              Md.idType
                          }).ToList();

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
        public int Add([FromBody] tbl_studentsData entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idStudData = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idStudData.ToString());
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
        public bool Update([FromBody] tbl_studentsData entity)
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
