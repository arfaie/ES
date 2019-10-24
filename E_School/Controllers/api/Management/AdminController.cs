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

namespace E_School.Controllers.api.Management
{
    public class AdminController : ApiController
    {
        AdminRepository bl = new AdminRepository();

        [ActionName("Login")]
        [HttpGet]
        public int Login([FromUri] string User, string Pass)
        {
            int Login = (bl.Where(x => x.userName.Equals(User) && x.Password.Equals(Pass)).Count());
            //if (Login == 1)
            //{
            //    return true;
            //}
            //else
            //    return false;
            return Login;
        }

        [ActionName("Select")]
        [HttpGet]
        public List<tbl_Admin____> select()
        {


            tbl_Admin____ tbl = new tbl_Admin____();

            return bl.Select().ToList();

        }

        [ActionName("Add")]
        [HttpPost]
        public string AddYear([FromBody]tbl_Admin____ entity)
        {
            try
            {
                if (entity == null)
                {
                    return "is null";
                }
                else
                {
                    entity.idAdmin = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return "false";
                    }
                    else
                    {
                        return "true";
                    }

                }

            }
            catch (Exception EX)
            {
                return "BadRequest";
            }
        }

        [ActionName("Update")]
        [HttpPost]
        public bool Update([FromBody]tbl_Admin____ entity)
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

        [ActionName("Test")]
        [HttpPost]
        public string Test([FromBody]string Test)
        {
            try
            {
                if (Test == null)
                {
                    return "is null";
                }

                else
                {
                    return "true";
                }

            }
            catch (Exception EX)
            {
                return "BadRequest";
            }
        }

        [ActionName("SendToAllParent")]
        [HttpGet]
        public bool SendToAllParent()
        {
            SendSMS sms = new SendSMS();
            bool check=sms.SendToAllParent();
            return check;
        }
    }
}

