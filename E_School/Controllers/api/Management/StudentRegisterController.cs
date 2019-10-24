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
using Newtonsoft.Json.Linq;
using E_School.Models.ViewModel;
using System.IO;
using System.Data.Entity.Validation;
using static E_School.Helpers.Utitlies.Custom_Exception;

namespace E_School.Controllers.api.Management
{
    public class StudentRegisterController : ApiController
    {
        StudentRegisterRepository bl = new StudentRegisterRepository();
        YearRepository blYear = new YearRepository();

        [ActionName("select")]
        [HttpGet]
        public List<view_studentRegister> select([FromUri] int idYear, [FromUri]int idLevel, [FromUri] int idClass)
        {
            int Today = 0.GetPersianDate() ;
            idYear = blYear.Where(x => x.yearStart <= Today && x.yearEnd >= Today).FirstOrDefault().idYear;
            var list = bl.Where(x => x.idClass == idClass & x.idYear == idYear/* & x.idLevel == idLevel*/).ToList();
            return list;

        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]tbl_studentRegister entity)
        {
            try
            {
                int idStud = entity.idStudent;
                int idYear = entity.idYear;
                int idLevel = entity.idLevel;

                var Exist = bl.Exist(idStud, idYear, idLevel);

                if (Exist == -3)
                {
                    return int.Parse("-3");
                }

                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idStudReg = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        StudentRepository studBL = new StudentRepository();
                        tbl_students tbl = new tbl_students();
                        tbl.idStudent = entity.idStudent;
                        studBL.Update(tbl, true, true);//UpdateStatus
                        Methods Methods = new Methods();
                        //Create Folder
                        string Path = Methods.DiPath(entity.idClass, entity.idStudent);
                        DirectoryInfo di = Directory.CreateDirectory(Path);
                        ///////Send SMS//////
                        SendSMS sms = new SendSMS();



                        StudentRepository stud = new StudentRepository();
                        var select = stud.Where(x => x.idStudent == entity.idStudent).Single();
                        SettingRepository settingBL = new SettingRepository();
                        var setting = settingBL.Select().Single();
                        string schoolName = setting.schoolName.ToString().Replace("مدرسه", "");
                        string studName = select.FName + " " + select.LName;
                        string text = schoolName + ";" + studName + ";" + select.pUser + ";" + select.pPass + ";" + select.studUser + ";" + select.pass + ";" + setting.Website;

                        sms.send(select.fPhone, text, 1, true);

                        ///////Create PDF/////
                        ReportController report = new ReportController();
                        try
                        {
                            report.RegFinancial(idStud);
                            return 1;
                        }
                        catch (DbEntityValidationException e)
                        {
                            var newException = new FormattedDbEntityValidationException(e);
                            StreamWriter sw = new StreamWriter("~/Content/Reports/txt3.log", false);
                            sw.WriteLine(newException.Message);
                            sw.WriteLine(newException.InnerException);
                            sw.WriteLine(newException.Source);
                            return 0;
                        }
                    }

                }

            }
            catch (Exception EX)
            {
                return -2;
            }
        }
        //public int TransferStudent([FromBody]LsStudentTransfer entity)
        //{
        //    try
        //    {
        //        StudentRegisterRepository bl = new StudentRegisterRepository();
        //        tbl_studentRegister ls = new tbl_studentRegister();
        //        int i = 0;
        //        foreach (var a in entity.idStudent)
        //        {
        //            int id = bl.GetLastIdentity() + 1;
        //            ls = new tbl_studentRegister();
        //            ls.idStudReg = id + i;
        //            ls.idClass = entity.idClass;
        //            ls.idLevel = entity.idLevel;
        //            ls.idMajor = entity.idMajor;
        //            ls.idYear = entity.idYear;
        //            int idStud = entity.idStudent[i];
        //            ls.idStudent = entity.idStudent[i];
        //            ls.registerStatus = true;
        //            ls.des = null;
        //            i++;
        //            ////if (entity.idStudent.Count() != i)
        //            ////{
        //            ////    bl.Add(ls, false);
        //            ////}
        //            ////else
        //            ////{
        //            ////    bl.Add(ls, true);
        //            ////}
        //        }
        //        bl.Add(ls, true);


        //        return 1;
        //    }
        //    catch
        //    {
        //        return -1;
        //    }
        //}
        [ActionName("Update")]
        [HttpPost]
        public bool Update([FromBody]tbl_studentRegister entity)
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
                    {
                        return true;
                    }
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
            return false;//برای اینکه از لیست ثبت نام شده ها نشه حذف کرد
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