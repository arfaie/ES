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
using System.Globalization;
using System.Web.Script.Serialization;
using E_School.Helpers.Utitlies;


namespace E_School.Controllers.api.Management
{
    public class DataTableController : ApiController
    {
        DataTableRepository bl = new DataTableRepository();
        schoolEntities db = new schoolEntities();
        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri]int idClass, int idYear)
        {

            var query = bl.Join(idClass, idYear);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(query);


            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };

        }

        [ActionName("Teacher")]
        [HttpGet]
        public HttpResponseMessage Teacher([FromUri]int Date, int idTeacher)
        {

            var query = bl.TecherDT(Date, idTeacher);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(query);


            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };

        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]tbl_dataTable entity)
        {
            try
            {
                int T2 = 0;
                entity.idDataTable = bl.GetLastIdentity() + 1;
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    var select = bl.Where(x => x.idDay == entity.idDay && x.idBell == entity.idBell && x.idYear == entity.idYear);
                    var selectT1 = select.Where(x => x.idTeacher == entity.idTeacher).FirstOrDefault();

                    if (entity.idTeacher2 != -1)
                    {
                        var selectT2 = select.Where(x => x.idTeacher2 == entity.idTeacher2).FirstOrDefault();
                        if (selectT2 != null)
                        {
                            T2 = 1;
                        }
                        
                    }
                    //var selectT2 = select.Where(x => x.idTeacher2 == entity.idTeacher2).Where(x => x.idTeacher2 != -1).FirstOrDefault();

                    if (selectT1 == null && T2 != 1)
                    {
                        ////if (select.idTeacher2 == -1)
                        ////{
                        ////    return -3;
                        ////}
                        ////if (select.idTeacher == entity.idTeacher || select.idTeacher2 == entity.idTeacher2)
                        ////{
                        ////    return -3;
                        ////}
                        ////else 
                        ////{
                        if (bl.Add(entity) == false)
                        {
                            return int.Parse("0");
                        }
                        else
                        {
                            return int.Parse(entity.idDataTable.ToString());
                        }
                        //}
                    }
                    else
                    {
                        return -3;
                        //if (bl.Add(entity) == false)
                        //{
                        //    return int.Parse("0");
                        //}
                        //else
                        //{
                        //    return int.Parse(entity.idDataTable.ToString());
                        //}

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
        public bool Update([FromBody]tbl_dataTable entity)
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
                    if (ob.isEditable(entity.idYear))
                    {
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