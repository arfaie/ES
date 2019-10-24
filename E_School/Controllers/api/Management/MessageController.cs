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
using E_School.Models.ViewModel;
using E_School.Models.ViewModel.Management;
using E_School.Models.ViewModel.api;
using System.IO;
using E_School.Helpers.Utitlies;


namespace E_School.Controllers.api.Management
{
    public class MessageController : ApiController
    {
        MessageRepository bl = new MessageRepository();

        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage Select([FromUri]int Start, [FromUri]int End)//return All messages | all recived | all send
        {
            if (Start == 0 && End == 0)
            {
                End = 99999999;
            }
            var sender = bl.Messages();
            var reciver = bl.PTSentMsg();

            schoolEntities db = new schoolEntities();

            var Result = (from s in sender
                          join r in reciver
                          on s.idMessage equals r.idMessage

                          join t in db.tbl_msgTypes
                          on s.idMessageType equals t.idMsgType
                          select new

                          {
                              r.body,
                              r.date,
                              r.idMessage,
                              r.idMessageType,
                              r.isRead,
                              s.senderName,
                              r.title,
                              r.recieverName,
                              r.idReceiverType,
                              r.idSenderType,
                              t.Type

                          }).ToList().Where(x => x.date < End && x.date > Start).Select(r => new
                          {
                              r.body,
                              date = r.date.ToSlashDate(),
                              r.idMessage,
                              r.idMessageType,
                              r.isRead,
                              r.senderName,
                              r.title,
                              r.recieverName,
                              r.Type,
                              r.idReceiverType,
                              r.idSenderType,
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

        [ActionName("SendList")]
        [HttpGet]
        public HttpResponseMessage SendList([FromUri]int Start, [FromUri]int End)
        {
            if (Start == 0 && End == 0)
            {
                End = 99999999;
            }
            var sender = bl.Messages().Where(x => x.idSenderType == 1 && x.isActiveSend == true);

            schoolEntities db = new schoolEntities();

            var Result = (from s in sender
                          join t in db.tbl_msgTypes
                          on s.idMessageType equals t.idMsgType

                          select new

                          {
                              s.body,
                              s.date,
                              s.idMessage,
                              s.idMessageType,
                              s.isRead,
                              s.senderName,
                              s.title,
                              s.recieverName,
                              s.idReceiverType,
                              s.idSenderType,
                              t.Type

                          }).ToList().Where(x => x.date < End && x.date > Start).Select(r => new
                          {
                              r.body,
                              date = r.date.ToSlashDate(),
                              r.idMessage,
                              r.idMessageType,
                              r.isRead,
                              r.senderName,
                              r.title,
                              r.recieverName,
                              r.Type,
                              r.idReceiverType,
                              r.idSenderType,
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

        [ActionName("ReciveList")]
        [HttpGet]
        public HttpResponseMessage ReciveList([FromUri]int Start, [FromUri]int End)
        {
            if (Start == 0 && End == 0)
            {
                End = 99999999;
            }

            var reciver = bl.PTSentMsg().Where(x => x.idReceiverType == 1 && x.isActiveRecive==true);

            schoolEntities db = new schoolEntities();

            var Result = (from r in reciver
                          join t in db.tbl_msgTypes
                          on r.idMessageType equals t.idMsgType

                          select new

                          {
                              r.body,
                              r.date,
                              r.idMessage,
                              r.idMessageType,
                              r.isRead,
                              r.title,
                              r.recieverName,
                              r.idReceiverType,
                              r.idSenderType,
                              r.senderName,
                              t.Type

                          }).ToList().Where(x => x.date < End && x.date > Start).Select(r => new
                          {
                              r.body,
                              date = r.date.ToSlashDate(),
                              r.idMessage,
                              r.idMessageType,
                              r.isRead,
                              r.title,
                              r.recieverName,
                              r.Type,
                              r.idReceiverType,
                              r.idSenderType,
                              r.senderName
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

        [ActionName("isRead")]
        [HttpGet]
        public bool isRead([FromUri] int idMsg)
        {
            var select = bl.Find(idMsg);
            //select.isRead = true;
            try
            {

                if (select == null)
                {
                    return false;
                }
                else
                {
                    if (bl.Update(select))
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


        [ActionName("Send")]
        [HttpPost]
        public int Send([FromBody] messageRecieversModel entity)
        {
            try
            {
                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idMessage = bl.GetLastIdentity() + 1;

                    if (entity.idMessageType == 2)//SMS
                    {
                        SendSMS sms = new SendSMS();
                        string body = entity.body;
                        entity.title = "پیامک";
                        string Numbers = bl.Numbers(entity.idRecieverType, entity.idRecievers);
                        return -1;//فعلا کار نمیکند بخاطر پیامک های خدماتی
                        //if (sms.groupSend(Numbers, body) !=1)
                        //{
                        //    return -1;
                        //}
                        //else
                        //{
                        //    return int.Parse(entity.idMessage.ToString());
                        //}

                    }
                    if (entity.idMessageType == 1)// System
                    {
                        bl.Add(entity);
                        return int.Parse(entity.idMessage.ToString());
                    }
                    else //Notife
                    {
                        SendPushNotification(entity.title, entity.body);
                        return int.Parse(entity.idMessage.ToString());
                    }

                }


            }
            catch (Exception EX)
            {
                return int.Parse("-2");
            }
        }


        [ActionName("deleteSend")]
        [HttpPost]
        public bool deleteSend([FromBody] int id)
        {
            try
            {

                if (id == null)
                {
                    return false;
                }
                else
                {
                    if (bl.deleteSend(id))
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

        [ActionName("deleteRecive")]
        [HttpPost]
        public bool deleteRecive([FromBody]int id)
        {
            try
            {

                if (id == null)
                {
                    return false;
                }
                else
                {
                    if (bl.deleteRecive(id))
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
        public static void SendPushNotification(string Title, string Body)
        {


            {
                    string applicationID = "AAAAxYkfdlI:APA91bH9BY4K05-Tpi8s-xcU-FY8Ht5VWFAPTDx0k-SX3bmXn-OqScEsoKaJhn0Crle7u_agNmgF-Vc_mS7GCiCf9vzyOUpFIU-a88XeLWQy52fiip44JTd_VCy0SgnHS2rA0IyxXmy6";

                string senderId = "848409097810";

                //string deviceId = "cCyZtmk5R_M:APA91bFAxRL-r0slodBN1zSYRYwCjPdtpJDagYs1e6Sx8ZSX5F99FrLjDBfgdlwZhCG2ZMGO4rDcfssoBhp1bOc4sJk3P2O3bM_ia7cD-OYpRUUX56El9Y2O9gd547Ye-DfzXZ-7h0zt";

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = "/topics/infogsharingir",
                    //to = "cCyZtmk5R_M:APA91bFAxRL-r0slodBN1zSYRYwCjPdtpJDagYs1e6Sx8ZSX5F99FrLjDBfgdlwZhCG2ZMGO4rDcfssoBhp1bOc4sJk3P2O3bM_ia7cD-OYpRUUX56El9Y2O9gd547Ye-DfzXZ-7h0zt",
                    //data = new
                    //{
                    //    message = "Hello",
                    //    url = "Its me",
                    //},
                    notification = new
                    {
                        title = Title,
                        body = Body,

                    },
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }

            //MessageController msg = new MessageController();
            //MessageRepository msgBL = new MessageRepository();
            //messageRecieversModel ob = new messageRecieversModel();
            //ob.body = Body;
            //int msgDate = 0;
            //int idStud = selectPayment.idStud;
            //ob.date = msgDate.GetPersianDate();
            //ob.idMessage = msgBL.GetLastIdentity() + 1;
            //ob.idMessageType = 1;
            //ob.idReciever = idStud;
            //ob.idRecieverType = 3;
            //ob.idSender = 1;
            //ob.idSenderType = 1;
            //ob.title = Title;
            //msg.Send(ob);
        }
    }
}