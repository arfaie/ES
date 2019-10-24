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
        public Boolean editScores([FromBody ]scoreModel entity)
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
        public List<AvrageScore> AvrageScore(int idMonth,int idLesson,bool isNumeral)
        {
            Methods ob = new Methods();
            YearRepository blYear = new YearRepository();
            ExamRepository blExam = new ExamRepository();
            Models.Repositories.ScoreRepository blScore = new Models.Repositories.ScoreRepository();
            int today = ob.getTodayDate();
            var MonthDate = ob.Monthes(idMonth).Split(',');
            int Start =int.Parse(MonthDate[0]);
            int End =int.Parse(MonthDate[1]);
            int idYear = blYear.Where(x => x.yearStart <= today && x.yearEnd > today).FirstOrDefault().idYear;
            if (isNumeral)
            {
                //var select = blExam.Where(x => x.idLesson == idLesson && x.examDate <= End && x.examDate >= Start&&x.);
            }
            else
            {

            }
            return null;
        }

        //[ActionName("StudentAvrage")]
        //[HttpGet]
        //public List<StudentAvrage> StudentAvrage([FromUri]int startDate,int endDate,int idStudent)
        //{
        //    try
        //    {
        //        int numeralCount=0, DescriptiveCount=0;
        //        var select = bl.StudentScore(idStudent).Where(x => x.date >= startDate && x.date <= endDate && x.score!=-1);//تمام نمرات عددی
        //        var select2 = bl.StudentScore(idStudent).Where(x => x.date >= startDate && x.date <= endDate && x.score==-1);//تمام نمرات توصیفی

        //        List<StudentAvrage> av = new List<StudentAvrage>();
        //        StudentAvrage ob=null;

        //        if (select.Count() != 0)
        //        {
        //            numeralCount = select.Count();
        //            foreach (var a in select)
        //            {
        //                ob = new StudentAvrage();
        //                int i = 0;
        //                ob.NumeralScores[i] = a.score;
        //                av.Add(ob);
        //                i++;
        //            }
        //        }
        //        if (select2.Count() != 0)
        //        {
        //            DescriptiveCount = select2.Count();
        //            foreach (var a in select2)
        //            {
        //                ob = new StudentAvrage();
        //                int i = 0;
        //                string Score = a.desScore;
        //                ob.DescriptiveScores[i] = Score;
        //                av.Add(ob);
        //                i++;
        //            }
        //        }
        //        int Count = numeralCount + DescriptiveCount;
        //        int sum = (int)select.Sum(x => x.numScore);
        //        int Avrage =Convert.ToInt32(sum / Count);
        //        if (Avrage >= 17)
        //        {
        //            ob.Avrage = "خیلی خوب";
        //        }
        //        else if (Avrage >= 17)
        //        {
        //            ob.Avrage = "خوب";
        //        }
        //        else if (Avrage >= 17)
        //        {
        //            ob.Avrage = "قابل قبول";
        //        }
        //        else
        //        {
        //            ob.Avrage = "نیازمند به تلاش";
        //        }
        //        return av;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        



    }
}
