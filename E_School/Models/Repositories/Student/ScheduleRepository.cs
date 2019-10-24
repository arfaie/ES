using E_School.Helpers.Utitlies;
using E_School.Models.DomainModels;
using E_School.Models.ViewModel.api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;

namespace E_School.Models.Repositories.api
{
    public class ScheduleRepository
    {
        private schoolEntities db = null;

        public ScheduleRepository()
        {
            db = new schoolEntities();
        }

        public List<scheduleModel> StudentSchedule(int idClass)
        {
            List<scheduleModel> list = new List<scheduleModel>();
            List<tbl_dataTable> ls;
            scheduleModel model;
            int idTeacher, idLesson;

            try
            {
                int date = getTodayDate();

                int idYear = db.tbl_years.Where(x => x.yearStart <= date && x.yearEnd >= date).FirstOrDefault().idYear;

                if (db.tbl_dataTable.Where(x => x.idYear == idYear && x.idClass == idClass).Any())
                {
                    ls = db.tbl_dataTable.Where(x => x.idYear == idYear && x.idClass == idClass).ToList();
                    for (int i = 0; i < ls.Count(); i++)
                    {
                        model = new scheduleModel();
                        model.idDataTable = ls.ElementAt(i).idDataTable;
                        model.idTeacher = ls.ElementAt(i).idTeacher;
                        model.idLesson = ls.ElementAt(i).idLesson;
                        model.idDay = ls.ElementAt(i).idDay;
                        model.idBell = ls.ElementAt(i).idBell;
                        idTeacher = ls.ElementAt(i).idTeacher;
                        model.teacherName = db.tbl_teachers.Where(x => x.idTeacher == idTeacher).FirstOrDefault().FName + " " + db.tbl_teachers.Where(x => x.idTeacher == idTeacher).FirstOrDefault().LName;
                        idLesson = ls.ElementAt(i).idLesson;
                        model.lessonName = db.tbl_lessons.Where(x => x.idLesson == idLesson).FirstOrDefault().lessonName;

                        if (ls.ElementAt(i).isTak == true)
                        {
                            model.isTak = true;
                            model.idTeacher2 = (int)ls.ElementAt(i).idTeacher2;
                            model.idLesson2 = (int)ls.ElementAt(i).idLesson2;
                            idTeacher = (int)ls.ElementAt(i).idTeacher2;
                            idLesson = (int)ls.ElementAt(i).idLesson2;
                            model.teacherName2 = db.tbl_teachers.Where(x => x.idTeacher == idTeacher).FirstOrDefault().FName + " " + db.tbl_teachers.Where(x => x.idTeacher == idTeacher).FirstOrDefault().LName;
                            model.lessonName2 = db.tbl_lessons.Where(x => x.idLesson == idLesson).FirstOrDefault().lessonName;
                        }

                        list.Add(model);
                    }

                    return list;
                }

                else
                    return list;


            }
            catch
            {
                return null;
            }
        }


        public List<scheduleModel> teacherSchedule(int idTeacher)
        {
            List<scheduleModel> list = new List<scheduleModel>();
            List<View_teacherSchedule> ls, lls;
            scheduleModel model;
            int idLesson, idClass;
            bool isTak = false;

            try
            {

                int date = getTodayDate();

                int idYear = db.tbl_years.Where(x => x.yearStart <= date && x.yearEnd >= date).FirstOrDefault().idYear;

                if (db.View_teacherSchedule.Where(x => x.idTeacher == idTeacher || x.idTeacher2 == idTeacher && x.idYear == idYear).Any())
                {
                    ls = db.View_teacherSchedule.Where(x => x.idTeacher == idTeacher && x.idYear == idYear).ToList();
                    lls = db.View_teacherSchedule.Where(x => x.idTeacher2 == idTeacher && x.idYear == idYear).ToList();



                    for (int i = 0; i < ls.Count(); i++)
                    {
                        isTak = false;


                        for (int j = 0; j < list.Count(); j++)
                        {
                            if (list.ElementAt(j).idBell == ls.ElementAt(i).idBell && list.ElementAt(j).idDay == ls.ElementAt(i).idDay)
                            {
                                list.ElementAt(j).isTak = true;
                                idClass = ls.ElementAt(i).idClass;
                                idLesson = ls.ElementAt(i).idLesson;
                                list.ElementAt(j).idLesson2 = idLesson;
                                list.ElementAt(j).idClass2 = idClass;
                                list.ElementAt(j).lessonName2 = db.tbl_lessons.Where(x => x.idLesson == idLesson).FirstOrDefault().lessonName;
                                list.ElementAt(j).className2 = db.tbl_classes.Where(x => x.idClass == idClass).FirstOrDefault().className;
                                isTak = true;
                                break;
                            }
                        }


                        if (!isTak)
                        {
                            model = new scheduleModel();
                            model.idDataTable = ls.ElementAt(i).idDataTable;
                            model.idTeacher = ls.ElementAt(i).idTeacher;
                            model.idLesson = ls.ElementAt(i).idLesson;
                            model.idDay = ls.ElementAt(i).idDay;
                            model.idBell = ls.ElementAt(i).idBell;
                            model.idClass = ls.ElementAt(i).idClass;
                            idClass = ls.ElementAt(i).idClass;
                            model.className = db.tbl_classes.Where(x => x.idClass == idClass).FirstOrDefault().className;
                            idLesson = ls.ElementAt(i).idLesson;
                            model.lessonName = db.tbl_lessons.Where(x => x.idLesson == idLesson).FirstOrDefault().lessonName;


                            list.Add(model);
                        }


                    }




                    for (int k = 0; k < lls.Count(); k++)
                    {
                        isTak = false;


                        for (int j = 0; j < list.Count(); j++)
                        {
                            if (list.ElementAt(j).idBell == lls.ElementAt(k).idBell && list.ElementAt(j).idDay == lls.ElementAt(k).idDay)
                            {
                                list.ElementAt(j).isTak = true;
                                idClass = lls.ElementAt(k).idClass;
                                idLesson = (int)lls.ElementAt(k).idLesson2;
                                list.ElementAt(j).idLesson2 = idLesson;
                                list.ElementAt(j).idClass2 = idClass;
                                list.ElementAt(j).lessonName2 = db.tbl_lessons.Where(x => x.idLesson == idLesson).FirstOrDefault().lessonName;
                                list.ElementAt(j).className2 = db.tbl_classes.Where(x => x.idClass == idClass).FirstOrDefault().className;
                                isTak = true;
                                break;
                            }
                        }


                        if (!isTak)
                        {
                            model = new scheduleModel();
                            model.idDataTable = lls.ElementAt(k).idDataTable;
                            model.idTeacher = (int)lls.ElementAt(k).idTeacher2;
                            model.idLesson = (int)lls.ElementAt(k).idLesson2;
                            model.idDay = lls.ElementAt(k).idDay;
                            model.idBell = lls.ElementAt(k).idBell;
                            model.idClass = lls.ElementAt(k).idClass;
                            idClass = lls.ElementAt(k).idClass;
                            model.className = db.tbl_classes.Where(x => x.idClass == idClass).FirstOrDefault().className;
                            idLesson = (int)lls.ElementAt(k).idLesson2;
                            model.lessonName = db.tbl_lessons.Where(x => x.idLesson == idLesson).FirstOrDefault().lessonName;


                            list.Add(model);
                        }


                    }

                    return list;
                }

                else
                    return list;
            }


            catch
            {
                return null;
            }
        }



        public Boolean absentation(absentModel entity)
        {

            tbl_absentation tbl;
            String sms = "";
            int idPresent, idTerm, today;


            try
            {
                for (int i = 0; i < entity.idAbsents.Count(); i++)
                {
                    tbl = new tbl_absentation();

                    if (db.tbl_absentation.Any())
                        tbl.idAbsentation = db.tbl_absentation.OrderByDescending(p => p.idAbsentation).FirstOrDefault().idAbsentation + 1;

                    else
                        tbl.idAbsentation = 1;

                    tbl.idStudent = entity.idAbsents.ElementAt(i);
                    tbl.status = true;
                    tbl.date = entity.date;
                    tbl.idDataTable = entity.idDataTable;
                    db.tbl_absentation.Add(tbl);
                    db.SaveChanges();


                }



                for (int i = 0; i < entity.idPresents.Count(); i++)
                {
                    tbl = new tbl_absentation();

                    if (db.tbl_absentation.Any())
                        tbl.idAbsentation = db.tbl_absentation.OrderByDescending(p => p.idAbsentation).FirstOrDefault().idAbsentation + 1;

                    else
                        tbl.idAbsentation = 1;

                    tbl.idStudent = entity.idPresents.ElementAt(i);
                    idPresent = entity.idPresents.ElementAt(i);
                    if (!sms.Equals(""))
                    {
                        sms += ",";
                        sms += db.tbl_students.Where(x => x.idStudent == idPresent).FirstOrDefault().fPhone;
                    }
                    else
                        sms += db.tbl_students.Where(x => x.idStudent == idPresent).FirstOrDefault().fPhone;

                    tbl.status = false;
                    tbl.date = entity.date;
                    tbl.idDataTable = entity.idDataTable;
                    db.tbl_absentation.Add(tbl);
                    db.SaveChanges();



                    //save presents in disciplines

                    tbl_studentsDisciplines tb = new tbl_studentsDisciplines();


                    if (db.tbl_studentsDisciplines.Any())
                        tb.idStudentDisciplines = db.tbl_studentsDisciplines.OrderByDescending(p => p.idStudentDisciplines).FirstOrDefault().idStudentDisciplines + 1;

                    else
                        tb.idStudentDisciplines = 1;

                    tb.idDisType = -2;
                    tb.idStudent = entity.idPresents.ElementAt(i);
                    tb.actDate = entity.date;
                    tb.justified = true;


                    today = getTodayDate();
                    idTerm = db.tbl_terms.Where(x => x.termStart <= today && x.termEnd >= today).FirstOrDefault().idTerm;

                    tb.idTerm = idTerm;
                    db.tbl_studentsDisciplines.Add(tb);
                    db.SaveChanges();


                }

                if (!sms.Equals(""))
                    sendSMS(entity.idPresents, entity.date, entity.idDataTable);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void sendSMS(int[] idPresents, int date, int idDataTable)
        {
            SendSMS ob = new SendSMS();
            SettingRepository settingBL = new SettingRepository();
            string schoolName = settingBL.Select().Single().schoolName.Replace("مدرسه", ""); ;

            MelliPayamak.Send sms = new MelliPayamak.Send();
            string retval = null;

            int id, idBell;
            String name, y, m, d, time, Date, bell;

            idBell = db.tbl_dataTable.Where(x => x.idDataTable == idDataTable).FirstOrDefault().idBell;
            bell = db.tbl_bells.Where(x => x.idBells == idBell).FirstOrDefault().BellName;

            int Numbercount = 0;
            for (int i = 0; i < idPresents.Count(); i++)
            {
                id = idPresents.ElementAt(i);
                string Recipient = db.tbl_students.Where(x => x.idStudent == id).FirstOrDefault().fPhone;
                name = db.tbl_students.Where(x => x.idStudent == id).FirstOrDefault().FName + " " + db.tbl_students.Where(x => x.idStudent == id).FirstOrDefault().LName;
                y = date.ToString().Substring(0, 4);
                m = date.ToString().Substring(4, 2);
                d = date.ToString().Substring(6, 2);
                Date = y + "/" + m + "/" + d;
                time = DateTime.Now.ToString("HH:mm");

                string text = schoolName + ";" + name + ";" + Date + ";" + time + ";" + bell + Environment.NewLine;
                retval = sms.SendByBaseNumber2("09144942959", "2478", text, Recipient, 3392);
                long sendStatus = long.Parse(retval);


                if (long.Parse(retval) > 100)
                {
                    Numbercount++;
                }
                //if (sendStatus == 0) // sms sent successful
                //{
                //    tbl_Setting tbl = db.tbl_Setting.FirstOrDefault();
                //    tbl.smsCount--;
                //    db.SaveChanges();
                //}
            }
            ob.updateSMSCount(2, true, Numbercount);

        }


        public Boolean editAbsentation(absentModel entity)
        {

            int idAbsent, idPresents, idStudent, presentCounter = 0;
            bool status, sms = false, isThereAnyLateCommer = false;
            int[] idStudents = new int[entity.idPresents.Count()];
            List<int> idLateCommers = new List<int>();

            try
            {
                for (int i = 0; i < entity.idAbsents.Count(); i++) //ثبت حاضرین
                {
                    idAbsent = entity.idAbsents.ElementAt(i);//آی دی جدول Absentation

                    if (db.tbl_absentation.Where(x => x.idAbsentation == idAbsent && x.status == false).Any())//بررسی میکند که آیا غائبی هست
                    {
                        tbl_absentation abs = db.tbl_absentation.Where(x => x.idAbsentation == idAbsent && x.status == false).FirstOrDefault();
                        idLateCommers.Add(abs.idStudent);
                        isThereAnyLateCommer = true;
                        tbl_studentsDisciplines des = new tbl_studentsDisciplines();
                        var select = db.tbl_studentsDisciplines.Where(x => x.idStudent == abs.idStudent).FirstOrDefault();
                        db.Entry(select).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    tbl_absentation tbl = db.tbl_absentation.Where(x => x.idAbsentation == idAbsent).FirstOrDefault();
                    tbl.status = true;
                    db.SaveChanges();
                }

                if (isThereAnyLateCommer)
                    //sendLateCommersSMS(idLateCommers, entity.date, entity.idDataTable);




                for (int i = 0; i < entity.idPresents.Count(); i++) // ثبت غائبین
                {
                    idPresents = entity.idPresents.ElementAt(i);
                    tbl_absentation tbl = db.tbl_absentation.Where(x => x.idAbsentation == idPresents).FirstOrDefault();
                    status = tbl.status;
                    tbl.status = false;
                    idStudent = tbl.idStudent;
                    //save decipline
                    tbl_studentsDisciplines tb = new tbl_studentsDisciplines();


                    if (db.tbl_studentsDisciplines.Any())
                        tb.idStudentDisciplines = db.tbl_studentsDisciplines.OrderByDescending(p => p.idStudentDisciplines).FirstOrDefault().idStudentDisciplines + 1;

                    else
                        tb.idStudentDisciplines = 1;

                    tb.idDisType = -2;
                    tb.idStudent = entity.idPresents.ElementAt(i);
                    tb.actDate = entity.date;
                    tb.justified = true;


                    int today = getTodayDate();
                    int idTerm = db.tbl_terms.Where(x => x.termStart <= today && x.termEnd >= today).FirstOrDefault().idTerm;

                    tb.idTerm = idTerm;
                    db.tbl_studentsDisciplines.Add(tb);
                    ///////////////
                    db.SaveChanges();

                    if (status == true)
                    {
                        sms = true;
                        idStudents[presentCounter] = idStudent;
                        presentCounter++;
                    }
                }

                if (sms)
                    sendSMS(idStudents, entity.date, entity.idDataTable);


                return true;
            }
            catch
            {
                return false;
            }
        }


        private void sendLateCommersSMS(List<int> idLateCommers, int date, int idDataTable)
        {
            SendSMS ob = new SendSMS();
            SettingRepository settingBL = new SettingRepository();
            string schoolName = settingBL.Select().Single().schoolName.Replace("مدرسه", ""); ;

            MelliPayamak.Send sms = new MelliPayamak.Send();
            string retval = null;

            //SmsServise.Send objSend = new SmsServise.Send();
            //byte[] StatusArray = null;
            //long[] RecIdArray = null;
            //string userName = "arfaie";
            //string password = "Navidarfaie123456789";
            ////string virtualNumber = "30008561661151";
            //string virtualNumber = "30008561222088";



            int id, idBell;
            String name, y, m, d, time, Date, bell;

            idBell = db.tbl_dataTable.Where(x => x.idDataTable == idDataTable).FirstOrDefault().idBell;
            bell = db.tbl_bells.Where(x => x.idBells == idBell).FirstOrDefault().BellName;

            int Numbercount = 0;
            for (int i = 0; i < idLateCommers.Count(); i++)
            {
                id = idLateCommers.ElementAt(i);
                string Recipient = db.tbl_students.Where(x => x.idStudent == id).FirstOrDefault().fPhone;
                name = db.tbl_students.Where(x => x.idStudent == id).FirstOrDefault().FName + " " + db.tbl_students.Where(x => x.idStudent == id).FirstOrDefault().LName;
                y = date.ToString().Substring(0, 4);
                m = date.ToString().Substring(4, 2);
                d = date.ToString().Substring(6, 2);
                Date = y + "/" + m + "/" + d;
                time = DateTime.Now.ToString("HH:mm");

                string text = schoolName + ";" + name + ";" + Date + ";" + time + ";" + bell + Environment.NewLine;
                retval = sms.SendByBaseNumber2("09144942959", "2478", text, Recipient, 3403);
                long sendStatus = long.Parse(retval);

                //string MessageBody = "گزارش تاخیر" + Environment.NewLine + name + Environment.NewLine + time + Environment.NewLine + bell;
                //sendStatus = objSend.SendSms(userName, password, virtualNumber, Recipient.Split(new char[] { ',' }), MessageBody, false, ref StatusArray, ref RecIdArray);

                if (sendStatus == 0) // sms sent successful
                {
                    tbl_Setting tbl = db.tbl_Setting.FirstOrDefault();
                    tbl.smsCount--;
                    db.SaveChanges();
                }
            }
            ob.updateSMSCount(2, true, Numbercount);


        }


        public HttpResponseMessage absHistory(int idClass, int idLesson)
        {
            schoolEntities db = new schoolEntities();

            int date = getTodayDate();

            int idYear = db.tbl_years.Where(x => x.yearStart <= date && x.yearEnd >= date).FirstOrDefault().idYear;

            try
            {
                var Result = (from a in db.tbl_absentation

                              join s in db.tbl_students
                              on a.idStudent equals s.idStudent
                              //where a.status == false

                              join dt in db.tbl_dataTable
                              on a.idDataTable equals dt.idDataTable
                              where dt.idClass == idClass & dt.idLesson == idLesson & dt.idYear == idYear

                              join t in db.tbl_teachers
                              on dt.idTeacher equals t.idTeacher

                              join l in db.tbl_lessons
                              on dt.idLesson equals l.idLesson

                              join d in db.tbl_days
                              on dt.idDay equals d.idDay

                              join b in db.tbl_bells
                              on dt.idBell equals b.idBells

                              select new
                              {
                                  a.idAbsentation,
                                  a.date,
                                  a.status,
                                  a.idStudent,
                                  a.idDataTable,
                                  s.FName,
                                  s.LName,
                                  dt.bellStart,
                                  dt.bellEnd,
                                  dt.idClass,
                                  dt.idLesson,
                                  dt.idYear,
                                  dt.isTak,
                                  TeacherFName = t.FName,
                                  TeacherLName = t.LName,
                                  l.lessonName,
                                  d.dayName,
                                  b.BellName
                              }).ToList().Select(a => new
                              {
                                  a.idAbsentation,
                                  a.idDataTable,
                                  date = a.date.ToSlashDate(),
                                  a.status,
                                  a.idStudent,
                                  a.FName,
                                  a.LName,
                                  End = a.bellEnd.Hours + ":" + a.bellEnd.Minutes,
                                  Start = a.bellStart.Hours + ":" + a.bellStart.Minutes,
                                  a.idClass,
                                  a.idLesson,
                                  a.idYear,
                                  a.isTak,
                                  a.TeacherFName,
                                  a.TeacherLName,
                                  a.lessonName,
                                  a.dayName,
                                  a.BellName
                              })
                                 .ToList();

                int k = 0;
                LsAbs model;
                List<LsAbs> listAbs = new List<LsAbs>();
                foreach (var c in Result)
                {
                    var a = listAbs.Where(x => x.Date == c.date).FirstOrDefault();
                    var b = listAbs.Where(x => x.idDt == c.idDataTable).FirstOrDefault();

                    if (a == null || b == null)
                    {
                        k++;
                        model = new LsAbs();
                        model.Date = c.date;
                        model.idDt = c.idDataTable;
                        model.Session = k;
                        listAbs.Add(model);
                    }
                }
                string Sessions = "";
                string Dates = "";
                string myJson = "";
                int idStud = 0;//برای مواقعی است که دانش آموز اصلا غیبتی ندارد  و در لیست ادد نمیکند و باغث تکرار نامش میشود
                int flag = 0;
                List<int> LsID = new List<int>();
                List<string> LsDate = new List<string>();
                List<string> LsSessionDate = new List<string>();
                List<int> LsIdDT = new List<int>();
                foreach (var b in Result)
                {
                    Sessions = "";
                    if (!LsID.Contains(b.idStudent))
                    {
                        LsID.Add(b.idStudent);
                        LsDate.Clear();
                        //var s = new
                        //{
                        //    idStud = b.idStudent,
                        //    Name = b.FName + " " + b.LName,
                        //    Sessions = (from lA in listAbs
                        //                from res in Result
                        //                where res.idStudent == b.idStudent && !res.status
                        //                where lA.Date == res.date && res.idDataTable == lA.idDt
                        //                select lA
                        //                )
                        //};
                        myJson += "{\"idStud\":";
                        myJson += b.idStudent + ",";
                        myJson += "\"Name\":";
                        myJson += "\"" + b.FName + " " + b.LName + "\",";
                        myJson += "\"Sessions\":[";
                        foreach (var a in Result.Where(x => x.idStudent == b.idStudent && x.status == false))
                        {
                            if (!LsDate.Contains(a.date) || a.idDataTable != b.idDataTable)
                            {
                                var select = listAbs.Where(x => x.Date == a.date && x.idDt == a.idDataTable).Single();
                                Sessions += select.Session.ToString();
                                LsDate.Add(a.date);
                                Sessions += ",";
                                idStud = b.idStudent;
                            }
                            //LsID.Add(a.idStudent);
                        }

                        myJson += Sessions + "],";
                        myJson += "\"Dates\":[";
                        if (flag == 0)//Dates array fill once and others is like first
                        {
                            foreach (var a in Result)
                            {
                                flag = 1;
                                string Mydate = "\"" + a.date.Substring(5, 5).ToString() + "\"";
                                //if (!Dates.Contains(Mydate))
                                if (!LsSessionDate.Contains(a.date) && !LsIdDT.Contains(a.idDataTable))
                                {
                                    LsSessionDate.Add(a.date);
                                    LsIdDT.Add(a.idDataTable);
                                    Dates += Mydate;
                                    if (!object.ReferenceEquals(a, Result.Last()))
                                    {
                                        Dates += ",";
                                    }
                                }


                            }
                        }
                        myJson += Dates + "]}";
                        if (!object.ReferenceEquals(b, Result.Last()))
                        {
                            myJson += ",";
                        }
                    }
                }

                myJson = "[" + myJson + "]";
                myJson = myJson.Replace(",]", "]");
                myJson = myJson.Replace("\"Sessions\":[]", "\"Sessions\":[-2]");
                // var aa =

                //new JProperty("", from r in Result//all
                //                                  //group r by r3.idRegcourse into r3
                //                  select new JObject(
                //       new JProperty("Date", r.date),
                //       new JProperty("idStudent",
                //           new JArray(from r2 in Result//student
                //                                       //where r2.idDataTable == 1
                //                      select (r2.idStudent))))).ToString();
                // aa = aa.Substring(4);
                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(Result);
                return new HttpResponseMessage()
                {
                    Content = new StringContent(myJson)
                };
            }

            catch
            {
                return null;
            }
        }


        public List<View_absentation> getDayAbsentation(int date, int idClass, int idDataTable)
        {
            return db.View_absentation.Where(x => x.date == date && x.idClass == idClass && x.idDataTable == idDataTable).ToList();
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