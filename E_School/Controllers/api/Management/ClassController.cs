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
using System.IO;
using E_School.Helpers.Utitlies;
namespace E_School.Controllers.api.Management
{
    public class ClassController : ApiController
    {
        ClassRepository bl = new ClassRepository();

        [ActionName("Select")]
        [HttpGet]
        public List<view_Classes> select(int id) 
        {
            if (id == -1)//return all classe
            {
                return bl.Select().OrderByDescending(x => x.idClass).Where(x => x.idClass != 6).ToList();

            }
            else
            {
                //کلاس با آی دی -1 برای امتحان شفاهی
                return bl.Select().OrderByDescending(x => x.idClass).Where(x => x.idClass != 6 && x.idYear == id).ToList();
            }
            

        }

        [ActionName("Add")]
        [HttpPost]
        public int AddClass([FromBody]tbl_classes entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idClass = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        string yearName = YearName(entity.idYear);
                        string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/گزارشات/" + yearName + "/" + entity.className);
                        DirectoryInfo di = Directory.CreateDirectory(Path);
                        return int.Parse(entity.idClass.ToString());
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
        public bool Update([FromBody]tbl_classes entity)
        {
            try
            {
                Methods ob = new Methods();
                if (ob.isEditable(entity.idYear))
                {
                    schoolEntities db = new schoolEntities();
                    string oldName = db.tbl_classes.Where(x => x.idClass == entity.idClass).FirstOrDefault().className;
                    if (entity == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (bl.Update(entity))
                        {
                            string yearName = YearName(entity.idYear);
                            string old = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/" + "/گزارشات/" + yearName + "/" + oldName);
                            string New = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/" + "/گزارشات/" + yearName + "/" + entity.className);
                            Directory.Move(old, New);
                            return true;
                        }
                        else
                            return false;
                    }

                }
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

                var Name = bl.Where(x => x.idYear == id).FirstOrDefault();
                int idYear = bl.Where(x => x.idClass == id).Single().idYear;
                string className = bl.Where(x => x.idClass == id).Single().className;
                if (id == null)
                {
                    return false;
                }
                else
                {
                    if (bl.Delete(id))
                    {

                        string yearName = YearName(idYear);
                        string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/گزارشات/" + yearName + "/" + className);
                        Directory.Delete(Path, true);
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

        public string YearName(int id)
        {
            YearRepository blYear = new YearRepository();
            string yearName = blYear.Where(x => x.idYear == id).Single().yearName;
            return yearName;
        }
    }
}
