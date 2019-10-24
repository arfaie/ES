using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;
using MvcInternetShop.Models.DomainModels;
using E_School.Models.ViewModel.api;
using System.Globalization;

namespace E_School.Models.Repositories.api
{
    public class MessagesRepository
    {

        private schoolEntities db = null;
        private int today;


        public MessagesRepository()
        {
            db = new schoolEntities();
        }



        public List<messageModel> Messages(int idReceiver, int idRecieverType)
        {

            messageModel model;
            int idSenderType, idsender;
            string senderFName, senderLName;
            List<messageModel> listModel = new List<messageModel>();
            List<View_studentMessage> ls = new List<View_studentMessage>();
            today = getTodayDate();
            int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;


            try
            {
                ls = db.View_studentMessage.Where(x=>x.idReceiverType == idRecieverType && x.idReceiver == idReceiver && x.idYear == idYear && x.isActiveRecive == true).Concat(db.View_studentMessage.Where(x => x.idMessageType == 3 && x.idYear == idYear && x.isActiveRecive == true)).OrderByDescending(x => x.idMsgRecord).ToList();


                for (int i = 0; i < ls.Count; i++)
                {
                    model = new messageModel();

                    idSenderType = ls.ElementAt(i).idSenderType;
                    if (idSenderType == 2) // teacher sent a message
                    {
                        idsender = ls.ElementAt(i).idSender;
                        senderFName = db.tbl_teachers.Where(x => x.idTeacher == idsender).FirstOrDefault().FName;
                        senderLName = db.tbl_teachers.Where(x => x.idTeacher == idsender).FirstOrDefault().LName;
                        model.senderName = senderFName + " " + senderLName;
                    }

                    else if (idSenderType == 3) // parent sent a message
                    {
                        idsender = ls.ElementAt(i).idSender;
                        senderFName = db.tbl_students.Where(x => x.idStudent == idsender).FirstOrDefault().FName;
                        senderLName = db.tbl_students.Where(x => x.idStudent == idsender).FirstOrDefault().LName;
                        model.senderName = senderFName + " " + senderLName;
                    }

                    else if (idSenderType == 1) // manager sent a message
                        model.senderName = "مدیریت";


                    model.idMsgRecord = ls.ElementAt(i).idMsgRecord;
                    model.idMessage = ls.ElementAt(i).idMessage;
                    model.idMessageType = ls.ElementAt(i).idMessageType;
                    model.idSender = ls.ElementAt(i).idSender;
                    model.isRead = (Boolean)ls.ElementAt(i).isRead;
                    model.title = ls.ElementAt(i).title;
                    model.body = ls.ElementAt(i).body;
                    model.date = ls.ElementAt(i).date;
                    listModel.Add(model);
                }


                return listModel;
            }
            catch
            {
                return null;
            }
        }


        public List<messageModel> PTSentMsg(int idSender, int idSenderType)
        {
            messageModel model;
            int idRecieverType, idReceiver;
            string RecieverFName, RecieverLName;
            List<messageModel> listModel = new List<messageModel>();
            List<View_studentMessage> ls = new List<View_studentMessage>();
            today = getTodayDate();
            int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;

            try
            {
                ls = db.View_studentMessage.Where(x => x.idSenderType == idSenderType && x.idSender == idSender && x.idYear == idYear && x.isActiveSend == true).OrderByDescending(x => x.idMsgRecord).ToList();

                for (int i = 0; i < ls.Count; i++)
                {
                    model = new messageModel();

                    idRecieverType = ls.ElementAt(i).idReceiverType;
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

                    else  // messages sent to manager (manager is reciever)
                        model.recieverName = "مدیریت";


                    model.idMsgRecord = ls.ElementAt(i).idMsgRecord;
                    model.idMessage = ls.ElementAt(i).idMessage;
                    model.idMessageType = ls.ElementAt(i).idMessageType;
                    model.idReciever = ls.ElementAt(i).idSender;
                    model.isRead = (Boolean)ls.ElementAt(i).isRead;
                    model.title = ls.ElementAt(i).title;
                    model.body = ls.ElementAt(i).body;
                    model.date = ls.ElementAt(i).date;
                    listModel.Add(model);
                }
                return listModel;
            }
            catch
            {
                return null;
            }
        }


        public List<classTeacherModel> getClassTeachers(int idClass)
        {
            classTeacherModel model;
            List<tbl_dataTable> ls = new List<tbl_dataTable>();
            List<classTeacherModel> teachersList = new List<classTeacherModel>();
            int idTeacher;
            List<int> list = new List<int>();
            Boolean check;


            try
            {
                today = getTodayDate();
                int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;
                ls = db.tbl_dataTable.Where(x => x.idClass == idClass && x.idYear == idYear).ToList();

                for (int i = 0; i < ls.Count(); i++)
                {

                    if (list.Count() == 0)
                    {
                        model = new classTeacherModel();
                        model.idTeacher = ls.ElementAt(i).idTeacher;
                        list.Add(ls.ElementAt(i).idTeacher);
                        idTeacher = ls.ElementAt(i).idTeacher;
                        model.teacherName = db.tbl_teachers.Where(x => x.idTeacher == idTeacher).First().FName + " " + db.tbl_teachers.Where(x => x.idTeacher == idTeacher).First().LName;
                        teachersList.Add(model);
                    }


                    else
                    {
                        check = false;
                        for (int j = 0; j < list.Count(); j++)
                        {
                            if (ls.ElementAt(i).idTeacher == list.ElementAt(j))
                            {
                                check = true;
                                break;
                            }

                        }

                        if (check == false)
                        {
                            model = new classTeacherModel();
                            model.idTeacher = ls.ElementAt(i).idTeacher;
                            list.Add(ls.ElementAt(i).idTeacher);
                            idTeacher = ls.ElementAt(i).idTeacher;
                            model.teacherName = db.tbl_teachers.Where(x => x.idTeacher == idTeacher).First().FName + " " + db.tbl_teachers.Where(x => x.idTeacher == idTeacher).First().LName;
                            teachersList.Add(model);
                        }
                    }


                }

                return teachersList;
            }

            catch
            {
                return null;
            }
        }


        public List<View_classStudents> getClassStudents(int idClass)
        {
            today = getTodayDate();
            int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;
            return db.View_classStudents.Where(x => x.idYear == idYear && x.idClass == idClass).OrderBy(x => x.LName).ToList();
        }

        public List<classTeacherModel> getTeacherClasses(int idTeacher)
        {
            classTeacherModel model = null;
            List<tbl_dataTable> ls = new List<tbl_dataTable>();
            List<classTeacherModel> classList = new List<classTeacherModel>();
            List<int> list = new List<int>();
            int idClass;
            Boolean check;

            try
            {
                today = getTodayDate();
                int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;
                ls = db.tbl_dataTable.Where(x => x.idTeacher == idTeacher && x.idYear == idYear).ToList();



                for (int i = 0; i < ls.Count(); i++)
                {

                    if (list.Count() == 0)
                    {
                        model = new classTeacherModel();
                        list.Add(ls.ElementAt(i).idClass);
                        model.idClass = ls.ElementAt(i).idClass;
                        idClass = ls.ElementAt(i).idClass;
                        model.className = db.tbl_classes.Where(x => x.idClass == idClass).First().className;
                        classList.Add(model);
                    }

                    else
                    {
                        check = false;
                        for (int j = 0; j < list.Count(); j++)
                        {
                            if (ls.ElementAt(i).idClass == list.ElementAt(j))
                            {
                                check = true;
                                break;
                            }

                        }

                        if (check == false)
                        {
                            model = new classTeacherModel();
                            list.Add(ls.ElementAt(i).idClass);
                            model.idClass = ls.ElementAt(i).idClass;
                            idClass = ls.ElementAt(i).idClass;
                            model.className = db.tbl_classes.Where(x => x.idClass == idClass).First().className;
                            classList.Add(model);
                        }
                    }




                }

                return classList;
            }

            catch
            {
                return null;
            }
        }


        public Boolean setMSgRead(Boolean isRead, int idMessageRecord)
        {


            try
            {
                tbl_messageRecord tbl = db.tbl_messageRecord.Where(x => x.idMsgRecord == idMessageRecord).FirstOrDefault();
                tbl.isRead = true;
                db.SaveChanges();


                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean recieveParentMessage(messageRecieversModel entity)
        {
            int idMessage = GetLastIdentity() + 1;
            tbl_messages tbl = new tbl_messages();
            tbl_messageRecord tblRecord;
            today = getTodayDate();
            int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;

            try
            {
                tbl.idMessage = idMessage;
                tbl.title = entity.title;
                tbl.body = entity.body;
                tbl.date = entity.date;
                tbl.idMessageType = (short)entity.idMessageType;
                tbl.idReceiverType = entity.idRecieverType;
                tbl.idSenderType = entity.idSenderType;
                tbl.idSender = entity.idSender;
                db.tbl_messages.Add(tbl);
                db.SaveChanges();

                for (int i = 0; i < entity.idRecievers.Count(); i++)
                {
                    tblRecord = new tbl_messageRecord();
                    int idMsgRecord = GetLastIdentity_Record() + 1;
                    tblRecord.idMsgRecord = idMsgRecord;
                    tblRecord.idMessage = idMessage;
                    tblRecord.idReceiverType = entity.idRecieverType;
                    tblRecord.idReceiver = entity.idRecievers[i];
                    tblRecord.isRead = false;
                    tblRecord.isActiveRecive = true;
                    tblRecord.isActiveSend = true;
                    tblRecord.idYear = idYear;
                    db.tbl_messageRecord.Add(tblRecord);
                    db.SaveChanges();
                }

                return true;

            }

            catch
            {
                return false;
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
        public Boolean deletSendeMsg(int idMsg)
        {

            try
            {

                tbl_messageRecord tbl = db.tbl_messageRecord.Where(x => x.idMsgRecord == idMsg).FirstOrDefault();
                tbl.isActiveSend = false;
                db.SaveChanges();

                return true;

            }

            catch
            {
                return false;
            }

        }

        public Boolean deleteRciveMsg(int idMsg)
        {

            try
            {

                tbl_messageRecord tbl = db.tbl_messageRecord.Where(x => x.idMsgRecord == idMsg).FirstOrDefault();
                tbl.isActiveRecive = false;
                db.SaveChanges();

                return true;

            }

            catch
            {
                return false;
            }

        }

        public int unreadMsgCount(int idReceiverType, int idReceiver)
        {
            try
            {
                int count = db.tbl_messageRecord.Where(x => x.idReceiverType == idReceiverType && x.idReceiver == idReceiver && x.isActiveRecive == true && x.isRead == false).Count();
                return count;

            }

            catch
            {
                return 0;
            }
        }


        private int getTodayDate()
        {
            DateTime d = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string y = pc.GetYear(d).ToString();
            string m = pc.GetMonth(d).ToString();
            if (m.Count() == 1)
                m = "0" + m;
            string day = pc.GetDayOfMonth(d).ToString();
            if (day.Count() == 1)
                day = "0" + day;
            int date = int.Parse(y + m + day);

            return date;
        }
    }
}