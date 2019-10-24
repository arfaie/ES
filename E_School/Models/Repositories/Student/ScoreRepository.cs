using E_School.Models.DomainModels;
using E_School.Models.ViewModel.api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;


namespace E_School.Models.Repositories.api
{
    public class ScoreRepository
    {
        private schoolEntities db = null;

        public ScoreRepository()
        {
            db = new schoolEntities();
        }


        public List<View_studentScore> StudentScore(int idStudent)
        {
            int today = getTodayDate();
            int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault().idYear;

            try
            {
                return db.View_studentScore.Where(x => x.idYear == idYear && x.idStudent == idStudent && x.idExam != -1).OrderByDescending(x => x.idScore).ToList();
            }
            catch
            {
                return null;
            }
        }

        public Boolean setHomeWorkScore(int idReceivedHomeWork, float score)
        {


            try
            {
                tbl_receivedHomeWorks tbl = db.tbl_receivedHomeWorks.Where(x => x.idRecHomeWork == idReceivedHomeWork).FirstOrDefault();
                tbl.score = score;
                db.SaveChanges();


                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean insertScores(scoreModel entity)
        {
            int idScore;
            tbl_scores tbl;


            try
            {
                for (int i = 0; i < entity.idStudent.Count(); i++)
                {
                    tbl = new tbl_scores();

                    if (db.tbl_scores.Any())
                        idScore = db.tbl_scores.OrderByDescending(p => p.idScore).FirstOrDefault().idScore;
                    else
                        idScore = 0;

                    tbl.idScore = idScore + 1;
                    tbl.idStudent = entity.idStudent.ElementAt(i);
                    tbl.idExam = entity.idExam;
                    tbl.date = entity.date;

                    if (entity.idDescriptiveScore == 0)
                    {
                        tbl.score = entity.score.ElementAt(i);
                        tbl.idDescriptiveScore = -1;
                    }


                    else
                    {
                        tbl.idDescriptiveScore = (int)entity.score.ElementAt(i);
                        tbl.score = -1;
                    }




                    db.tbl_scores.Add(tbl);
                    db.SaveChanges();
                }

                return true;
            }

            catch
            {
                return false;
            }
        }

        public List<descriptiveScore> getDiscriptiveScores()
        {
            List<descriptiveScore> ls = new List<descriptiveScore>();
            List<tbl_descriptiveScores> list = new List<tbl_descriptiveScores>();
            descriptiveScore model;


            try
            {
                list = db.tbl_descriptiveScores.ToList();
                for (int i = 0; i < list.Count(); i++)
                {
                    model = new descriptiveScore();
                    if (list.ElementAt(i).idDescriptiveScore != -1)
                    {
                        model.idDescriptiveScore = list.ElementAt(i).idDescriptiveScore;
                        model.desScore = list.ElementAt(i).desScore;
                        ls.Add(model);
                    }
                }

                return ls;
            }
            catch
            {
                return null;
            }
        }

        public List<submitScores> getSubmitedScores(int idExam, int idClass)
        {
            List<tbl_scores> ls = new List<tbl_scores>();
            List<submitScores> list = new List<submitScores>();
            List<tbl_studentRegister> students = new List<tbl_studentRegister>();
            submitScores model;

            try
            {
                students = db.tbl_studentRegister.Where(x => x.idClass == idClass).ToList();
                ls = db.tbl_scores.Where(x => x.idExam == idExam).ToList();

                if (students != null)
                {

                    for (int k = 0; k < students.Count(); k++)
                    {
                        model = new submitScores();
                        model.idStudent = students.ElementAt(k).idStudent;
                        int idStudent = students.ElementAt(k).idStudent;
                        model.studentName = db.tbl_students.Where(x => x.idStudent == idStudent).First().FName + " " + db.tbl_students.Where(x => x.idStudent == idStudent).First().LName;
                        model.idDescriptiveScore = -1;

                        for (int i = 0; i < ls.Count(); i++)
                        {
                            if (ls.ElementAt(i).idStudent == students.ElementAt(k).idStudent)
                            {
                                model.idScore = ls.ElementAt(i).idScore;
                                model.score = (float)ls.ElementAt(i).score;
                                model.idDescriptiveScore = ls.ElementAt(i).idDescriptiveScore;
                                if (model.idDescriptiveScore != -1)
                                    model.desScore = db.tbl_descriptiveScores.Where(x => x.idDescriptiveScore == model.idDescriptiveScore).FirstOrDefault().desScore;
                            }

                           
                        }
                        list.Add(model);
                    }


                    list = list.OrderBy(x => x.studentName).ToList();

                    return list;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public Boolean editScores(scoreModel entity)
        {
            int idScore;
            

            try
            {
                for (int i = 0; i < entity.scoreId.Count(); i++)
                {


                    idScore = entity.scoreId.ElementAt(i);

                    if (idScore != 0)
                    {
                        tbl_scores tbl = db.tbl_scores.Where(x => x.idScore == idScore).FirstOrDefault();

                        if (entity.idDescriptiveScore == 0)
                        {
                            if (entity.score.ElementAt(i) != -1)
                            {
                                tbl.score = entity.score.ElementAt(i);
                                tbl.idDescriptiveScore = -1;
                                tbl.date = entity.date;
                                db.SaveChanges();
                            }

                            else
                            {
                                db.tbl_scores.Remove(tbl);
                                db.SaveChanges();
                            }
                           
                        }


                        else
                        {
                            if (entity.score.ElementAt(i) != -10)
                            {
                                tbl.idDescriptiveScore = (int)entity.score.ElementAt(i);
                                tbl.score = -1;
                                tbl.date = entity.date;
                                db.SaveChanges();
                            }

                            else
                            {
                                db.tbl_scores.Remove(tbl);
                                db.SaveChanges();
                            }

                        }
                        
                    }

                    else
                    {
                        tbl_scores tbl = new tbl_scores();

                        if (db.tbl_scores.Any())
                            idScore = db.tbl_scores.OrderByDescending(p => p.idScore).FirstOrDefault().idScore;
                        else
                            idScore = 0;

                        tbl.idScore = idScore + 1;
                        tbl.idStudent = entity.idStudent.ElementAt(i);
                        tbl.idExam = entity.idExam;
                        tbl.date = entity.date;

                        if (entity.idDescriptiveScore == 0)
                        {
                            tbl.score = entity.score.ElementAt(i);
                            tbl.idDescriptiveScore = -1;
                        }


                        else
                        {
                            tbl.idDescriptiveScore = (int)entity.score.ElementAt(i);
                            tbl.score = -1;
                        }

                        db.tbl_scores.Add(tbl);
                        db.SaveChanges();
                    }
                }

                return true;
            }

            catch
            {
                return false;
            }
        }

        public int checkSubmitedExam(int idExam)
        {
            try
            {
                if (db.tbl_scores.Where(x => x.idExam == idExam).Any())
                    return 1;
                else
                    return 0;
            }

            catch
            {
                return -1;
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
        public IQueryable<View_studentScore> Where(System.Linq.Expressions.Expression<Func<View_studentScore, bool>> predicate)
        {
            try
            {
                return db.View_studentScore.Where(predicate);
            }
            catch
            {
                return null;
            }
        }
    }
}