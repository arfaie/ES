using E_School.Helpers.Utitlies;
using E_School.Models;
using E_School.Models.DomainModels;
using E_School.Models.Repositories.api;
using E_School.Models.ViewModel.api;
using E_School.Models.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace E_School.api.Controllers.api.Student
{
    public class ScoreController : ApiController
    {
        ScoreRepository bl = new ScoreRepository();
        YearRepository blYear = new YearRepository();

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
        public HttpResponseMessage AvrageScore(int idMonth, int idLesson, bool isNumeral, int idClass)
        {

            Methods ob = new Methods();
            ExamRepository blExam = new ExamRepository();
            Models.Repositories.StudentRegisterRepository blstudentRegister = new Models.Repositories.StudentRegisterRepository();
            Models.Repositories.api.ScoreRepository blScore = new Models.Repositories.api.ScoreRepository();
            Models.Repositories.StudAvrageRepository blStudAvrage = new Models.Repositories.StudAvrageRepository();
            int today = ob.getTodayDate();
            var MonthDate = ob.Monthes(idMonth).Split(',');
            int Start = int.Parse(MonthDate[0]);
            int End = int.Parse(MonthDate[1]);
            int idYear = blYear.getThisYear();
            IQueryable<View_studentScore> select;
            AvrageScore obAvrage = new AvrageScore();
            List<int> idStuds = new List<int>();
            var selectStudents = blstudentRegister.Where(x => x.idYear == idYear && x.idClass == idClass);

            //آیا نمره ماهانه برای درس ثبت شده است؟ 
            var isExist = blStudAvrage.Where(x => x.idYear == idYear && x.idClass == idClass && x.idLesson == idLesson);
            var jsonSerialiser = new JavaScriptSerializer();

            //اگر نمره ماهانه برای درس ثبت نشده باشد
            if (isExist.FirstOrDefault() == null)
            {
                foreach (var a in selectStudents)
                {
                    idStuds.Add(a.idStudent);
                }
                //محاسبه نمرات عددی
                if (isNumeral)
                {
                    select = blScore.Where(x => x.idLesson == idLesson && x.date <= End && x.date >= Start && x.idDescriptiveScore == -1);
                    foreach (var b in idStuds)
                    {
                        if (!obAvrage.idStudent.Contains(b))
                        {
                            List<double> lsScoreList = new List<double>();
                            var selectScores = select.Where(x => x.idStudent == b);
                            foreach (var s in selectScores)
                            {
                                double score = (double)((s.score * 20) / s.maxScore);
                                lsScoreList.Add(score);
                            }
                            var Name = selectStudents.Where(x => x.idStudent == b).FirstOrDefault();
                            double sum = lsScoreList.Sum();
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
                //محاسبه نمرات توصیفی
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
                                selectDesScore = blDesScore.Where(x => x.numScore > 17).FirstOrDefault();
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
                                selectDesScore = blDesScore.Where(x => x.numScore < 10).FirstOrDefault();
                            }
                            else if (sum == 0)
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

            }
            //اگر نمره ای ثبت شده باشد،سلکت و نمایش بده
            else
            {
                var json = jsonSerialiser.Serialize(isExist);

                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                        json.ToString()

                    )
                };
            }

            //نمایش نمرات محاسبه شده
            var json_ = jsonSerialiser.Serialize(obAvrage);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json_.ToString()

                )
            };
        }

        [ActionName("StudentAvrage")]
        [HttpPost]
        public int StudentAvrage([FromBody]StudentAvrage entity)
        {
            //Add Data to Database
            try
            {
                Models.Repositories.StudAvrageRepository bl = new Models.Repositories.StudAvrageRepository();
                tbl_Avrages ob;
                int i = 0;
                foreach (var a in entity.idStudent)
                {
                    try
                    {
                        ob = new tbl_Avrages();
                        int id = bl.GetLastIdentity() + 1;
                        ob.id = id + i;
                        ob.idClass = entity.idClass;
                        ob.idDesScore = entity.idDescriptiveScore[i];
                        ob.idLesson = entity.idLesson;
                        ob.idStudent = entity.idStudent[i];
                        ob.idYear = blYear.getThisYear();
                        ob.Score = entity.NumeralScores[i];
                        ob.idMonth = entity.idMonth;
                        i++;
                        if (entity.idStudent.Count() != i)
                        {
                            bl.Add(ob, false);
                        }
                        else
                        {
                            bl.Add(ob, true);
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                    //bl.Add(ob, true);

                }
            }
            catch
            {
                return -2;
            }
            return 1;
        }
        [ActionName("AvrageScore")]
        [HttpGet]
        public HttpResponseMessage GetAvrage(int idMonth, int idClass, int idStudent)
        {
            Models.Repositories.StudAvrageRepository bl = new Models.Repositories.StudAvrageRepository();
            int idYear = blYear.getThisYear();
            var select = bl.Where(x => x.idMonth == idMonth && x.idClass == idClass && x.idStudent == idStudent && x.idYear == idYear);
            var jsonSerialiser = new JavaScriptSerializer();
            if (select != null)
            {
                var json_ = jsonSerialiser.Serialize(select);

                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                        json_.ToString()

                    )
                };
            }
            else
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                                        "-1".ToString()

                                    )
                };
            }

        }
    }
}
