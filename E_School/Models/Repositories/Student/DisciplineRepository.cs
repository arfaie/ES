using E_School.Models.DomainModels;
using E_School.Models.ViewModel.api;
using E_School.Models.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories.api
{
    public class DisciplineRepository
    {
        private schoolEntities db = null;

        public DisciplineRepository()
        {
            db = new schoolEntities();
        }


        public List<disciplineModel> getClassDisciplines(int idClass)
        {
            List<View_discipline> ls = new List<View_discipline>();
            List<disciplineModel> list = new List<disciplineModel>();
            disciplineModel model;
            int today = getTodayDate();

            try
            {
                int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;
                ls = db.View_discipline.Where(x => x.idYear == idYear && x.idClass == idClass).OrderByDescending(x => x.actDate).ToList();

                for (int i = 0; i < ls.Count(); i++)
                {
                    model = new disciplineModel();
                    model.actDate = ls.ElementAt(i).actDate;
                    model.disciplineName = ls.ElementAt(i).disName;
                    model.studentName = ls.ElementAt(i).FName + " " + ls.ElementAt(i).LName;
                    model.des = ls.ElementAt(i).des;
                    list.Add(model);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }


        public List<disciplineModel> getDiscipTypes()
        {
            try
            {
                List<tbl_disciplineTypes> list = db.tbl_disciplineTypes.Where(x => x.idDisType > -2).ToList();
                disciplineModel model;
                List<disciplineModel> ls = new List<disciplineModel>();

                for (int i = 0; i < list.Count(); i++)
                {
                    model = new disciplineModel();
                    model.disciplineName = list.ElementAt(i).disName;
                    model.idDisType = list.ElementAt(i).idDisType;
                    ls.Add(model);
                }

                return ls;
            }

            catch
            {
                return null;
            }
        }

        public Boolean getDiscipline(getDiscipModel entity)
        {
            int idMessage, idMsgRecord, idDiscip, today = getTodayDate();
            TermRepository blTerm = new TermRepository();
            tbl_messages tb = new tbl_messages();
            tbl_messageRecord tblRecord;
            int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;
            int idTerm = blTerm.Where(x => x.termStart <= today && x.termEnd >= today).FirstOrDefault().idTerm;
            entity.idTerm = idTerm;
            if (db.tbl_studentsDisciplines.Any())
                idDiscip = db.tbl_studentsDisciplines.OrderByDescending(p => p.idStudentDisciplines).FirstOrDefault().idStudentDisciplines;
            else
                idDiscip = 0;

            tbl_studentsDisciplines tbl;

            try
            {
                tbl = new tbl_studentsDisciplines();
                tbl.idStudentDisciplines = idDiscip + 1;
                tbl.idStudent = entity.idStudent;
                tbl.idDisType = entity.idDisType;
                tbl.actDate = entity.actDate;

                if (!entity.des.Equals(""))
                    tbl.des = entity.des;

                tbl.justified = true;
                tbl.idTerm = entity.idTerm;
                db.tbl_studentsDisciplines.Add(tbl);
                db.SaveChanges();


                if (entity.idDisType == -1) // مورد انضباطی از نوع تاخیر است
                {

                    if (db.tbl_studentsDisciplines.Where(x => x.idStudent == entity.idStudent && x.actDate == entity.actDate && x.idDisType == -2 && x.justified == true).Any())
                    {
                        tbl_studentsDisciplines t = new tbl_studentsDisciplines();
                        t = db.tbl_studentsDisciplines.Where(x => x.idStudent == entity.idStudent && x.actDate == entity.actDate && x.idDisType == -2 && x.justified == true).FirstOrDefault();
                        t.justified = false;
                        db.SaveChanges();
                    }


                    // اگر برای این دانش آموز در این روز غیبت ثبت شده باشد، غیبت موجه شده و تاخیر ثبت می شود
                    if (db.tbl_absentation.Where(x => x.idDataTable == entity.idDataTable && x.idStudent == entity.idStudent && x.date == entity.actDate).Any())
                    {
                        tbl_absentation t = new tbl_absentation();
                        tbl_studentsDisciplines d = new tbl_studentsDisciplines();
                        t = db.tbl_absentation.Where(x => x.idDataTable == entity.idDataTable && x.idStudent == entity.idStudent && x.date == entity.actDate).FirstOrDefault();
                        d = db.tbl_studentsDisciplines.Where(x=>x.idStudent == entity.idStudent && x.actDate == entity.actDate).FirstOrDefault();
                        t.status = true;
                        d.justified = true;
                        db.SaveChanges();
                    }
                }

            }

            catch
            {
                return false;
            }






            try
            {
                //send message to student's parent

                if (db.tbl_messages.Any())
                    idMessage = db.tbl_messages.OrderByDescending(p => p.idMessage).FirstOrDefault().idMessage;
                else
                    idMessage = 0;

                tb.idMessage = idMessage + 1;
                tb.title = "گزارش ثبت مورد انضباطی";
                tb.body = db.tbl_disciplineTypes.Where(x => x.idDisType == entity.idDisType).FirstOrDefault().disName;
                tb.date = entity.actDate;
                tb.idMessageType = 1;
                tb.idReceiverType = 3;
                tb.idSenderType = 2;
                tb.idSender = entity.idTeacher;
                db.tbl_messages.Add(tb);
                db.SaveChanges();

                tblRecord = new tbl_messageRecord();

                if (db.tbl_messageRecord.Any())
                    idMsgRecord = db.tbl_messageRecord.OrderByDescending(p => p.idMsgRecord).FirstOrDefault().idMsgRecord;
                else
                    idMsgRecord = 0;

                tblRecord.idMsgRecord = idMsgRecord + 1;
                tblRecord.idMessage = idMessage + 1;
                tblRecord.idReceiverType = 3;
                tblRecord.idReceiver = entity.idStudent;
                tblRecord.isRead = false;
                tblRecord.isActiveRecive = true;
                tblRecord.isActiveSend = true;
                tblRecord.idYear = idYear;
                db.tbl_messageRecord.Add(tblRecord);
                db.SaveChanges();

                if (entity.idDisType == -1)
                {
                    sendSMS(entity.idStudent, entity.actDate);
                }


                return true;

            }

            catch
            {
                return false;
            }



        }


        private void sendSMS(int idStudent, int date)
        {
            SmsServise.Send objSend = new SmsServise.Send();
            byte[] StatusArray = null;
            long[] RecIdArray = null;
            string userName = "arfaie";
            string password = "Navidarfaie123456789";
            string virtualNumber = "30008561222088";



            String name, y, m, d, time, schoolName = "";


            string Recipient = db.tbl_students.Where(x => x.idStudent == idStudent).FirstOrDefault().fPhone;
            name = db.tbl_students.Where(x => x.idStudent == idStudent).FirstOrDefault().FName + " " + db.tbl_students.Where(x => x.idStudent == idStudent).FirstOrDefault().LName;
            y = date.ToString().Substring(0, 4);
            m = date.ToString().Substring(4, 2);
            d = date.ToString().Substring(6, 2);
            time = y + "/" + m + "/" + d;

            if (db.tbl_Setting.Count() > 0)
                schoolName = db.tbl_Setting.FirstOrDefault().schoolName;
            string MessageBody = "گزارش تاخیر" + Environment.NewLine + name + Environment.NewLine + time + Environment.NewLine + schoolName;
            int sendStatus;
            sendStatus = objSend.SendSms(userName, password, virtualNumber, Recipient.Split(new char[] { ',' }), MessageBody, false, ref StatusArray, ref RecIdArray);

            if (sendStatus == 0) // sms sent successful
            {
                tbl_Setting tbl = db.tbl_Setting.FirstOrDefault();
                tbl.smsCount--;
                db.SaveChanges();
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