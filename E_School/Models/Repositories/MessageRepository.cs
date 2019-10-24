using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;
using E_School.Models.ViewModel;
using E_School.Models.ViewModel.Management;
using E_School.Models.ViewModel.api;
using System.Data.Entity.Validation;
using static E_School.Helpers.Utitlies.Custom_Exception;

namespace E_School.Models.Repositories
{
    public class MessageRepository : IDisposable
    {
        private schoolEntities db = null;

        public MessageRepository()
        {
            db = new schoolEntities();
        }

        public Boolean Add(messageRecieversModel entity)
        {

            tbl_messages tbl = new tbl_messages();
            tbl_messageRecord tblRecord;
            int idYear = db.tbl_years.OrderByDescending(p => p.idYear).First().idYear;

            try
            {
                tbl.idMessage = entity.idMessage;
                tbl.title = entity.title;
                tbl.body = entity.body;
                tbl.date = entity.date;
                tbl.idMessageType = (short)entity.idMessageType;
                tbl.idReceiverType = entity.idRecieverType;
                tbl.idSenderType = entity.idSenderType;
                tbl.idSender = entity.idSender;
                db.tbl_messages.Add(tbl);
                db.SaveChanges();

                if (entity.idRecievers != null)
                {
                    for (int i = 0; i < entity.idRecievers.Count(); i++)
                    {
                        tblRecord = new tbl_messageRecord();
                        int idMsgRecord = GetLastIdentity_Record() + 1;
                        tblRecord.idMsgRecord = idMsgRecord + 1;
                        tblRecord.idMessage = entity.idMessage;
                        tblRecord.idReceiverType = entity.idRecieverType;
                        tblRecord.idReceiver = entity.idRecievers[i];
                        tblRecord.isRead = false;
                        tblRecord.isActiveRecive = true;
                        tblRecord.isActiveSend = true;
                        tblRecord.idYear = idYear;
                        db.tbl_messageRecord.Add(tblRecord);
                        db.SaveChanges();
                    }
                }
                else
                {
                    tblRecord = new tbl_messageRecord();
                    int idMsgRecord = GetLastIdentity_Record() + 1;
                    tblRecord.idMsgRecord = idMsgRecord;
                    tblRecord.idMessage = entity.idMessage;
                    tblRecord.idReceiverType = entity.idRecieverType;
                    tblRecord.idReceiver = entity.idReciever;
                    tblRecord.isRead = false;
                    tblRecord.isActiveRecive = true;
                    tblRecord.isActiveSend = true;
                    tblRecord.idYear = idYear;
                    db.tbl_messageRecord.Add(tblRecord);
                    db.SaveChanges();
                }

                //db.SaveChanges();
                return true;

            }

            catch (DbEntityValidationException e)
            {
                var newException = new FormattedDbEntityValidationException(e);
                return false;
            }

        }

        public string Numbers(int idRecieverType, int[] idRecievers)
        {
            string Numbers = "";

            for (int i = 1; i <= idRecievers.Count(); i++)
            {
                if (idRecieverType == 2)//Teacher
                {
                    TeacherRepository Teache = new TeacherRepository();
                    int k = idRecievers[i - 1];
                    var select = Teache.Where(x => x.idTeacher == k).Single();
                    Numbers += select.mobile;
                    if (idRecievers.Count() != i)
                    {
                        Numbers += ",";
                    }
                }

                if (idRecieverType == 3)//Parent
                {
                    StudentRepository Parent = new StudentRepository();
                    int k = idRecievers[i - 1];
                    var select = Parent.Where(x => x.idStudent == k).Single();
                    Numbers += select.fPhone;
                    if (idRecievers.Count() != i)
                    {
                        Numbers += ",";
                    }
                }

                if (idRecieverType == 4)//Student
                {
                    StudentRepository Student = new StudentRepository();
                    int k = idRecievers[i - 1];
                    var select = Student.Where(x => x.idStudent == k).Single();
                    Numbers += select.studentMobile;
                    if (idRecievers.Count() != i)
                    {
                        Numbers += ",";
                    }
                }

                if (idRecieverType == 5)//Offices
                {
                    OfficesRepository Offices = new OfficesRepository();
                    int k = idRecievers[i - 1];
                    var select = Offices.Where(x => x.idOffices == k).Single();
                    Numbers += select.mobile;
                    if (idRecievers.Count() != i)
                    {
                        Numbers += ",";
                    }
                }

            }
            return Numbers;
        }

        public bool Update(tbl_messageRecord entity, bool autoSave = true)
        {
            try
            {
                entity.isRead = true;
                db.tbl_messageRecord.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(tbl_messages entity, bool autoSave = true)
        {
            try
            {
                db.Entry(entity).State = EntityState.Deleted;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteSend(int id, bool autoSave = true)
        {
            try
            {
                var entity = Where(x => x.idMessage == id).First();
                entity.isActiveSend = false;
                db.tbl_messageRecord.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteRecive(int id, bool autoSave = true)
        {
            try
            {
                var entity = Where(x => x.idMessage == id).Single();
                entity.isActiveRecive= false;
                db.tbl_messageRecord.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public tbl_messageRecord Find(int id)
        {
            try
            {
                return db.tbl_messageRecord.Where(x => x.idMessage == id).Single();
            }
            catch
            {
                return null;
            }
        }


        public IQueryable<tbl_messageRecord> Where(System.Linq.Expressions.Expression<Func<tbl_messageRecord, bool>> predicate)
        {
            try
            {
                return db.tbl_messageRecord.Where(predicate);
            }
            catch
            {
                return null;
            }
        }


        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_messageRecord, TResult>> selector)
        {
            try
            {
                return db.tbl_messageRecord.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public int GetLastIdentity()
        {
            try
            {
                if (db.tbl_messages.Any())
                    return db.tbl_messages.OrderByDescending(p => p.idMessage).First().idMessage;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public int GetLastIdentity_Record()
        {
            try
            {
                if (db.tbl_messages.Any())
                    return db.tbl_messageRecord.OrderByDescending(p => p.idMsgRecord).First().idMsgRecord;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public int Save()
        {
            try
            {
                return db.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        

        public List<MessageModel> Messages()
        {
            MessageModel model;
            int idSenderType, idsender, idRecieverType, idReciver;
            string senderFName, senderLName, RecieverFName, RecieverLName;
            List<MessageModel> listModel = new List<MessageModel>();
            List<View_studentMessage> ls = new List<View_studentMessage>();
            //int idYear = db.tbl_years.OrderByDescending(p => p.idYear).First().idYear;


            try
            {
                //ls = db.View_studentMessage.Where(x => x.idReceiverType == idRecieverType && x.idReceiver == idReceiver && x.idYear == idYear && x.isActive == true).OrderByDescending(x => x.idMsgRecord).ToList();
                ls = db.View_studentMessage.Where(x =>/* x.idYear == idYear &&*/ x.isActiveRecive == true && x.idSenderType == 1).OrderByDescending(x => x.idMsgRecord).ToList();

                for (int i = 0; i < ls.Count; i++)
                {
                    model = new MessageModel();
                    idRecieverType = ls.ElementAt(i).idReceiverType;
                    idReciver = ls.ElementAt(i).idReceiver;
                    idSenderType = ls.ElementAt(i).idSenderType;
                    if (idSenderType == 2) // teacher sent a message
                    {
                        idsender = ls.ElementAt(i).idSender;
                        senderFName = db.tbl_teachers.Where(x => x.idTeacher == idsender).FirstOrDefault().FName;
                        senderLName = db.tbl_teachers.Where(x => x.idTeacher == idsender).FirstOrDefault().LName;
                        model.idSenderType = 2;
                        model.senderName = senderFName + " " + senderLName;
                    }

                    else if (idSenderType == 3) // parent sent a message
                    {
                        idsender = ls.ElementAt(i).idSender;
                        senderFName = db.tbl_students.Where(x => x.idStudent == idsender).FirstOrDefault().FName;
                        senderLName = db.tbl_students.Where(x => x.idStudent == idsender).FirstOrDefault().LName;
                        model.senderName = senderFName + " " + senderLName;
                        model.idSenderType = 3;
                    }

                    else if (idSenderType == 1)// manager sent a message
                    {
                        model.senderName = "مدیریت";
                        model.idSenderType = 1;
                    }
                    ///////////////////////////////////////////////////////////////////////////////////

                    if (idRecieverType == 2) // messages sent to teacher (teacher is reciever)
                    {
                        RecieverFName = db.tbl_teachers.Where(x => x.idTeacher == idReciver).FirstOrDefault().FName;
                        RecieverLName = db.tbl_teachers.Where(x => x.idTeacher == idReciver).FirstOrDefault().LName;
                        model.recieverName = RecieverFName + " " + RecieverLName;
                    }


                    else if (idRecieverType == 3) // messages sent to parents (parents is reciever)
                    {
                        RecieverFName = db.tbl_students.Where(x => x.idStudent == idReciver).FirstOrDefault().FName;
                        RecieverLName = db.tbl_students.Where(x => x.idStudent == idReciver).FirstOrDefault().LName;
                        model.recieverName = RecieverFName + " " + RecieverLName;
                    }
                    else if (idRecieverType == -1) // messages sent to parents (parents is reciever)
                    {
                        model.recieverName = "کل مدرسه";
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////


                    model.idMsgRecord = ls.ElementAt(i).idMsgRecord;
                    model.idMessage = ls.ElementAt(i).idMessage;
                    model.idMessageType = ls.ElementAt(i).idMessageType;
                    model.idSender = ls.ElementAt(i).idSender;
                    model.isRead = (Boolean)ls.ElementAt(i).isRead;
                    model.title = ls.ElementAt(i).title;
                    model.body = ls.ElementAt(i).body;
                    model.date = ls.ElementAt(i).date;
                    model.isActiveRecive = (bool)ls.ElementAt(i).isActiveRecive;
                    model.isActiveSend = (bool)ls.ElementAt(i).isActiveSend;

                    listModel.Add(model);
                }


                return listModel;
            }
            catch
            {
                return null;
            }
        }

        public List<MessageModel> PTSentMsg()
        {
            MessageModel model;
            int idRecieverType, idReceiver, idSenderType;
            string senderFName, senderLName;
            string RecieverFName, RecieverLName;
            List<MessageModel> listModel = new List<MessageModel>();
            List<View_studentMessage> ls = new List<View_studentMessage>();
            //int idYear = db.tbl_years.OrderByDescending(p => p.idYear).First().idYear;

            try
            {
                //ls = db.View_studentMessage.Where(x => x.idSenderType == idSenderType && x.idSender == idSender && x.idYear == idYear && x.isActive == true).OrderByDescending(x => x.idMsgRecord).ToList();
                ls = db.View_studentMessage.Where(x =>/* x.idYear == idYear &&*/ x.isActiveSend == true & x.idReceiverType == 1).OrderByDescending(x => x.idMsgRecord).ToList();
                for (int i = 0; i < ls.Count; i++)
                {
                    model = new MessageModel();

                    idRecieverType = ls.ElementAt(i).idReceiverType;
                    idSenderType = ls.ElementAt(i).idSenderType;
                    idReceiver = ls.ElementAt(i).idReceiver;
                    if (idRecieverType == 2) // messages sent to teacher (teacher is reciever)
                    {
                        RecieverFName = db.tbl_teachers.Where(x => x.idTeacher == idReceiver).FirstOrDefault().FName;
                        RecieverLName = db.tbl_teachers.Where(x => x.idTeacher == idReceiver).FirstOrDefault().LName;
                        model.recieverName = RecieverFName + " " + RecieverLName;
                    }


                    else if (idRecieverType == 3) // messages sent to parents (parents is reciever)
                    {
                        RecieverFName = db.tbl_students.Where(x => x.idStudent == idReceiver).FirstOrDefault().FName;
                        RecieverLName = db.tbl_students.Where(x => x.idStudent == idReceiver).FirstOrDefault().LName;
                        model.recieverName = RecieverFName + " " + RecieverLName;
                    }
                    else if (idRecieverType == -1) // messages sent to parents (parents is reciever)
                    {
                        model.recieverName = "کل مدرسه";
                    }

                    else  // messages sent to manager (manager is reciever)
                        model.recieverName = "مدیریت";

                    //-----------------------------------------------------------------------------------------------------
                    if (idSenderType == 2) // teacher sent a message
                    {
                        model.idSender = ls.ElementAt(i).idSender;
                        senderFName = db.tbl_teachers.Where(x => x.idTeacher == model.idSender).FirstOrDefault().FName;
                        senderLName = db.tbl_teachers.Where(x => x.idTeacher == model.idSender).FirstOrDefault().LName;
                        model.idSenderType = 2;
                        model.senderName = senderFName + " " + senderLName;
                    }

                    else if (idSenderType == 3) // parent sent a message
                    {
                        model.idSender = ls.ElementAt(i).idSender;
                        senderFName = db.tbl_students.Where(x => x.idStudent == model.idSender).FirstOrDefault().FName;
                        senderLName = db.tbl_students.Where(x => x.idStudent == model.idSender).FirstOrDefault().LName;
                        model.senderName = senderFName + " " + senderLName;
                        model.idSenderType = 3;
                    }

                    //-----------------------------------------------------------------------------------------------------

                    model.idMsgRecord = ls.ElementAt(i).idMsgRecord;
                    model.idMessage = ls.ElementAt(i).idMessage;
                    model.idMessageType = ls.ElementAt(i).idMessageType;
                    model.idReciever = ls.ElementAt(i).idSender;
                    model.isRead = (Boolean)ls.ElementAt(i).isRead;
                    model.title = ls.ElementAt(i).title;
                    model.body = ls.ElementAt(i).body;
                    model.date = ls.ElementAt(i).date;
                    model.idSenderType = ls.ElementAt(i).idSenderType;

                    model.idReceiverType = ls.ElementAt(i).idReceiverType;
                    model.isActiveRecive=(bool) ls.ElementAt(i).isActiveRecive;
                    model.isActiveSend = (bool)ls.ElementAt(i).isActiveSend;
                    listModel.Add(model);
                }
                return listModel;
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }
        ~MessageRepository()
        {
            Dispose(false);
        }
    }
}