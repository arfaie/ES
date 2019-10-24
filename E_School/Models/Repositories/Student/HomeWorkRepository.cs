using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_School.Models.DomainModels;
using E_School.Models.ViewModel.api;
using System.Globalization;

namespace E_School.Models.Repositories.api
{
    public class HomeWorkRepository
    {
        private schoolEntities db = null;

        public HomeWorkRepository()
        {
            db = new schoolEntities();
        }

        public List<homeworkModel> getTeacherHomeWorks(int idTeacher)
        {
            int idLesson, idYear;


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


            idYear = db.tbl_years.Where(x => x.yearStart <= date && x.yearEnd > date).FirstOrDefault().idYear;
            List<View_homeWork> ls = db.View_homeWork.Where(x => x.idYear == idYear && x.idTeacher == idTeacher && x.exDateTime < date).OrderByDescending(x => x.idHomeWork).ToList();
            List<homeworkModel> list = new List<homeworkModel>();
            homeworkModel model;

            try
            {
                for (int i = 0; i < ls.Count(); i++)
                {
                    model = new homeworkModel();
                    model.answerBody = ls.ElementAt(i).answerBody;
                    model.idHomeWork = ls.ElementAt(i).idHomeWork;
                    model.exDateTime = ls.ElementAt(i).exDateTime;
                    model.UploadDate = ls.ElementAt(i).UploadDate;
                    model.idClass = ls.ElementAt(i).idClass;
                    model.idTeacher = ls.ElementAt(i).idTeacher;
                    model.title = ls.ElementAt(i).title;
                    model.body = ls.ElementAt(i).body;
                    model.HomeWorkFile = ls.ElementAt(i).HomeWorkFile;
                    model.answerBody = ls.ElementAt(i).answerBody;
                    model.answerFile = ls.ElementAt(i).answerFile;
                    model.className = ls.ElementAt(i).className;
                    model.idLesson = ls.ElementAt(i).idLesson;
                    idLesson = ls.ElementAt(i).idLesson;
                    model.lessonName = db.tbl_lessons.Where(x => x.idLesson == idLesson).First().lessonName;
                    list.Add(model);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public List<homeworkModel> teacherRemainHomework(int idTeacher)
        {
            int idLesson, idYear;


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


            idYear = db.tbl_years.Where(x => x.yearStart <= date && x.yearEnd > date).FirstOrDefault().idYear;
            List<View_homeWork> ls = db.View_homeWork.Where(x => x.idYear == idYear && x.idTeacher == idTeacher && x.exDateTime >= date).OrderByDescending(x => x.idHomeWork).ToList();
            List<homeworkModel> list = new List<homeworkModel>();
            homeworkModel model;

            try
            {
                for (int i = 0; i < ls.Count(); i++)
                {
                    model = new homeworkModel();
                    model.answerBody = ls.ElementAt(i).answerBody;
                    model.idHomeWork = ls.ElementAt(i).idHomeWork;
                    model.exDateTime = ls.ElementAt(i).exDateTime;
                    model.UploadDate = ls.ElementAt(i).UploadDate;
                    model.idClass = ls.ElementAt(i).idClass;
                    model.idTeacher = ls.ElementAt(i).idTeacher;
                    model.title = ls.ElementAt(i).title;
                    model.body = ls.ElementAt(i).body;
                    model.HomeWorkFile = ls.ElementAt(i).HomeWorkFile;
                    model.answerBody = ls.ElementAt(i).answerBody;
                    model.answerFile = ls.ElementAt(i).answerFile;
                    model.className = ls.ElementAt(i).className;
                    model.idLesson = ls.ElementAt(i).idLesson;
                    idLesson = ls.ElementAt(i).idLesson;
                    model.lessonName = db.tbl_lessons.Where(x => x.idLesson == idLesson).First().lessonName;
                    list.Add(model);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public List<View_HomeWorkResponse> homeWorkResponse(int idHomeWork)
        {
            try
            {
                return db.View_HomeWorkResponse.Where(x => x.idHomeWork == idHomeWork).OrderByDescending(x => x.idRecHomeWork).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<homeworkModel> studentHomeWork(int idStudent)
        {
            List<tbl_homeWorks> list = new List<tbl_homeWorks>();
            List<homeworkModel> ls = new List<homeworkModel>();
            homeworkModel model;
            int idLesson, idYear, idClass;


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


            idYear = db.tbl_years.Where(x => x.yearStart <= date && x.yearEnd > date).FirstOrDefault().idYear;
            idClass = db.tbl_studentRegister.Where(z => z.idStudent == idStudent && z.idYear == idYear).FirstOrDefault().idClass;
            if (db.tbl_classes.Where(x => x.idClass == idClass && x.idYear == idYear).Any())
                list = db.tbl_homeWorks.Where(x => x.idClass == idClass && x.exDateTime < date).ToList();


            try
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    model = new homeworkModel();
                    model.idHomeWork = list.ElementAt(i).idHomeWork;
                    int a = list.ElementAt(i).idHomeWork;

                    if (db.tbl_receivedHomeWorks.Where(x => x.idHomeWork == a).Any())
                    {
                        if (db.tbl_receivedHomeWorks.Where(x => x.idHomeWork == a).FirstOrDefault().score != null)
                            model.score = (int)db.tbl_receivedHomeWorks.Where(x => x.idHomeWork == a).FirstOrDefault().score;
                        else
                            model.score = -1;
                    }

                    else
                        model.score = -1;

                    model.exDateTime = list.ElementAt(i).exDateTime;
                    model.UploadDate = list.ElementAt(i).UploadDate;
                    model.idTeacher = list.ElementAt(i).idTeacher;
                    model.idClass = list.ElementAt(i).idClass;
                    model.title = list.ElementAt(i).title;
                    model.body = list.ElementAt(i).body;
                    model.HomeWorkFile = list.ElementAt(i).HomeWorkFile;
                    model.answerBody = list.ElementAt(i).answerBody;
                    model.answerFile = list.ElementAt(i).answerFile;
                    idLesson = list.ElementAt(i).idLesson;
                    model.lessonName = db.tbl_lessons.Where(x => x.idLesson == idLesson).FirstOrDefault().lessonName;
                    ls.Add(model);
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }


        public List<homeworkModel> studentRemainHomeWork(int idStudent)
        {
            List<tbl_homeWorks> list = new List<tbl_homeWorks>();
            List<homeworkModel> ls = new List<homeworkModel>();
            homeworkModel model;
            int idLesson, idYear, idClass;


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


            idYear = db.tbl_years.Where(x => x.yearStart <= date && x.yearEnd > date).FirstOrDefault().idYear;
            idClass = db.tbl_studentRegister.Where(z => z.idStudent == idStudent && z.idYear == idYear).FirstOrDefault().idClass;
            if (db.tbl_classes.Where(x => x.idClass == idClass && x.idYear == idYear).Any())
                list = db.tbl_homeWorks.Where(x => x.idClass == idClass && x.exDateTime >= date).ToList();


            try
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    model = new homeworkModel();
                    model.idHomeWork = list.ElementAt(i).idHomeWork;
                    int a = list.ElementAt(i).idHomeWork;

                    if (db.tbl_receivedHomeWorks.Where(x => x.idHomeWork == a).Any())
                    {
                        if (db.tbl_receivedHomeWorks.Where(x => x.idHomeWork == a).FirstOrDefault().score != null)
                            model.score = (int)db.tbl_receivedHomeWorks.Where(x => x.idHomeWork == a).FirstOrDefault().score;
                        else
                            model.score = -1;
                    }

                    else
                        model.score = -1;

                    model.exDateTime = list.ElementAt(i).exDateTime;
                    model.UploadDate = list.ElementAt(i).UploadDate;
                    model.idTeacher = list.ElementAt(i).idTeacher;
                    model.idClass = list.ElementAt(i).idClass;
                    model.title = list.ElementAt(i).title;
                    model.body = list.ElementAt(i).body;
                    model.HomeWorkFile = list.ElementAt(i).HomeWorkFile;
                    model.answerBody = list.ElementAt(i).answerBody;
                    model.answerFile = list.ElementAt(i).answerFile;
                    idLesson = list.ElementAt(i).idLesson;
                    model.lessonName = db.tbl_lessons.Where(x => x.idLesson == idLesson).FirstOrDefault().lessonName;
                    ls.Add(model);
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }

        public List<tbl_homeWorks> studentHomeWorkFromSchedule(int idClass, int idLesson, int idLesson2, int date)
        {

            try
            {
                if (idLesson2 == -1)
                    return db.tbl_homeWorks.Where(x => x.idClass == idClass && x.idLesson == idLesson && x.exDateTime > date).OrderBy(x => x.exDateTime).ToList();
                else
                    return db.tbl_homeWorks.Where(x => x.idClass == idClass && x.exDateTime > date && (x.idLesson == idLesson || x.idLesson == idLesson2)).OrderBy(x => x.exDateTime).ToList();

            }
            catch
            {
                return null;
            }
        }

        public bool Update(tbl_homeWorks entity)
        {
            try
            {
                db.tbl_homeWorks.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();

                return true;

            }
            catch
            {
                return false;
            }
        }

        public int insert(tbl_homeWorks entity)
        {
            int idHomeWork = 0;
            if (db.tbl_homeWorks.Any())
                idHomeWork = db.tbl_homeWorks.OrderByDescending(p => p.idHomeWork).FirstOrDefault().idHomeWork;
            tbl_homeWorks tbl;

            try
            {
                tbl = new tbl_homeWorks();
                tbl.idHomeWork = idHomeWork + 1;
                tbl.idClass = entity.idClass;
                tbl.idLesson = entity.idLesson;
                tbl.idTeacher = entity.idTeacher;
                tbl.title = entity.title;

                if (entity.HomeWorkFile != null)
                    tbl.HomeWorkFile = entity.HomeWorkFile;

                if (!entity.body.Equals(""))
                    tbl.body = entity.body;

                tbl.exDateTime = entity.exDateTime;
                tbl.UploadDate = entity.UploadDate;
                db.tbl_homeWorks.Add(tbl);
                db.SaveChanges();

                return idHomeWork + 1;

            }
            catch
            {
                return 0;
            }
        }

        public bool Delete(int id)
        {
            List<tbl_receivedHomeWorks> ls = new List<tbl_receivedHomeWorks>();
            int idRecHomeWork;
            tbl_receivedHomeWorks y = null;
            var entity1 = y;

            if (db.tbl_receivedHomeWorks.Any())
                ls = db.tbl_receivedHomeWorks.Where(x => x.idHomeWork == id).ToList();



            try
            {
                for (int i = 0; i < ls.Count(); i++)
                {
                    idRecHomeWork = ls.ElementAt(i).idRecHomeWork;
                    entity1 = db.tbl_receivedHomeWorks.Find(idRecHomeWork);
                    db.Entry(entity1).State = EntityState.Deleted;
                    db.SaveChanges();
                }



                var entity = db.tbl_homeWorks.Find(id);
                db.Entry(entity).State = EntityState.Deleted;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int getHomeworkResponse(receivedHomeWorkModel entity)
        {
            int idRecHomeWork = 0;
            tbl_receivedHomeWorks tbl;

            if (db.tbl_receivedHomeWorks.Any())
                idRecHomeWork = db.tbl_receivedHomeWorks.OrderByDescending(p => p.idRecHomeWork).FirstOrDefault().idRecHomeWork;


            try
            {
                tbl = new tbl_receivedHomeWorks();
                tbl.idRecHomeWork = idRecHomeWork + 1;
                tbl.idHomeWork = entity.idHomeWork;
                tbl.idStudent = entity.idStudent;
                tbl.dateTime = entity.dateTime;
                tbl.answerFile = entity.answerFile;

                if (!entity.answerBody.Equals("null"))
                    tbl.answerBody = entity.answerBody;

                if (entity.answerFile != null)
                    tbl.answerFile = entity.answerFile;

                db.tbl_receivedHomeWorks.Add(tbl);
                db.SaveChanges();
                return idRecHomeWork + 1;
            }

            catch
            {
                return -1;
            }
        }

        public bool uploadHomeWorkResponse(int idRecHomeWork, String fileName)
        {
            try
            {
                tbl_receivedHomeWorks tbl = db.tbl_receivedHomeWorks.Where(x => x.idRecHomeWork == idRecHomeWork).FirstOrDefault();
                tbl.answerFile = fileName;
                db.SaveChanges();
                return true;
            }

            catch
            {
                return false;
            }
        }


        public int getHomeWorksCount(int idClass)
        {
            int homeWorksCount = 0;
            int today = getTodayDate();
            homeWorksCount = db.tbl_homeWorks.Where(x => x.exDateTime >= today && x.idClass == idClass).Count();
            return homeWorksCount;
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