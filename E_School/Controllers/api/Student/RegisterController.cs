using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.Repositories.api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_School.api.Controllers.api.Student
{
    public class RegisterController : ApiController
    {
        RegisterRepository bl = new RegisterRepository();



        [ActionName("studentRegister")]
        [HttpPost]
        public String studentRegister([FromBody] tbl_students entity)
        {
            return bl.studentRegister(entity);
        }



        [HttpPost]
        [ActionName("getManifestFile")] // get manifest image from student
        public string getManifestFile()
        {
            int iUploadedCnt = 0;
            System.Web.HttpPostedFile hpf = null;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "", imgName;
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploaded/Students/imgManifest/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
            }

               return "Files Uploaded Successfully";

      
        }



        [HttpPost]
        [ActionName("getPersonalFile")] // get personal image from student
        public string getPersonalFile()
        {
            int iUploadedCnt = 0;
            System.Web.HttpPostedFile hpf = null;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "", id;
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploaded/Students/imgPersonal/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
            }

                return "Files Uploaded Successfully";
        } 
    }
}
