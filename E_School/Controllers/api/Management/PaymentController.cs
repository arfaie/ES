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
using E_School.Models.ViewModel.api;

namespace E_School.Controllers.api.Management
{
    public class PaymentController : ApiController
    {
        PeymentRepository bl = new PeymentRepository();

        [ActionName("select")]
        [HttpGet]
        public HttpResponseMessage select()
        {
            schoolEntities db = new schoolEntities();

            tbl_students stud = new tbl_students();

            var Result = (from p in db.tbl_payments

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
                              pt.payType

                          }).ToList().Select(p => new
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
                              p.payType
                          })
                                 .ToList();
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(Result);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };
        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]tbl_payments entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {

                    Random rnd = new Random();
                    int random = rnd.Next(1, 99999);
                    while (bl.Where(x => x.docNo == random).FirstOrDefault() != null)
                    {
                        random = rnd.Next(1, 99999);
                    }
                    entity.docNo = random;
                    entity.isOrg = true;
                    entity.idPay = bl.GetLastIdentity(true) + 1;

                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idPay.ToString());
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
        public bool Update([FromBody]tbl_editTransections entity)//Here we add a new record
        {
            try
            {

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    int oldValue = 0;
                    MessageController msg = new MessageController();
                    MessageRepository msgBL = new MessageRepository();
                    messageRecieversModel ob = new messageRecieversModel();
                    var selectPayment = bl.Where(x => x.idPay == entity.idPayment && x.isOrg==true).FirstOrDefault();
                    var selectEdti=bl.WhereEdid(x=>x.idPayment==entity.idPayment && x.isOrg == true).FirstOrDefault();
                    if (selectEdti != null)
                    {
                        oldValue = selectEdti.value;
                    }
                    if (selectPayment != null)
                    {
                        oldValue = selectPayment.value;
                    }
                    ob.body = " شماره سند " + selectPayment.docNo + "\n" + "\n" + " تاریخ سند " + selectPayment.financialDate.ToSlashDate() + "\n" + "\n" + " مبلغ جدید " + entity.value + "\n" + "\n" + " مبلغ قبلی " + oldValue;
                    int msgDate = 0;
                    int idStud = selectPayment.idStud;
                    ob.date = msgDate.GetPersianDate();
                    ob.idMessage = msgBL.GetLastIdentity() + 1;
                    ob.idMessageType = 1;
                    ob.idReciever = idStud;
                    ob.idRecieverType = 3;
                    ob.idSender = 1;
                    ob.idSenderType = 1;
                    ob.title = "ویرایش سند پرداخت";
                    msg.Send(ob);

                    entity.idTrans = bl.GetLastIdentity(false) + 1;
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