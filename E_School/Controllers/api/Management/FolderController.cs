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
using System.IO;


namespace E_School.Controllers.api.Management
{
    public class FolderController : ApiController
    {
        DataTableRepository bl = new DataTableRepository();
        schoolEntities db = new schoolEntities();
        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            string path = @"c:\گزارشات";

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    //return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                // Delete the directory.
                di.Delete();
                Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
            
            return new HttpResponseMessage()
            {
                Content = new StringContent("1")
            };

        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]tbl_dataTable entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idDataTable = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idDataTable.ToString());
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