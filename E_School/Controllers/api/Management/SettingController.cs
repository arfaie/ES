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
    public class SettingController : ApiController
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
        public bool Add([FromBody]tbl_Setting entity)
        {
            try
            {
                if (bl.GetLastIdentity() > 0)
                {
                    Update(entity);
                    return true;
                }
                else
                {
                    if (entity == null)
                    {
                        return false;
                    }
                    else
                    {
                        entity.idSetting = bl.GetLastIdentity() + 1;
                        if (bl.Add(entity) == false)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                    }
                }
            }
            catch (Exception EX)
            {
                return false;
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