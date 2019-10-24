using E_School.Models.DomainModels;
using E_School.Models.ViewModel.api;
using E_School.Models.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories.api
{
    public class ExamRepository
    {
        private schoolEntities db = null;

        public ExamRepository()
        {
            db = new schoolEntities();
        }

        public List<View_Exam> TeacherExam(int idTeacher, int idTerm)
        {
            try
            {
                int todays = today();
                int idYear = db.tbl_years.Where(x => x.yearStart <= todays && x.yearEnd > todays).FirstOrDefault().idYear;
                return db.View_Exam.Where(x => x.idTeacher == idTeacher && x.idTerm == idTerm && x.idYear == idYear && x.idExamType != 3 && x.idExam != -1).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<View_Exam> StudentExam(int idTerm, int idClass)
        {
            try
            {
                int todays = today();
                int idYear = db.tbl_years.Where(x => x.yearStart <= todays && x.yearEnd > todays).FirstOrDefault().idYear;
                return db.View_Exam.Where(x => x.idYear == idYear && x.idTerm == idTerm && x.idClass == idClass && x.idExamType != 3 && x.idExam != -1).OrderBy(x=>x.examDate).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<View_Exam> StudentClassExam(int date, int idClass)
        {
            try
            {
                int todays = today();
                int idYear = db.tbl_years.Where(x => x.yearStart <= todays && x.yearEnd > todays).FirstOrDefault().idYear;
                return db.View_Exam.Where(x => x.idYear == idYear && x.idClass == idClass && x.examDate >= date && x.idExamType == 3 && x.idExam != -1).OrderBy(x=>x.examDate).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<View_Exam> TeacherClassExam(int idTeacher)
        {
            try
            {
                int todays = today();
                int idYear = db.tbl_years.Where(x => x.yearStart <= todays && x.yearEnd > todays).FirstOrDefault().idYear;
                return db.View_Exam.Where(x => x.idTeacher == idTeacher && x.idYear == idYear && x.idExamType == 3 && x.idExam != -1).OrderByDescending(x => x.examDate).ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<examModel> getExamTypes()
        {
            List<examModel> ls = new List<examModel>();
            examModel model;
            try
            {
                List<tbl_examTypes> list = db.tbl_examTypes.ToList();

                for (int i = 0; i < list.Count() ; i++)
                {
                    model = new examModel();
                    model.idExamType = list.ElementAt(i).idExamType;
                    model.examType = list.ElementAt(i).examType;
                    ls.Add(model);
                    
                }
                return ls;
            }

            catch
            {
                return null;
            }
        }

        public List<ScheduleExam> GetScheduleExams(int idClass, int idLesson, int idLesson2, int date)
        {
            List<ScheduleExam> list = new List<ScheduleExam>();
            ScheduleExam model;
            int idLsn;
            List<tbl_exams> ls = new List<tbl_exams>();

            try
            {
                if(idLesson2 != -1)
                    ls = db.tbl_exams.Where(x => x.idClass == idClass && x.examDate > date && x.idExamType == 3 && (x.idLesson == idLesson || x.idLesson == idLesson2)).OrderBy(x => x.examDate).ToList();
                 
                else
                    ls = db.tbl_exams.Where(x => x.idClass == idClass && x.examDate > date && x.idExamType == 3 && x.idLesson == idLesson).OrderBy(x => x.examDate).ToList();


                for (int i = 0; i < ls.Count(); i++)
                {
                    model = new ScheduleExam();
                    model.examDate = ls.ElementAt(i).examDate;
                    idLsn = ls.ElementAt(i).idLesson;
                    model.lessonName = db.tbl_lessons.Where(x => x.idLesson == idLsn).FirstOrDefault().lessonName;
                    list.Add(model);
                }

                return list;
            }

            catch
            {
                return null;
            }
        }


        public int insertExam(tbl_exams entity)
        {
            tbl_exams tbl = new tbl_exams();
            int idTerm, idExam;

            int todays = today();
            int idYear = db.tbl_years.Where(x => x.yearStart <= todays && x.yearEnd > todays).FirstOrDefault().idYear;
            if (db.tbl_terms.Where(x => x.idYear == idYear && (x.termStart <= entity.examDate && x.termEnd >= entity.examDate)).Any())
                idTerm = db.tbl_terms.Where(x => x.idYear == idYear && (x.termStart <= entity.examDate && x.termEnd >= entity.examDate)).FirstOrDefault().idTerm;
            else
                return -1;

            if (db.tbl_exams.OrderByDescending(p => p.idExam).Any())
                idExam = db.tbl_exams.OrderByDescending(p => p.idExam).FirstOrDefault().idExam;

            else
                idExam = 0;

            try
            {
                tbl.idExam = idExam + 1;
                tbl.idExamType = 3; // class exam
                tbl.idLesson = entity.idLesson;
                tbl.idTeacher = entity.idTeacher;
                tbl.idClass = entity.idClass;
                tbl.examTitle = entity.examTitle;
                tbl.examDate = entity.examDate;
                
                //tbl.startTime = entity.startTime;
                tbl.idTerm = idTerm;
                tbl.maxScore = entity.maxScore;
                if(entity.des != null)
                    tbl.des = entity.des;
                db.tbl_exams.Add(tbl);
                db.SaveChanges();

                return 1;
            }

            catch
            {
                return 0;
            }

        }



        public bool editExam(tbl_exams entity)
        {
            tbl_exams tbl = db.tbl_exams.Where(x => x.idExam == entity.idExam).FirstOrDefault();
            

            try
            {
                tbl.examTitle = entity.examTitle;
                tbl.examDate = entity.examDate;
                tbl.maxScore = entity.maxScore;
                if (entity.des != null)
                    tbl.des = entity.des;
                //db.tbl_exams.Attach(tbl);
                //tbl.maxScore = 15;
                db.SaveChanges();

                return true;
            }

            catch
            {
                return false;
            }
        }


        public int deleteExam(int idExam)
        {
            List<tbl_scores> ls = new List<tbl_scores>();
            int idScore;
            tbl_scores y = null;
            var entity1 = y;

            if (db.tbl_scores.Where(x => x.idExam == idExam).Count() > 0)
                ls = db.tbl_scores.Where(x => x.idExam == idExam).ToList();



            try
            {
                for (int i = 0; i < ls.Count(); i++)
                {
                    idScore = ls.ElementAt(i).idScore;
                    entity1 = db.tbl_scores.Find(idScore);
                    db.Entry(entity1).State = EntityState.Deleted;
                    db.SaveChanges();
                }



                var entity = db.tbl_exams.Find(idExam);
                db.Entry(entity).State = EntityState.Deleted;
                db.SaveChanges();

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public int GetExamMaxScore(int idExam)
        {
            int maxScore;
            
            try
            {
                maxScore = (int) db.tbl_exams.Where(x => x.idExam == idExam).FirstOrDefault().maxScore;
                return maxScore;
            }
            catch
            {
                return 0;
            }
        }

        private int today()
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

        public IQueryable<View_Exam> Where(System.Linq.Expressions.Expression<Func<View_Exam, bool>> predicate)
        {
            try
            {
                return db.View_Exam.Where(predicate);
            }
            catch
            {
                return null;
            }
        }
    }
}