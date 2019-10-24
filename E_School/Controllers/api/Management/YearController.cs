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
using System.IO;
using System.Web.Script.Serialization;

namespace E_School.Controllers.api.Management
{
    public class YearController : ApiController
    {
        YearRepository bl = new YearRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            schoolEntities db = new schoolEntities();
            var Result = (from y in db.tbl_years
                          where y.idYear > 0
                          select new
                          {
                              y.des,
                              y.idYear,
                              y.yearEnd,
                              y.yearName,
                              y.yearStart

                          }).AsEnumerable().Select(y => new
                          {
                              y.des,
                              y.idYear,
                              yearEnd = y.yearEnd.Value.ToSlashDate(),
                              y.yearName,
                              yearStart = y.yearStart.Value.ToSlashDate()
                          });

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result.OrderByDescending(x=>x.idYear));

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };
        }
        [ActionName("Add")]
        [HttpPost]
        public int AddYear([FromBody]tbl_years entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idYear = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/گزارشات/" + entity.yearName);
                        DirectoryInfo di = Directory.CreateDirectory(Path);
                        return entity.idYear;
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
        public bool Update([FromBody]tbl_years entity)
        {
            schoolEntities db = new schoolEntities();
            string oldName = db.tbl_years.Where(x => x.idYear == entity.idYear).FirstOrDefault().yearName;
            try
            {
                Methods ob = new Methods();
                if (ob.isEditable(entity.idYear))
                {
                    int id = entity.idYear;
                    if (entity == null)
                    {
                        return false;
                    }
                    else
                    {

                        if (bl.Update(entity))
                        {
                            string old = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/" + "/گزارشات/" + oldName);
                            string New = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/" + "/گزارشات/" + entity.yearName);
                            if (old != New)
                            {
                                Directory.Move(old, New);
                            }

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
                if (id == null)
                {
                    return false;
                }
                else
                {
                    if (bl.Delete(id))
                    {
                        string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/گزارشات/" + Name.yearName);
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
    }
}