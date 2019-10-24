using E_School.Helpers.Utitlies;
using E_School.Models;
using E_School.Models.DomainModels;
using E_School.Models.Repositories.api;
using E_School.Models.ViewModel.api;
using E_School.Models.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace E_School.api.Controllers.api.Student
{
    public class ScoreController : ApiController
    {
        ScoreRepository bl = new ScoreRepository();

        [ActionName("StudentScore")]
        [HttpPost]
        public List<View_studentScore> StudentScore([FromBody] int idStudent)
        {
            return bl.StudentScore(idStudent);
        }



        [ActionName("StudentReport")]
        [HttpGet]
        public string StudentReport([FromUri] int idYear, [FromUri] int idTerm)
        {
            string test = "https://up.7learn.com/ss/andrd/7Learn-saeid-shahini.pdf";
            string noReport = "no";
            return test;

        }




        [ActionName("setHomeWorkScore")]
        [HttpGet]
        public Boolean setHomeWorkScore([FromUri] int idReceivedHomeWork, [FromUri] float score)
        {
            if (bl.setHomeWorkScore(idReceivedHomeWork, score))
                return true;

            else
                return false;
        }



        [ActionName("getScores")]
        [HttpPost]
        public Boolean getScores([FromBody] scoreModel entity)
        {
            return bl.insertScores(entity);
        }



        [ActionName("getDiscriptiveScores")]
        [HttpGet]
        public List<descriptiveScore> getDiscriptiveScores()
        {
            return bl.getDiscriptiveScores();
        }





        [ActionName("getSubmitedScores")]
        [HttpGet]
        public List<submitScores> getSubmitedScores([FromUri]int idExam, [FromUri]int idClass)
        {
            return bl.getSubmitedScores(idExam, idClass);
        }




        [ActionName("editScores")]
        [HttpPost]
        public Boolean editScores([FromBody]scoreModel entity)
        {
            return bl.editScores(entity);
        }



        [ActionName("checkSubmitedExam")]
        [HttpGet]
        public int checkSubmitedExam([FromUri]int idExam)
        {
            return bl.checkSubmitedExam(idExam);
        }

        [ActionName("AvrageScore")]
        [HttpGet]
        public AvrageScore AvrageScore(int idMonth, int idLesson, bool isNumeral,int idClass)
        {
            
            Methods ob = new Methods();
            YearRepository blYear = new YearRepository();
            ExamRepository blExam = new ExamRepository();
            Models.Repositories.StudentRegisterRepository blstudentRegister = new Models.Repositories.StudentRegisterRepository();
            Models.Repositories.api.ScoreRepository blScore = new Models.Repositories.api.ScoreRepository();
            int today = ob.getTodayDate();
            var MonthDate = ob.Monthes(idMonth).Split(',');
            int Start = int.Parse(MonthDate[0]);
            int End = int.Parse(MonthDate[1]);
            int idYear = blYear.Where(x => x.yearStart <= today && x.yearEnd > today).FirstOrDefault().idYear;
            IQueryable<View_studentScore> select;
            AvrageScore obAvrage = new AvrageScore();
            List<int> idStuds = new List<int>();
            var selectStudents = blstudentRegister.Where(x => x.idYear == idYear && x.idClass == idClass);
            foreach (var a in selectStudents)
            {
                idStuds.Add(a.idStudent);
            }
            if (isNumeral)
            {
                select = blScore.Where(x => x.idLesson == idLesson && x.date <= End && x.date >= Start && x.idDescriptiveScore == -1);
                foreach (var b in idStuds)
                {
                    if (!obAvrage.idStudent.Contains(b))
                    {
                        var selectScores = select.Where(x => x.idStudent == b);
                        var Name = selectScores.FirstOrDefault();
                        double sum = selectScores.Sum(x => x.score);
                        int cnt = selectScores.Count();
                        double Avrage = Math.Round(sum / cnt, 2);
                        obAvrage.StudentName.Add(Name.FName + " " + Name.LName);
                        obAvrage.idStudent.Add(b);
                        if (sum != 0)
                        {
                            obAvrage.NumeralScores.Add(Avrage);
                        }
                        else
                        {
                            obAvrage.NumeralScores.Add(0);
                        }
                    }
                }
            }
            else
            {
                Models.Repositories.DescriptiveScoreRepository blDesScore = new Models.Repositories.DescriptiveScoreRepository();
                select = blScore.Where(x => x.idLesson == idLesson && x.date <= End && x.date >= Start && x.score == -1);
                string avrage = null;
                int idDesScore = 0;
                foreach (var b in idStuds)
                {
                    if (!obAvrage.idStudent.Contains(b))
                    {
                        tbl_descriptiveScores selectDesScore = new tbl_descriptiveScores();
                        var selectScores = select.Where(x => x.idStudent == b);
                        var Name = selectScores.FirstOrDefault();
                        float sum = (int)selectScores.Sum(x => x.numScore);
                        int cnt = selectScores.Count();
                        float n = sum / cnt;
                        double Avrage = Math.Round(n);
                        if (Avrage > 17)
                        {
                            selectDesScore=blDesScore.Where(x => x.numScore > 17).FirstOrDefault() ;
                        }
                        else if (Avrage <= 17 && Avrage > 14)
                        {
                            selectDesScore = blDesScore.Where(x => x.numScore <= 17 && x.numScore > 14).FirstOrDefault();
                        }
                        else if (Avrage <= 14 && Avrage >= 10)
                        {
                            selectDesScore = blDesScore.Where(x => x.numScore <= 14 && x.numScore >= 10).FirstOrDefault();
                        }
                        else if (Avrage < 10)
                        {
                            selectDesScore = blDesScore.Where(x => x.numScore <10).FirstOrDefault();
                        }
                        else if (sum==0)
                        {
                            selectDesScore = blDesScore.Where(x => x.numScore < 10).FirstOrDefault();
                        }
                        avrage = selectDesScore.desScore;
                        idDesScore = selectDesScore.idDescriptiveScore;

                        obAvrage.StudentName.Add(Name.FName + " " + Name.LName);
                        obAvrage.idStudent.Add(b);
                        obAvrage.idDescriptiveScore.Add(idDesScore);
                        obAvrage.DescriptiveScores.Add(avrage);

                    }
                }
            }

            return obAvrage;
        }

        [ActionName("StudentAvrage")]
        [HttpGet]
        public bool StudentAvrage([FromBody]StudentAvrage entity)
        {
            //Add Data to Database
            try
            {
                Models.Repositories.StudAvrageRepository bl = new Models.Repositories.StudAvrageRepository();
                foreach(var a in entity.NumeralScores)
                {
                    
                }
                //bl.Add()
                //int numeralCount = 0, DescriptiveCount = 0;
                //var NumeralSelect = bl.StudentScore(idStudent).Where(x => x.date >= startDate && x.date <= endDate && x.score != -1);//تمام نمرات عددی
                //var DescSelect = bl.StudentScore(idStudent).Where(x => x.date >= startDate && x.date <= endDate && x.score == -1);//تمام نمرات توصیفی

                //List<StudentAvrage> LsAvrg = new List<StudentAvrage>();
                //StudentAvrage ob = null;

                //if (NumeralSelect.Count() != 0)
                //{
                //    numeralCount = NumeralSelect.Count();
                //    foreach (var a in NumeralSelect)
                //    {
                //        ob = new StudentAvrage();
                //        int i = 0;
                //        ob.NumeralScores[i] = a.score;
                //        LsAvrg.Add(ob);
                //        i++;
                //    }
                //}
                //if (DescSelect.Count() != 0)
                //{
                //    DescriptiveCount = DescSelect.Count();
                //    foreach (var a in DescSelect)
                //    {
                //        ob = new StudentAvrage();
                //        int i = 0;
                //        string Score = a.desScore;
                //        ob.DescriptiveScores[i] = Score;
                //        LsAvrg.Add(ob);
                //        i++;
                //    }
                //}
                //int Count = numeralCount + DescriptiveCount;
                //int sum = (int)NumeralSelect.Sum(x => x.numScore);
                //int Avrage = Convert.ToInt32(sum / Count);
                //if (Avrage >= 17)
                //{
                //    ob.Avrage = "خیلی خوب";
                //}
                //else if (Avrage >= 17)
                //{
                //    ob.Avrage = "خوب";
                //}
                //else if (Avrage >= 17)
                //{
                //    ob.Avrage = "قابل قبول";
                //}
                //else
                //{
                //    ob.Avrage = "نیازمند به تلاش";
                //}
                return null;
            }
            catch
            {
                return null;
            }
        }





    }
}
