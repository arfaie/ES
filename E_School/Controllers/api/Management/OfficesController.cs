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

namespace E_School.Controllers.api.Management
{
    public class OfficesController : ApiController
    {
        OfficesRepository bl = new OfficesRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            schoolEntities db = new schoolEntities();
            var Result = (from o in db.tbl_offices
                          where o.idOffices > 0
                          select new
                          {
                              o.FName,
                              o.idOffices,
                              o.imgPersonal,
                              o.LName,
                              o.personalCode,
                              o.natCode,
                              o.job

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

        [ActionName("Details")]
        [HttpGet]
        public HttpResponseMessage Details([FromUri] int id)
        {
            schoolEntities db = new schoolEntities();
            var Result = (from o in db.tbl_offices
                          where o.idOffices > 0&& o.idOffices==id
                          select new
                          {
                              o.address,
                              o.birthDate,
                              o.email,
                              o.fatherName,
                              o.FName,
                              o.gender,
                              o.idOffices,
                              o.imgManifest,
                              o.imgPersonal,
                              o.isActive,
                              o.LName,
                              o.mobile,
                              o.natCode,
                              o.personalCode,
                              o.phone,
                              o.status,
                              o.Pass,
                              o.userName,
                              o.job

                          }).ToList().Select(o => new
                          {
                              o.FName,
                              o.LName,
                              o.address,
                              birthDate = o.birthDate.ToSlashDate(),
                              o.email,
                              o.fatherName,
                              o.gender,
                              o.idOffices,
                              o.imgManifest,
                              o.imgPersonal,
                              o.isActive,
                              o.mobile,
                              o.natCode,
                              o.personalCode,
                              o.phone,
                              o.status,
                              o.Pass,
                              o.userName,
                              o.job
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
        public int Add([FromBody] tbl_offices entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idOffices = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idOffices.ToString());
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
        public bool Update([FromBody] tbl_offices entity)
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
