using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using E_School.Models.DomainModels;
using System.IO;
using E_School.Models.Repositories;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;

using System.Web;

namespace E_School.Controllers.api.Management
{
    public class UploadController : ApiController
    {

        [HttpPost]
        [ActionName("UploadFiles")]
        public string UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";


            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;



            // CHECK THE FILE COUNT.

            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];  //if File count is more than one we should use this> System.Web.HttpPostedFile hpf =hfc[iCnt];

                if (hpf.FileName.Contains("_sp"))
                {
                    sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploaded/Students/imgPersonal/");
                }

                if (hpf.FileName.Contains("_op"))
                {
                    sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploaded/Offices/imgPersonal/");
                }

                if (hpf.FileName.Contains("_sm"))
                {
                    sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploaded/Students/imgManifest/");
                }

                if (hpf.FileName.Contains("_on"))
                {
                    sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploaded/Offices/NatCard/");
                }

                if (hpf.FileName.Contains("_r"))
                {
                    sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploaded/Students/med_eduData/");
                }

                if (hpf.ContentLength > 0)
                {
                    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    //if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    //{
                        
                    //    // SAVE THE FILES IN THE FOLDER.
                    //    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    //    iUploadedCnt = iUploadedCnt + 1;
                    //}
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return iUploadedCnt + " Files Uploaded Successfully";
            }
            else
            {
                return "Upload Failed";
            }
        }
    }
}
