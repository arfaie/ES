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
    public class TeacherController : ApiController
    {
        TeacherRepository bl = new TeacherRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            schoolEntities db = new schoolEntities();
            var Result = (from t in db.tbl_teachers
                          where t.idTeacher>0
                          select new
                          {
                              t.FName,
                              t.idTeacher,
                              t.imgPersonal,
                              t.LName,
                              t.personalCode,
                              t.natCode,


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
            var Result = (from t in db.tbl_teachers
                          where t.idTeacher > 00 && t.idTeacher== id
                          select new
                          {
                              t.address,
                              t.birthDate,
                              t.email,
                              t.fatherName,
                              t.FName,
                              t.gender,
                              t.idTeacher,
                              t.imgManifest,
                              t.imgPersonal,
                              t.isActive,
                              t.LName,
                              t.mobile,
                              t.natCode,
                              t.personalCode,
                              t.phone,
                              t.status,
                              t.teacherPass,
                              t.teacherUser

                          }).ToList().Select(t => new
                          {
                              t.FName ,
                              t.LName,
                              t.address,
                              birthDate = t.birthDate.ToSlashDate(),
                              t.email,
                              t.fatherName,
                              t.gender,
                              t.idTeacher,
                              t.imgManifest,
                              t.imgPersonal,
                              t.isActive,
                              t.mobile,
                              t.natCode,
                              t.personalCode,
                              t.phone,
                              t.status,
                              t.teacherPass,
                              t.teacherUser
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
        public int Add([FromBody] tbl_teachers entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    var User = bl.Where(x => x.teacherUser == entity.teacherUser).FirstOrDefault();
                    if (User != null)
                    {
                        return -2;
                    }
                    else
                    {
                        entity.idTeacher = bl.GetLastIdentity() + 1;
                        entity.status = true;
                        entity.isActive = true;
                        if (entity.idTeacher == 0)
                        {
                            entity.idTeacher = 1;
                        }
                        if (bl.Add(entity) == false)
                        {
                            return int.Parse("0");
                        }
                        else
                        {
                            return int.Parse(entity.idTeacher.ToString());
                        }
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
        public bool Update([FromBody] tbl_teachers entity)
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
