using E_School.Models.DomainModels;
using E_School.Models.EntityModels;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace E_School.Controllers.api.Management
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("Index");
        }

        //public ActionResult GetReportSnapshotIEnumerable()
        //{
        //    StiReport report = new StiReport();
        //    report.Load(Server.MapPath("~/Content/BusinessObjects_IEnumerable.mrt"));
        //    report.RegData("EmployeeIEnumerable", CreateBusinessObjectsIEnumerable.GetEmployees());

        //    return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);
        //}

        //public ActionResult ViewerEvent()
        //{
        //    return StiMvcViewer.ViewerEventResult(HttpContext);
        //}

        //public ActionResult PrintReport()
        //{
        //    return StiMvcViewer.PrintReportResult(HttpContext);
        //}

        //public FileResult ExportReport()
        //{
        //    return StiMvcViewer.ExportReportResult(HttpContext);
        //}

        //public ActionResult Interaction()
        //{
        //    return StiMvcViewer.InteractionResult(HttpContext);
        //}

        //[HttpGet]
        //public JsonResult BookingStatus()
        //{
            //Creating Web Service reference object  
            //myws objPayRef = new myws();
            //int b = 12;
            ////calling and storing web service output into the variable  
            //var BookingStatusInfo = objPayRef.HelloWorld();
            ////returning josn result  
            //return Json(BookingStatusInfo, JsonRequestBehavior.AllowGet);

        //}  

    }
}
