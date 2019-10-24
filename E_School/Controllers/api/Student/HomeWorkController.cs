using E_School.Models;
using E_School.Models.DomainModels;
using E_School.Models.Repositories.api;
using E_School.Models.ViewModel.api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;


namespace E_School.api.Controllers.api.Student
{
    public class HomeWorkController : ApiController
    {


        HomeWorkRepository bl = new HomeWorkRepository();

        [ActionName("teacherHomework")]
        [HttpGet]
        public List<homeworkModel> teacherHomework([FromUri] int idTeacher)
        {
            return bl.getTeacherHomeWorks(idTeacher);
        }



        [ActionName("teacherRemainHomework")]
        [HttpGet]
        public List<homeworkModel> teacherRemainHomework([FromUri] int idTeacher)
        {
            return bl.teacherRemainHomework(idTeacher);
        }



        [ActionName("HomeWorkRespone")]
        [HttpGet]
        public List<View_HomeWorkResponse> HomeWorkRespone([FromUri] int idHomeWork)
        {
            return bl.homeWorkResponse(idHomeWork);
        }



        [ActionName("StudentHomeWork")]
        [HttpGet]
        public List<homeworkModel> StudentHomeWork([FromUri] int idStudent)
        {
            return bl.studentHomeWork(idStudent);
        }



        [ActionName("studentRemainHomeWork")]
        [HttpGet]
        public List<homeworkModel> studentRemainHomeWork([FromUri] int idStudent)
        {
            return bl.studentRemainHomeWork(idStudent);
        }



        [ActionName("StudentScheduleHomeWork")]
        [HttpGet]
        public List<tbl_homeWorks> StudentScheduleHomeWork([FromUri] int idClass, [FromUri] int idLesson, [FromUri] int idLesson2, [FromUri] int date)
        {
            return bl.studentHomeWorkFromSchedule(idClass, idLesson, idLesson2, date);
        }


        [ActionName("updateHomeWork")]
        [HttpPost]
        public Boolean updateHomeWork([FromBody] tbl_homeWorks entity)
        {
            return bl.Update(entity);
        }



        [ActionName("getHomeWork")]
        [HttpPost]
        public int getHomeWork([FromBody] tbl_homeWorks entity)
        {
            return bl.insert(entity);
        }




        [HttpPost]
        [ActionName("getFile")] // get homeWork file from teacher
        public string getFile()
        {
            int iUploadedCnt = 0;
            System.Web.HttpPostedFile hpf = null;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "", id;
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/homeWorks/homeWorkFile/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // SAVE THE FILES IN THE FOLDER.
                    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    iUploadedCnt = iUploadedCnt + 1;
                }
            }

            return "Files Uploaded Successfully";
        }



        [HttpPost]
        [ActionName("getAnswerFile")] // get homeWork answer file from teacher
        public string getAnswerFile()
        {
            int iUploadedCnt = 0;
            System.Web.HttpPostedFile hpf = null;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "", id;
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/homeWorks/answerFile/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // SAVE THE FILES IN THE FOLDER.
                    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    iUploadedCnt = iUploadedCnt + 1;
                }
            }


            return "Files Uploaded Successfully";
        }



        [HttpPost]
        [ActionName("getHomeworkResponse")]
        public int getHomeworkResponse([FromBody] receivedHomeWorkModel entity)
        {
            return bl.getHomeworkResponse(entity);
        }



        [HttpPost]
        [ActionName("getResponseFile")]
        public string getResponseFile() // response sent for homeWorks by students
        {
            int iUploadedCnt = 0;
            System.Web.HttpPostedFile hpf = null;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "", id;
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/homeWorks/receivedResponse/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // SAVE THE FILES IN THE FOLDER.
                    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    iUploadedCnt = iUploadedCnt + 1;
                }
            }

            return "Files Uploaded Successfully";
        }



        [ActionName("deleteHomeWork")]
        [HttpGet]
        public Boolean deleteHomeWork([FromUri] int idHomeWork)
        {
            return bl.Delete(idHomeWork);
        }



        [ActionName("getHomeWorksCount")]
        [HttpGet]
        public int getHomeWorksCount(int idClass)
        {
            return bl.getHomeWorksCount(idClass);
        }

    }
}