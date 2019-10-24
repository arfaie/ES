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
    public class StudentController : ApiController
    {
        StudentRepository bl = new StudentRepository();


        [ActionName("PrimaryRegister")]
        [HttpGet]
        public HttpResponseMessage PrimaryRegister()
        {
            //var s = bl.Where(x => x.idStatus == 6).OrderByDescending(x => x.idStudent).ToList();
            var select = bl.Where(x => x.idStatus == 6&&x.isActive==true).OrderByDescending(x => x.idStudent).ToList();
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(select);
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };
        }
        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri]string natCode)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from s in db.tbl_students

                          join e in db.tbl_educations
                          on s.idFatherEdu equals e.idEdu
                          where s.natCode == natCode

                          join ed in db.tbl_educations
                          on s.idMotherEdu equals ed.idEdu

                          join m in db.tbl_marriges
                          on s.idMarrige equals m.idMarrige

                          select new
                          {
                              s.birthDate,
                              s.fatherName,

                              s.FName,

                              s.idStudent,

                              s.imgPersonal,

                              s.LName,

                              s.natCode,

                              s.studentCode,

                          }).ToList().Select(s => new
                          {
                              birthDate = s.birthDate.ToLongSlashDate(),
                              s.fatherName,

                              s.FName,
                              s.LName,
                              s.idStudent,

                              s.imgPersonal,

                              s.natCode,

                              s.studentCode,

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
        [ActionName("StudentBank")]
        [HttpGet]
        public HttpResponseMessage StudentBank([FromUri]bool Status)
        {
            var select = bl.Where(x => x.isActive == Status);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(select);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };
        }

        [ActionName("Details")]
        [HttpGet]
        public HttpResponseMessage Details([FromUri]int idStud)
        {
            schoolEntities db = new schoolEntities();

            var Result = (from s in db.tbl_students

                          join e in db.tbl_educations
                          on s.idFatherEdu equals e.idEdu
                          where s.idStudent == idStud
                          join ed in db.tbl_educations
                          on s.idMotherEdu equals ed.idEdu

                          join m in db.tbl_marriges
                          on s.idMarrige equals m.idMarrige

                          select new
                          {
                              s.birthDate,
                              s.fatherName,
                              s.fJob,
                              s.fJobAddress,
                              s.fJobPhone,
                              s.FName,
                              s.fPhone,
                              s.gender,
                              s.homeAddress,
                              s.homePhone,
                              FatherEdu = e.eduName,
                              MotherEdu = ed.eduName,
                              s.idMotherEdu,
                              s.idFatherEdu,
                              m.marrigeName,
                              s.idStatus,
                              s.idStudent,
                              s.imgManifest,
                              s.imgPersonal,
                              s.isActive,
                              s.leftHand,
                              s.LName,
                              s.mJob,
                              s.mJobAddress,
                              s.mJobPhone,
                              s.motherName,
                              s.mPhone,
                              s.natCode,
                              s.pass,
                              s.pPass,
                              s.pUser,
                              s.religion,
                              s.studentCode,
                              s.studentMobile,
                              s.studUser
                          }).ToList().Select(s => new
                          {
                              birthDate = s.birthDate.ToLongSlashDate(),
                              s.fatherName,
                              s.fJob,
                              s.fJobAddress,
                              s.fJobPhone,
                              s.FName,
                              s.fPhone,
                              s.gender,
                              s.homeAddress,
                              s.homePhone,
                              FatherEdu = s.FatherEdu,
                              MotherEdu = s.MotherEdu,
                              s.idMotherEdu,
                              s.idFatherEdu,
                              s.marrigeName,
                              s.idStatus,
                              s.idStudent,
                              s.imgManifest,
                              s.imgPersonal,
                              s.isActive,
                              s.leftHand,
                              s.LName,
                              s.mJob,
                              s.mJobAddress,
                              s.mJobPhone,
                              s.motherName,
                              s.mPhone,
                              s.natCode,
                              s.pass,
                              s.pPass,
                              s.pUser,
                              s.religion,
                              s.studentCode,
                              s.studentMobile,
                              s.studUser
                          })
                                            .ToList();
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result.OrderByDescending(x => x.idStudent));

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };
        }

        [ActionName("ChangeStatus")]
        [HttpPost]
        public int ChangeStatus([FromBody] LsStudentTransfer entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    int i = 0;
                    foreach (int idStud in entity.idStudent)
                    {
                        var select = bl.Find(idStud);
                        if (entity.Status == true)//تبدیل حالت غیرفعال به فعال
                        {
                            select.isActive = true;
                        }
                        else//تبدیل حالت فعال به غیرفعال
                        {
                            select.isActive = false;
                        }
                        i++;
                        if (entity.idStudent.Count() != i)
                        {
                            bl.Update(select, false);
                        }
                        else
                        {
                            bl.Update(select, true);
                        }
                    }
                    return 1;
                }

            }
            catch (Exception EX)
            {
                return int.Parse("-2");
            }
        }

        [ActionName("StudentTransfer")]
        [HttpPost]
        public int StudentTransfer([FromBody] List<int> entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    int i = 0;
                    foreach (int idStud in entity)
                    {
                        var select = bl.Find(idStud);
                        select.idStatus = 6;
                        i++;
                        if (entity.Count() != i)
                        {
                            bl.Update(select, false);
                        }
                        else
                        {
                            bl.Update(select, true);
                        }
                    }
                    return 1;
                }

            }
            catch (Exception EX)
            {
                return int.Parse("-2");
            }
        }


        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody] tbl_students entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idStudent = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idStudent.ToString());
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
        public bool Update([FromBody] tbl_students entity)
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
