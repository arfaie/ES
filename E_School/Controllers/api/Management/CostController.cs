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

namespace E_School.Controllers.api.Management
{
    public class CostController : ApiController
    {
        CostRepository bl = new CostRepository();

        [ActionName("select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            schoolEntities db = new schoolEntities();
            tbl_costs cost = new tbl_costs();
            tbl_years year = new tbl_years();

            var Result = (from c in db.tbl_costs
                                 join o in db.tbl_years
                                 on c.idYear equals o.idYear
                                 where c.idCost>0
                                 select new
                                 {
                                     c.Des,
                                     c.idCost,
                                     c.idYear,
                                     c.Name,
                                     c.Value,
                                     o.yearName
                                 }).ToList();//.OrderBy(m => m.order_date.Month).ThenBy(y => y.order_date.Year);


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
        public int Add([FromBody]tbl_costs entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idCost= bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idYear.ToString());
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
        public bool Update([FromBody]tbl_costs entity)
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