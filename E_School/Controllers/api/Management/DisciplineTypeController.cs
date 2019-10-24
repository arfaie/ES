using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace E_School.Controllers.api.Management
{
    public class DisciplineTypeController : ApiController
    {
        DisciplineTypeRepository bl = new DisciplineTypeRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            var select = bl.Select();
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(select);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };

        }

        [ActionName("Add")]
        [HttpPost]
        public int AddYear([FromBody]tbl_disciplineTypes entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idDisType = bl.GetLastIdentity()+1;
                    if (entity.idDisType == 0)
                    {
                        entity.idDisType = 1;
                    }
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idDisType.ToString());
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
        public bool Update([FromBody]tbl_disciplineTypes entity)
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
                if (id <0)
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
