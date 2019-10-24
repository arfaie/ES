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

namespace E_School.Controllers.api.Management
{
    public class LevelController : ApiController
    {
        LevelRepository bl = new LevelRepository();

        [ActionName("Select")]
        [HttpGet]
        public List<tbl_levels> select()
        {
            var list = bl.Select().AsQueryable();
            list = list.Where(x => x.idLevel >0);
            return list.ToList();
        }

        [ActionName("Add")]
        [HttpPost]
        public int AddYear([FromBody]tbl_levels entity)
        {
            try
            {

                
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idLevel = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idLevel.ToString());
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
        public bool Update([FromBody]tbl_levels entity)
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