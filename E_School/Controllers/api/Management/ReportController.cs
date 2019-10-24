using E_School.Helpers.Utitlies;
using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.ViewModel;
using E_School.Models.ViewModel.Management;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using static E_School.Helpers.Utitlies.Custom_Exception;

namespace E_School.Controllers.api.Management
{
    public class ReportController : ApiController
    {
        Methods Method = new Methods();
        ExamRepository bl = new ExamRepository();
        schoolEntities db = new schoolEntities();
        NowDateRepository blDate = new NowDateRepository();
        SettingRepository blSetting = new SettingRepository();
        [ActionName("Report")]
        [HttpGet]
        public int Report([FromUri]int idYear, int idClass, int Date1, int Date2, int idType)
        {
            if (idType == 1)
            {
                Exam(idYear, idClass, Date1, Date2);
            }

            if (idType == 2)
            {
                Payments(idYear, idClass, Date1, Date2);
            }

            if (idType == 3)
            {
                Debts(idYear, idClass, Date1, Date2);
            }

            if (idType == 4)
            {
                Offices(idYear, idClass, Date1, Date2);
            }

            if (idType == 5)
            {
                Teachers(idYear, idClass, Date1, Date2);
            }

            return 1;
        }

        [ActionName("Exam")]
        [HttpGet]
        public HttpResponseMessage Exam([FromUri]int idYear, int idClass, int Date1, int Date2)
        {
            CreateDir("برنامه امتحانی");
            string aa = "";
            try
            {
                blDate.Update();

                if (Date1 == 0 & Date2 == 0)
                {
                    Date2 = 99999999;
                }

                var Result = (from e in db.tbl_exams

                              join c in db.tbl_classes
                              on e.idClass equals c.idClass
                              where e.idClass == idClass

                              join v in db.tbl_levels
                             on c.idLevel equals v.idLevel

                              join term in db.tbl_terms
                              on e.idTerm equals term.idTerm
                              where term.idYear == idYear

                              join eType in db.tbl_examTypes
                              on e.idExamType equals eType.idExamType

                              join l in db.tbl_lessons
                              on e.idLesson equals l.idLesson


                              select new
                              {
                                  e.examDate,
                                  e.examTitle,
                                  e.idClass,
                                  e.idExam,
                                  e.idExamType,
                                  e.idLesson,
                                  e.idTerm,
                                  e.maxScore,
                                  eType.examType,
                                  l.lessonName,
                                  c.className,
                                  term.termName,
                                  e.des,
                                  v.levelName

                              }).AsEnumerable().Select(e => new
                              {
                                  Date = e.examDate.ToSlashDate(),
                                  e.des,
                                  e.examTitle,
                                  e.idClass,
                                  e.idExam,
                                  e.idExamType,
                                  e.idLesson,
                                  e.idTerm,
                                  e.maxScore,
                                  e.examType,
                                  e.lessonName,
                                  e.className,
                                  e.termName,
                                  e.levelName

                              });

                ReportSetting Setting = new ReportSetting();
                Setting.Date = blDate.Find(1).shamsiDate;
                Setting.SchoolName = blSetting.Find(1).schoolName;
                ExportPDF("Exam", Result, "Exam", CreateDir("برنامه امتحانی") + "/" + Result.FirstOrDefault().className, Setting);

            }


            catch (Exception ex)
            {
                aa = ex.InnerException.ToString();
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    aa

                )
            };
        }

        [ActionName("Payments")]
        [HttpGet]
        public HttpResponseMessage Payments([FromUri]int idYear, int idClass, int Date1, int Date2)
        {
            CreateDir("مالی");
            blDate.Update();

            if (Date1 == 0 & Date2 == 0)
            {
                Date2 = 99999999;
            }
            var Payment = (from p in db.tbl_payments

                           join s in db.tbl_students
                           on p.idStud equals s.idStudent

                           join pt in db.tbl_payTypes
                           on p.idPayType equals pt.idPayType
                           where p.financialDate > Date1 & p.financialDate < Date2

                           select new
                           {

                               p.Des,
                               p.financialDate,
                               p.value,
                               s.FName,
                               s.LName,
                               pt.payType,
                               p.isOrg,
                               p.docNo,


                           }).ToList().Where(x => x.isOrg == true).Select(p => new
                           {
                               p.Des,
                               financialDate = p.financialDate.ToSlashDate(),
                               p.value,
                               Name = p.FName + " " + p.LName,
                               payType = /*"پرداخت " +*/ p.payType,
                               p.docNo
                           }).ToList();

            LsDates obDates = new LsDates();
            if (Date2 != 99999999)
            {
                obDates.Date1 = Date1.ToSlashDate();
                obDates.Date2 = Date2.ToSlashDate();
            }

            //LsFinancial ob;
            //List<LsFinancial> ls = new List<LsFinancial>();
            //int payments = Payment.AsEnumerable().Sum(x => x.value);

            string a = "";
            string b = "";

            string join = "";
            var jsonSerialiser = new JavaScriptSerializer();

            var Editjson = jsonSerialiser.Serialize(Payment.OrderBy(x => x.financialDate));
            //var Debtjson = jsonSerialiser.Serialize(ls);

            if (Editjson == "[]")
            {
                Editjson = "";
            }

            //if (Debtjson == "[]")
            //{
            //    Debtjson = "";
            //}

            //a = Editjson.Substring(0, Editjson.Length - 1);
            //b = Debtjson.Substring(1);

            join = a + "," + b;


            if (join == "[,]")
            {
                join = "0";
            }

            ReportSetting Setting = new ReportSetting();
            Setting.Date = blDate.Find(1).shamsiDate;
            Setting.SchoolName = blSetting.Find(1).schoolName;
            ExportPDF("Financial", Payment, "Payments", CreateDir("مالی") + "/" + "لیست پرداخت ها",Setting, "Dates",obDates);

            return new HttpResponseMessage()
            {
                Content = new StringContent("1")
            };
        }

        [ActionName("Debts")]
        [HttpGet]
        public HttpResponseMessage Debts([FromUri]int idYear, int idClass, int Date1, int Date2)
        {
            CreateDir("مالی");
            blDate.Update();

            if (Date1 == 0 & Date2 == 0)
            {
                Date2 = 99999999;
            }

            LsDates obDates = new LsDates();
            if (Date2 != 99999999)
            {
                obDates.Date1 = Date1.ToSlashDate();
                obDates.Date2 = Date2.ToSlashDate();
            }

            var Debt = (from d in db.tbl_debts

                        join y in db.tbl_years
                        on d.idYear equals y.idYear

                        join s in db.tbl_students
                        on d.idStud equals s.idStudent

                        join c in db.tbl_costs
                        on d.idCost equals c.idCost

                        join rg in db.tbl_registrationCourses
                        on d.idRegCourse equals rg.idRegcourse into ulist
                        from u in ulist.DefaultIfEmpty()

                        select new
                        {
                            d.Date,

                            d.idCost,
                            d.idDebt,
                            d.idRegCourse,
                            d.idStud,
                            d.idYear,
                            y.yearName,
                            s.LName,
                            s.FName,
                            costTitle = c.Name,
                            costValue = c.Value,
                            regCoreTitle = u.title,
                            regCoreValue = u.value

                        }).ToList().Select(d => new
                        {
                            Date = d.Date.ToSlashDate(),

                            d.idCost,
                            d.idDebt,
                            d.idRegCourse,
                            d.idStud,
                            d.idYear,
                            d.yearName,
                            d.LName,
                            d.FName,
                            d.regCoreTitle,
                            d.costTitle,
                            d.costValue,
                            d.regCoreValue
                        })
                                 .ToList();

            var Payment = (from p in db.tbl_payments

                           join y in db.tbl_years
                           on p.idYear equals y.idYear

                           join s in db.tbl_students
                           on p.idStud equals s.idStudent

                           join pt in db.tbl_payTypes
                           on p.idPayType equals pt.idPayType

                           select new
                           {
                               p.chBank,
                               p.chDate,
                               p.chNumber,
                               p.Des,
                               p.financialDate,
                               p.fishDate,
                               p.fishNumber,
                               p.idPay,
                               p.idPayType,
                               p.idStud,
                               p.idYear,
                               p.issuTracking,
                               p.value,
                               s.FName,
                               s.LName,
                               pt.payType,
                               p.isOrg,
                               p.docNo,


                           }).AsEnumerable().Where(x => x.isOrg == true).Select(p => new
                           {
                               p.chBank,
                               chDate = p.chDate.Value.ToSlashDate(),
                               p.chNumber,
                               p.Des,
                               financialDate = p.financialDate.ToSlashDate(),
                               fishDate = p.fishDate.Value.ToSlashDate(),
                               p.fishNumber,
                               p.idPay,
                               p.idPayType,
                               p.idStud,
                               p.idYear,
                               p.issuTracking,
                               p.value,
                               p.FName,
                               p.LName,
                               p.payType,
                               p.isOrg,
                               p.docNo
                           });


            LsFinancial ob;
            List<LsFinancial> ls = new List<LsFinancial>();
            List<int> LsidStud = new List<int>();

            foreach (var pay in Payment)
            {
                if (!LsidStud.Contains(pay.idStud))
                {

                    LsidStud.Add(pay.idStud);

                    ob = new LsFinancial();

                    ob.docNo = pay.docNo;
                    ob.financialDate = pay.financialDate;

                    ob.FName = pay.FName + " " + pay.LName;
                    ob.idStud = pay.idStud;
                    if (pay.payType == "تخفیف")
                    {
                        ob.payType = pay.payType;
                    }
                    else
                    {
                        ob.payType = "پرداخت " + pay.payType;
                    }
                    ob.value = Payment.Where(x => x.idStud == pay.idStud).Sum(x => x.value);
                    ob.regCorseTitle = "";
                    ob.costTitle = "";
                    ls.Add(ob);
                    LsidStud.Add(pay.idStud);
                }

            }
            LsidStud.Clear();
            foreach (var d in Debt)
            {

                if (!LsidStud.Contains(d.idStud))
                {
                    ob = new LsFinancial();
                    ob.idStud = d.idStud;
                    ob.regCorseTitle = d.regCoreTitle;
                    ob.costTitle = d.costTitle;
                    ob.costValue = Debt.Where(x => x.idStud == d.idStud).Sum(x => x.costValue) + Debt.Where(x => x.idStud == d.idStud).Sum(x => x.regCoreValue);
                    ob.Date = d.Date;
                    ob.payType = "";
                    ls.Add(ob);
                    LsidStud.Add(d.idStud);
                }

            }

            LsidStud.Clear();
            List<LsDebts> lsDebts = new List<LsDebts>();
            int total = 0;
            foreach (var t in ls.ToList())
            {
                total = 0;
                if (!LsidStud.Contains(t.idStud))
                {
                    LsDebts obDebts = new LsDebts();
                    int pay = ls.Where(x => x.idStud == t.idStud).Sum(x => x.value);
                    int cost = ls.Where(x => x.idStud == t.idStud).Sum(x => x.costValue);
                    total = pay - cost;
                    if (total < 0)
                    {
                        obDebts.total = total;
                        obDebts.idStud = t.idStud;
                        lsDebts.Add(obDebts);
                    }

                    LsidStud.Add(t.idStud);
                }
            }

            var Students = db.tbl_students.ToList();

            var Result = (from s in Students

                          join l in lsDebts
                          on s.idStudent equals l.idStud



                          select new
                          {
                              s.FName,
                              s.LName,
                              s.fatherName,
                              s.studentMobile,
                              s.homePhone,
                              s.fPhone,
                              l.total

                          }).OrderBy(x => x.total).AsQueryable().Select(s => new
                          {

                              Name = s.FName + " " + s.LName,
                              s.fatherName,
                              s.studentMobile,
                              s.homePhone,
                              s.fPhone,
                              s.total
                          });

            ReportSetting Setting = new ReportSetting();
            Setting.Date = blDate.Find(1).shamsiDate;
            Setting.SchoolName = blSetting.Find(1).schoolName;
            ExportPDF("Financial", Result, "Debts", CreateDir("مالی") + "/" + "لیست بدهکاران", Setting,"Dates", obDates);

            return new HttpResponseMessage()
            {
                Content = new StringContent("1")
            };
        }

        [ActionName("StudDisciplines")]//Ehsan
        [HttpGet]
        public int StudDisciplines([FromUri]int Date, int idStud)
        {
            blDate.Update();
            var select = (from d in db.tbl_studentsDisciplines

                          join s in db.tbl_students
                          on d.idStudent equals s.idStudent
                          where s.idStudent == idStud

                          join t in db.tbl_disciplineTypes
                          on d.idDisType equals t.idDisType

                          join term in db.tbl_terms
                          on d.idTerm equals term.idTerm
                          where term.termStart < Date & term.termEnd > Date

                          join y in db.tbl_years
                          on term.idYear equals y.idYear


                          select new
                          {
                              s.FName,
                              s.LName,
                              term.termName,
                              t.disName,
                              d.justified,
                              d.actDate,

                          }).AsEnumerable().Select(d => new
                          {
                              Name = d.FName + " " + d.LName,
                              d.termName,
                              d.disName,
                              d.justified,
                              actDate = d.actDate.ToSlashDate(),
                          });

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(select);

            return 1;

        }

        [ActionName("Offices")]
        [HttpGet]
        public HttpResponseMessage Offices([FromUri]int idYear, int idClass, int Date1, int Date2)
        {
            CreateDir("پرسنل");
            blDate.Update();
            var select = (from d in db.tbl_offices

                          select new
                          {
                              d.FName,
                              d.LName,
                              d.fatherName,
                              d.natCode,
                              d.personalCode,
                              d.birthDate,
                              d.mobile,
                              d.phone,
                              d.idOffices

                          }).AsEnumerable().Where(x => x.idOffices > 0).Select(d => new
                          {
                              d.fatherName,
                              d.natCode,
                              d.personalCode,
                              birthDate = d.birthDate.ToSlashDate(),
                              d.mobile,
                              d.phone,
                              Name = d.FName + " " + d.LName
                          });

            ReportSetting Setting = new ReportSetting();
            Setting.Date = blDate.Find(1).shamsiDate;
            Setting.SchoolName = blSetting.Find(1).schoolName;
            ExportPDF("Offices", select, "Offices", CreateDir("پرسنل") + "/" + "لیست پرسنل",Setting);


            return new HttpResponseMessage()
            {
                Content = new StringContent("1")
            };
        }

        [ActionName("Teachers")]
        [HttpGet]
        public HttpResponseMessage Teachers([FromUri]int idYear, int idClass, int Date1, int Date2)
        {
            CreateDir("پرسنل");
            blDate.Update();
            var select = (from d in db.tbl_teachers

                          select new
                          {
                              d.FName,
                              d.LName,
                              d.fatherName,
                              d.natCode,
                              d.personalCode,
                              d.birthDate,
                              d.mobile,
                              d.phone,
                              d.idTeacher

                          }).AsEnumerable().Where(x => x.idTeacher > 0).Select(d => new
                          {
                              d.fatherName,
                              d.natCode,
                              d.personalCode,
                              birthDate = d.birthDate.ToSlashDate(),
                              d.mobile,
                              d.phone,
                              Name = d.FName + " " + d.LName
                          });

            ReportSetting Setting = new ReportSetting();
            Setting.Date = blDate.Find(1).shamsiDate;
            Setting.SchoolName = blSetting.Find(1).schoolName;
            ExportPDF("Teachers", select, "Teachers", CreateDir("پرسنل") + "/" + "لیست معلمین",Setting);

            return new HttpResponseMessage()
            {
                Content = new StringContent("")
            };
        }

        [ActionName("RegFinancial")]//ok
        [HttpGet]
        public string RegFinancial([FromUri]int idStud)
        {

            //string financialPath = "مالی/" + "برگ ثبت نام";
            //CreateDir(financialPath);
            string aa = "";
            try
            {

                var Student = (from s in db.tbl_students

                               join e in db.tbl_educations
                               on s.idFatherEdu equals e.idEdu
                               where s.idStudent == idStud
                               join ed in db.tbl_educations
                               on s.idMotherEdu equals ed.idEdu

                               join m in db.tbl_marriges
                               on s.idMarrige equals m.idMarrige

                               select new
                               {
                                   s.birthDate,
                                   s.fatherName,
                                   s.fJob,
                                   s.fJobAddress,
                                   s.fJobPhone,
                                   s.FName,
                                   s.fPhone,
                                   s.gender,
                                   s.homeAddress,
                                   s.homePhone,
                                   FatherEdu = e.eduName,
                                   MotherEdu = ed.eduName,
                                   s.idMotherEdu,
                                   s.idFatherEdu,
                                   m.marrigeName,
                                   s.idStatus,
                                   s.idStudent,
                                   s.imgManifest,
                                   s.imgPersonal,
                                   s.isActive,
                                   s.leftHand,
                                   s.LName,
                                   s.mJob,
                                   s.mJobAddress,
                                   s.mJobPhone,
                                   s.motherName,
                                   s.mPhone,
                                   s.natCode,
                                   s.pass,
                                   s.pPass,
                                   s.pUser,
                                   s.religion,
                                   s.studentCode,
                                   s.studentMobile,
                                   s.studUser
                               }).ToList().Select(s => new
                               {
                                   
                                   birthDate = s.birthDate.ToLongSlashDate(),
                                   s.fatherName,
                                   s.fJob,
                                   s.fJobAddress,
                                   s.fJobPhone,
                                   Name = s.FName + " " + s.LName,
                                   s.fPhone,
                                   s.gender,
                                   s.homeAddress,
                                   s.homePhone,
                                   FatherEdu = s.FatherEdu,
                                   MotherEdu = s.MotherEdu,
                                   s.idMotherEdu,
                                   s.idFatherEdu,
                                   s.marrigeName,
                                   s.idStatus,
                                   s.idStudent,
                                   s.imgManifest,
                                   s.imgPersonal,
                                   s.isActive,
                                   s.leftHand,

                                   s.mJob,
                                   s.mJobAddress,
                                   s.mJobPhone,
                                   s.motherName,
                                   s.mPhone,
                                   s.natCode,
                                   s.pass,
                                   s.pPass,
                                   s.pUser,
                                   s.religion,
                                   s.studentCode,
                                   s.studentMobile,
                                   s.studUser
                               })
                                                .ToList();


                var select = StudentFinancial(idStud);
                //ExportPDF("Financial", select, "RegFinancial", CreateStudDir( + "/" + Student.FirstOrDefault().Name, "Student", Student);
                try
                {
                    ReportSetting Setting = new ReportSetting();
                    Setting.Date = blDate.Find(1).shamsiDate;
                    Setting.SchoolName = blSetting.Find(1).schoolName;
                    ExportPDF("Financial", select, "RegFinancial", CreateStudDir(idStud) + "/" + "برگ ثبت نام",Setting, "Student", Student);
                    return (1).ToString();
                }
                catch (DbEntityValidationException e)
                {
                    var newException = new FormattedDbEntityValidationException(e);
                    StreamWriter sw = new StreamWriter("~/Content/Reports/txt2.log", false);
                    sw.WriteLine(newException.Message);
                    sw.WriteLine(newException.InnerException);
                    sw.WriteLine(newException.Source);
                    return newException.InnerException.ToString();
                }

            }

            catch (Exception ex)
            {
                aa = ex.InnerException.ToString();
                return (0).ToString();
            }

        }

        [ActionName("StudFinancial")]//Ehsan
        [HttpGet]
        public int StudFinancial([FromUri]int idStud)
        {

            try
            {
                string financialPath = "مالی/" + "فاکتور خدمات";
                CreateDir(financialPath);

                var Student = (from s in db.tbl_students
                               where s.idStudent == idStud
                               select new
                               {
                                   s.FName,
                                   s.LName
                               }).AsEnumerable().Select(s => new
                               {

                                   Name = s.FName + " " + s.LName,

                               });
                blDate.Update();
                var select = StudentFinancial(idStud);

                Methods Methods = new Methods();
                //Create Folder
                //string Path = Methods.DiPath(entity.idYear, entity.idClass, entity.idStudent);
                ReportSetting Setting = new ReportSetting();
                Setting.Date = blDate.Find(1).shamsiDate;
                Setting.SchoolName = blSetting.Find(1).schoolName;
                ExportPDF("Financial", select, "StudFinancial", CreateStudDir(idStud) + "/" + "فاکتور خدمات", Setting,"Student", Student);
            }

            catch (Exception ex)
            {
                ex.ToString();
            }

            return 1;
        }

        /*All Students*/

        [ActionName("allStudFinancial")]
        [HttpGet]
        public int allStudFinancial()
        {
            List<tbl_students> lsStud = db.tbl_students.ToList();
            //int count = lsStud.Count;
            //int[] id=new int[count];
            foreach (var idStud_ in lsStud)
            {
                //int temp = 0;
                //id[temp] = s.idStudent;
                //temp++;

                try
                {
                    string financialPath = "مالی/" + "فاکتور خدمات";
                    CreateDir(financialPath);

                    var Student = (from s in db.tbl_students
                                   where s.idStudent == idStud_.idStudent
                                   select new
                                   {
                                       s.FName,
                                       s.LName
                                   }).AsEnumerable().Select(s => new
                                   {

                                       Name = s.FName + " " + s.LName,

                                   });
                    blDate.Update();
                    var select = StudentFinancial(idStud_.idStudent);

                    Methods Methods = new Methods();
                    //Create Folder
                    //string Path = Methods.DiPath(entity.idYear, entity.idClass, entity.idStudent);
                    ReportSetting Setting = new ReportSetting();
                    Setting.Date = blDate.Find(1).shamsiDate;
                    Setting.SchoolName = blSetting.Find(1).schoolName;
                    ExportPDF("Financial", select, "StudFinancial", CreateStudDir(idStud_.idStudent) + "/" + "فاکتور خدمات", Setting,"Student", Student);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }


            return 1;
        }

        /*All Students*/
        public string ExportPDF(string firstReg, object first, string stimulName, string pdfName, object Setting, string secondReg = null, object second = null)
        {
            string a = "";
            try
            {
                StiReport report = new StiReport();
                string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Reports/");
                report.Load(Path + stimulName + ".mrt");
                report.RegData(firstReg, first);
                report.RegData("Setting", Setting);
                if (secondReg != null)
                {
                    report.RegData(secondReg, second);
                }
                StiOptions.Export.Pdf.ReduceFontFileSize = false;
                report.Render();
                StiPdfExportService pdfExport = new StiPdfExportService();
                pdfExport.ExportPdf(report, pdfName + ".pdf");

            }
            catch (DbEntityValidationException e)
            {
                var newException = new FormattedDbEntityValidationException(e);
                StreamWriter sw = new StreamWriter("~/Content/Reports/txt.log", false);
                sw.WriteLine(newException.Message);
                sw.WriteLine(newException.InnerException);
                sw.WriteLine(newException.Source);
                //return ;
            }

            return a;
        }

        //----------------------------------------------Help Methods
        public IQueryable StudentFinancial(int idStud)
        {
            List<LsFinancial> ls = new List<LsFinancial>();
            try
            {
                blDate.Update();
                var Debt = (from d in db.tbl_debts

                            join y in db.tbl_years
                            on d.idYear equals y.idYear

                            join s in db.tbl_students
                            on d.idStud equals s.idStudent

                            join c in db.tbl_costs
                            on d.idCost equals c.idCost

                            join rg in db.tbl_registrationCourses
                            on d.idRegCourse equals rg.idRegcourse into ulist
                            from u in ulist.DefaultIfEmpty()
                            where d.idStud == idStud
                            select new
                            {
                                d.Date,

                                d.idCost,
                                d.idDebt,
                                d.idRegCourse,
                                d.idStud,
                                d.idYear,
                                y.yearName,
                                s.LName,
                                s.FName,
                                costTitle = c.Name,
                                costValue = c.Value,
                                regCoreTitle = u.title,
                                regCoreValue = u.value

                            }).ToList().Select(d => new
                            {
                                Date = d.Date.ToSlashDate(),

                                d.idCost,
                                d.idDebt,
                                d.idRegCourse,
                                d.idStud,
                                d.idYear,
                                d.yearName,
                                d.LName,
                                d.FName,
                                d.regCoreTitle,
                                d.costTitle,
                                d.costValue,
                                d.regCoreValue
                            })
                                     .ToList();

                var Payment = (from p in db.tbl_payments

                               join y in db.tbl_years
                               on p.idYear equals y.idYear

                               join s in db.tbl_students
                               on p.idStud equals s.idStudent

                               join pt in db.tbl_payTypes
                               on p.idPayType equals pt.idPayType
                               where s.idStudent == idStud
                               select new
                               {
                                   p.chBank,
                                   p.chDate,
                                   p.chNumber,
                                   p.Des,
                                   p.cashDate,
                                   p.financialDate,
                                   p.fishDate,
                                   p.fishNumber,
                                   p.idPay,
                                   p.idPayType,
                                   p.idStud,
                                   p.idYear,
                                   p.issuTracking,
                                   p.value,
                                   s.FName,
                                   s.LName,
                                   pt.payType,
                                   p.isOrg,
                                   p.docNo,


                               }).ToList().Where(x => x.isOrg == true).Select(p => new
                               {
                                   p.chBank,
                                   chDate = p.chDate.Value.ToSlashDate(),
                                   p.chNumber,
                                   p.Des,
                                   p.cashDate,
                                   financialDate = p.financialDate.ToSlashDate(),
                                   fishDate = p.fishDate.Value.ToSlashDate(),
                                   p.fishNumber,
                                   p.idPay,
                                   p.idPayType,
                                   p.idStud,
                                   p.idYear,
                                   p.issuTracking,
                                   p.value,
                                   p.FName,
                                   p.LName,
                                   p.payType,
                                   p.isOrg,
                                   p.docNo
                               })
                                     .ToList();

                var EditPayment = (from p in db.tbl_payments

                                   join e in db.tbl_editTransections
                                   on p.idPay equals e.idPayment

                                   join s in db.tbl_students
                                   on p.idStud equals s.idStudent

                                   join pt in db.tbl_payTypes
                                   on p.idPayType equals pt.idPayType
                                   where s.idStudent == idStud
                                   select new
                                   {
                                       p.chBank,
                                       p.chDate,
                                       p.chNumber,
                                       e.Des,
                                       p.financialDate,
                                       e.Date,
                                       p.fishDate,
                                       p.fishNumber,
                                       p.idPay,
                                       p.idPayType,
                                       p.idStud,
                                       p.idYear,
                                       p.issuTracking,
                                       e.value,
                                       s.FName,
                                       s.LName,
                                       pt.payType,
                                       e.isOrg,
                                       p.docNo,
                                       e.idPayment,


                                   }).AsEnumerable()/*.Where(x => x.isOrg == false)*/.Select(p => new
                                   {
                                       p.chBank,
                                       chDate = p.chDate.Value.ToSlashDate(),
                                       p.chNumber,
                                       p.Des,
                                       date = p.Date.ToSlashDate(),
                                       financialDate = p.financialDate.ToSlashDate(),
                                       fishDate = p.fishDate.Value.ToSlashDate(),
                                       p.fishNumber,
                                       p.idPay,
                                       p.idPayType,
                                       p.idStud,
                                       p.idYear,
                                       p.issuTracking,
                                       p.value,
                                       p.FName,
                                       p.LName,
                                       p.payType,
                                       p.isOrg,
                                       p.docNo
                                   }).ToList();

                var Student = (from s in db.tbl_students

                               join e in db.tbl_educations
                               on s.idFatherEdu equals e.idEdu
                               where s.idStudent == idStud
                               join ed in db.tbl_educations
                               on s.idMotherEdu equals ed.idEdu

                               join m in db.tbl_marriges
                               on s.idMarrige equals m.idMarrige

                               select new
                               {
                                   s.birthDate,
                                   s.fatherName,
                                   s.fJob,
                                   s.fJobAddress,
                                   s.fJobPhone,
                                   s.FName,
                                   s.fPhone,
                                   s.gender,
                                   s.homeAddress,
                                   s.homePhone,
                                   FatherEdu = e.eduName,
                                   MotherEdu = ed.eduName,
                                   s.idMotherEdu,
                                   s.idFatherEdu,
                                   m.marrigeName,
                                   s.idStatus,
                                   s.idStudent,
                                   s.imgManifest,
                                   s.imgPersonal,
                                   s.isActive,
                                   s.leftHand,
                                   s.LName,
                                   s.mJob,
                                   s.mJobAddress,
                                   s.mJobPhone,
                                   s.motherName,
                                   s.mPhone,
                                   s.natCode,
                                   s.pass,
                                   s.pPass,
                                   s.pUser,
                                   s.religion,
                                   s.studentCode,
                                   s.studentMobile,
                                   s.studUser
                               }).ToList().Select(s => new
                               {
                                   birthDate = s.birthDate.ToLongSlashDate(),
                                   s.fatherName,
                                   s.fJob,
                                   s.fJobAddress,
                                   s.fJobPhone,
                                   Name = s.FName + " " + s.LName,
                                   s.fPhone,
                                   s.gender,
                                   s.homeAddress,
                                   s.homePhone,
                                   FatherEdu = s.FatherEdu,
                                   MotherEdu = s.MotherEdu,
                                   s.idMotherEdu,
                                   s.idFatherEdu,
                                   s.marrigeName,
                                   s.idStatus,
                                   s.idStudent,
                                   s.imgManifest,
                                   s.imgPersonal,
                                   s.isActive,
                                   s.leftHand,

                                   s.mJob,
                                   s.mJobAddress,
                                   s.mJobPhone,
                                   s.motherName,
                                   s.mPhone,
                                   s.natCode,
                                   s.pass,
                                   s.pPass,
                                   s.pUser,
                                   s.religion,
                                   s.studentCode,
                                   s.studentMobile,
                                   s.studUser
                               })
                                                .ToList();


                LsFinancial ob;

                int payments = Payment.AsEnumerable().Sum(x => x.value);
                int editPayment_ = EditPayment.AsEnumerable().Sum(x => x.value);
                int debts = Debt.AsEnumerable().Sum(x => x.costValue) + Debt.AsEnumerable().Sum(x => x.regCoreValue);
                int total = payments+ editPayment_ - debts;

                foreach (var pay in Payment)
                {
                    ob = new LsFinancial();

                    ob.docNo = pay.docNo;
                    ob.financialDate = pay.financialDate;

                    ob.FName = pay.FName + " " + pay.LName;
                    if (pay.payType == "تخفیف")
                    {
                        ob.payType = pay.payType;
                    }
                    else
                    {
                        ob.payType = "پرداخت " + pay.payType;
                    }
                    
                    ob.value = pay.value;
                    ob.regCorseTitle = "";
                    ob.costTitle = "";
                    ls.Add(ob);
                }
                foreach (var editPayment in EditPayment)
                {
                    if (editPayment.isOrg == true)
                    {
                        ob = new LsFinancial();

                        ob.docNo = editPayment.docNo;
                        ob.financialDate = editPayment.financialDate;

                        ob.FName = editPayment.FName + " " + editPayment.LName;
                        if (editPayment.payType == "تخفیف")
                        {
                            ob.payType = editPayment.payType;
                        }
                        else
                        {
                            ob.payType = "پرداخت " + editPayment.payType;
                        }

                        ob.value = editPayment.value;
                        ob.regCorseTitle = "";
                        ob.costTitle = "";
                        ls.Add(ob);
                    }
                    
                }
                foreach (var edit in Debt)
                {
                    ob = new LsFinancial();
                    ob.regCorseTitle = edit.regCoreTitle;
                    ob.costTitle = edit.costTitle;
                    ob.costValue = edit.costValue + edit.regCoreValue;
                    ob.Date = edit.Date;
                    ob.total = total;
                    ob.payType = "";
                    ls.Add(ob);
                }



            }

            catch (Exception ex)
            {
                ex.InnerException.ToString();

            }
            return ls.AsQueryable();
        }

        public string CreateDir(string Name)
        {
            //string yearName = YearName(entity.idYear);
            string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/گزارشات/" + "مدیریت/" + Name);
            DirectoryInfo di = Directory.CreateDirectory(Path);
            return Path;
        }
        public string CreateStudDir(int idStud)
        {
            StudentRegisterRepository studReg = new StudentRegisterRepository();
            var select = studReg.Where(x => x.idStudent == idStud).OrderByDescending(x => x.idStudReg).FirstOrDefault();
            if (select != null)
            {
                int idClass = select.idClass;
                var Path = Method.DiPath(idClass, idStud);
                DirectoryInfo di = Directory.CreateDirectory(Path);
                return Path;
            }
            return null;

        }
    }
}
