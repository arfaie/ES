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
    public class SettingsControlle : ApiController
    {
        SettingRepository bl = new SettingRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            var Result = bl.Select();

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result);

            return new HttpResponseMessage()
            {
                Content = new StringContent(json)
            };
        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]tbl_Setting entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idSetting = bl.GetLastIdentity()+1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idSetting.ToString());
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
        public bool Update([FromBody]tbl_Setting entity)
        {
            try
            {

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    if (bl.GetLastIdentity() == 0)
                    {
                        Add(entity);
                        return true;
                    }
                    else
                    {
                        entity.idSetting = 1;
                        if (bl.Update(entity))
                            return true;
                        else
                            return false;
                    }
                        
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
