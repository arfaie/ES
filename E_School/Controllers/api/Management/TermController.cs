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
    public class TermController : ApiController
    {
        TermRepository bl = new TermRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            schoolEntities db = new schoolEntities();

            var Result = (from t in db.tbl_terms

                          join y in db.tbl_years
                          on t.idYear equals y.idYear

                          select new
                          {
                              t.des,
                              t.idTerm,
                              t.idYear,
                              t.termEnd,
                              t.termName,
                              t.termStart,
                              y.yearName

                          }).OrderByDescending(x => x.termEnd).AsEnumerable().Select(t => new
                          {
                              t.des,
                              t.idTerm,
                              t.idYear,
                              termEnd = t.termEnd.Value.ToSlashDate(),
                              t.termName,
                              termStart = t.termStart.Value.ToSlashDate(),
                              t.yearName
                          });
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
        public int AddTerm([FromBody]tbl_terms entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idTerm = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idTerm.ToString());
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
        public bool Update([FromBody]tbl_terms entity)
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
                    int idTerm = bl.Where(x => x.idTerm == id).Single().idYear;
                    Methods ob = new Methods();
                    if (ob.isEditable(idTerm))
                    {
                        if (bl.Delete(id))
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
    }
}
