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
using System.Web.Script.Serialization;
using E_School.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace E_School.Controllers.api.Management
{
    public class FinancialController : ApiController
    {
        schoolEntities db = new schoolEntities();
        [ActionName("Studentselect")]
        [HttpGet]
        public HttpResponseMessage Studentselect([FromUri] string Name)
        {
            var Result = (from s in db.tbl_students

                          join rg in db.tbl_studentRegister
                          on s.idStudent equals rg.idStudent
                          where s.FName.Contains(Name) || s.LName.Contains(Name)

                          join y in db.tbl_years
                          on rg.idYear equals y.idYear

                          select new
                          {
                              s.idStudent,
                              rg.idYear,
                              y.yearName,
                              s.FName,
                              s.LName,
                              s.fatherName,
                              s.idStatus

                          }).ToList().Select(s => new
                          {
                              s.idStudent,
                              s.idYear,
                              s.yearName,
                              Name = s.FName + " " + s.LName,
                              s.fatherName,
                              s.idStatus
                          }).ToList();

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };
        }

        [ActionName("financialDetails")]
        [HttpGet]
        public HttpResponseMessage financialDetails([FromUri] int idStud)//
        {


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

                        }).AsEnumerable().Select(d => new
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
                        });

            var Payment = (from p in db.tbl_payments

                           join y in db.tbl_years
                           on p.idYear equals y.idYear

                           join s in db.tbl_students
                           on p.idStud equals s.idStudent
                           where s.idStudent == idStud

                           join pt in db.tbl_payTypes
                           on p.idPayType equals pt.idPayType
                           /*&& p.isOrg == true*/
                           select new
                           {
                               p.cardDate,
                               p.cashDate,
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


                           }).AsEnumerable().Select(p => new
                           {
                               p.chBank,
                               cashDate = p.cashDate.Value.ToSlashDate(),
                               cardDate = p.cardDate.Value.ToSlashDate(),
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
                                   p.cashDate


                               }).AsEnumerable()/*.Where(x => x.isOrg == false)*/.Select(p => new
                               {
                                   p.chBank,
                                   chDate = p.chDate.Value.ToSlashDate(),
                                   p.chNumber,
                                   p.Des,
                                   date = p.Date.ToSlashDate(),
                                   financialDate = p.financialDate.ToSlashDate(),
                                   fishDate = p.fishDate.Value.ToSlashDate(),
                                   cashDate = p.cashDate.Value.ToSlashDate(),
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

            List<LsFinancial> ls = new List<LsFinancial>();
            foreach (var pay in Payment)
            {
                LsFinancial ob = new LsFinancial();
                ob.cashDate = pay.cashDate;
                ob.cardDate = pay.cardDate;
                ob.chBank = pay.chBank;
                ob.chDate = pay.chDate;
                ob.chNumber = (int)pay.chNumber;
                //ob.Date = pay.financialDate;
                ob.Des = pay.Des;
                ob.docNo = pay.docNo;
                ob.financialDate = pay.financialDate;
                ob.fishDate = pay.fishDate;
                ob.fishNumber = pay.fishNumber;
                ob.FName = pay.FName;
                ob.idPay = pay.idPay;
                ob.idPayment = pay.idPay;
                ob.idPayType = pay.idPayType;
                ob.idStud = pay.idStud;
                ob.idYear = pay.idYear;
                ob.isOrg = pay.isOrg;
                ob.issuTracking = pay.issuTracking;
                ob.LName = pay.LName;
                ob.payType = pay.payType;
                ob.value = pay.value;
                ls.Add(ob);
            }

            foreach (var edit in EditPayment)
            {
                LsFinancial ob = new LsFinancial();
                ob.chBank = edit.chBank;
                ob.chDate = edit.chDate;
                ob.cashDate = edit.cashDate;
                ob.chNumber = (int)edit.chNumber;
                ob.Date = edit.financialDate;
                ob.Des = edit.Des;
                ob.docNo = edit.docNo;
                ob.financialDate = edit.financialDate;
                ob.fishDate = edit.fishDate;
                ob.fishNumber = edit.fishNumber;
                ob.FName = edit.FName;
                ob.idPay = edit.idPay;
                ob.idPayment = edit.idPay;
                ob.idPayType = edit.idPayType;
                ob.idStud = edit.idStud;
                ob.idYear = edit.idYear;
                ob.isOrg = edit.isOrg;
                ob.issuTracking = edit.issuTracking;
                ob.LName = edit.LName;
                ob.payType = edit.payType;
                ob.value = edit.value;
                ls.Add(ob);
            }

            string a = "";
            string b = "";
            //string cc = "";
            string join = "";
            var jsonSerialiser = new JavaScriptSerializer();
            //var Paymentjson = jsonSerialiser.Serialize(Payment);
            var Editjson = jsonSerialiser.Serialize(ls.OrderByDescending(x => x.Date));
            var Debtjson = jsonSerialiser.Serialize(Debt);

            //if (Editjson == "[]")
            //{
            //    Editjson = "";
            //}
            if (Editjson != "[]")
            {
                a = Editjson.Substring(0, Editjson.Length - 1);
            }
            if (Debtjson != "[]")
            {
                if (Editjson == "[]")
                {
                    b = Debtjson.Substring(0);
                }
                else
                {
                    b = Debtjson.Substring(1);
                }

            }

            if (a != "" & b != "")
            {
                join = a + "," + b;
            }

            if (a == "")
            {
                join = b;
            }
            if (b == "")
            {
                join = a;
            }

            if (join == ",")
            {
                join = "0";
            }
            if (Debtjson == "[]")
            {
                join += "]";
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    join.ToString()
                )
            };
        }
    }
}