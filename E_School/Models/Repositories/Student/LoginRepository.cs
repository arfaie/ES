using E_School.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories.api
{
    public class LoginRepository
    {
        private schoolEntities db = null;
        private int today;

        public LoginRepository()
        {
            db = new schoolEntities();
        }


        public List<View_studentInfo> login(int idUserType, string user, string pass)
        {
            today = getTodayDate();
            int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd > today).FirstOrDefault().idYear;

            // get student information
            if (idUserType == 0)
            {
                try
                {
                    int idStudent = db.tbl_students.Where(x => x.studUser == user && x.pass == pass && x.isActive == true).FirstOrDefault().idStudent;
                    var aa= db.View_studentInfo.Where(x => x.idStudent == idStudent && x.idYear == idYear).ToList();
                    return aa;
                }
                catch
                {
                    return null;
                }
            }



            // get student parent information
            else if (idUserType == 1)
            {
                try
                {
                    int idStudent = db.tbl_students.Where(x => x.pUser == user && x.pPass == pass).FirstOrDefault().idStudent;
                     var aa= db.View_studentInfo.Where(x => x.idStudent == idStudent && x.idYear == idYear).ToList();
                    return aa;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }


        public IQueryable<tbl_teachers> TeacherLogin(string user, string pass)
        {
            try
            {
                var a= db.tbl_teachers.Where(x => x.teacherUser == user && x.teacherPass == pass).AsQueryable();
                var q = a.ToList();
                return a;
            }
            catch
            {
                return null;
            }
        }

        public int changePassword(int idUserType, int idUser, String lastPass, String newPass)
        {
            String pass;




            if (idUserType == 0) // student's pass
            {
                try
                {
                    tbl_students tbl = db.tbl_students.Where(x => x.idStudent == idUser).FirstOrDefault();
                    pass = tbl.pass;

                    if (pass.Equals(lastPass))
                    {
                        tbl.pass = newPass;
                        db.SaveChanges();
                        return 1;
                    }

                    else
                        return -1;
                }

                catch
                {
                    return -1;
                }

            }



            else if (idUserType == 1) // parent's pass
            {
                try
                {
                    tbl_students tbl = db.tbl_students.Where(x => x.idStudent == idUser).FirstOrDefault();
                    pass = tbl.pPass;

                    if (pass.Equals(lastPass))
                    {
                        tbl.pPass = newPass;
                        db.SaveChanges();
                        return 1;
                    }

                    else
                        return -1;
                }

                catch
                {
                    return -1;
                }

            }



            else if (idUserType == 2) // teacher's pass
            {
                try
                {
                    tbl_teachers tbl = db.tbl_teachers.Where(x => x.idTeacher == idUser).FirstOrDefault();
                    pass = tbl.teacherPass;

                    if (pass.Equals(lastPass))
                    {
                        tbl.teacherPass = newPass;
                        db.SaveChanges();
                        return 1;
                    }

                    else
                        return -1;
                }

                catch
                {
                    return -1;
                }

            }

            return -1;
        }


        public IQueryable<View_studentInfo> getStudentInformation(int idUser)
        {
            return db.View_studentInfo.Where(x => x.idStudent == idUser).AsQueryable();
        }


        public IQueryable<tbl_teachers> getTeacherInformation(int id)
        {
            try
            {
                return db.tbl_teachers.Where(x => x.idTeacher == id).AsQueryable();
            }
            catch
            {
                return null;
            }
        }


        public IQueryable<tbl_students> editStudentInfo(int id)
        {
            return db.tbl_students.Where(x => x.idStudent == id).AsQueryable();
        }


        public bool editingStudentInfo(tbl_students entity)
        {
            tbl_students tbl = db.tbl_students.Where(x => x.idStudent == entity.idStudent).FirstOrDefault();

            try
            {
                //tbl.studentCode = 1;
                tbl.FName = entity.FName;
                tbl.LName = entity.LName;
                tbl.fatherName = entity.fatherName;
                tbl.gender = entity.gender;
                tbl.birthDate = entity.birthDate;
                //tbl.idStatus = 6;
                tbl.natCode = entity.natCode;

                if (entity.imgManifest != null)
                    tbl.imgManifest = entity.imgManifest;

                if (entity.imgPersonal != null)
                    tbl.imgPersonal = entity.imgPersonal;

                tbl.leftHand = entity.leftHand;
                tbl.homeAddress = entity.homeAddress;

                if (!entity.studentMobile.Equals(""))
                    tbl.studentMobile = entity.studentMobile;

                tbl.idMarrige = entity.idMarrige;
                tbl.religion = entity.religion;



                if (entity.idFatherEdu != 0)
                    tbl.idFatherEdu = entity.idFatherEdu;

                if (!entity.fJob.Equals(""))
                    tbl.fJob = entity.fJob;

                if (!entity.fJobAddress.Equals(""))
                    tbl.fJobAddress = entity.fJobAddress;

                if (!entity.fJobPhone.Equals(""))
                    tbl.fJobPhone = entity.fJobPhone;

                if (!entity.motherName.Equals(""))
                    tbl.motherName = entity.motherName;

                if (entity.idMotherEdu != 0)
                    tbl.idMotherEdu = entity.idMotherEdu;

                if (!entity.mJob.Equals(""))
                    tbl.mJob = entity.mJob;

                if (!entity.mJobAddress.Equals(""))
                    tbl.mJobAddress = entity.mJobAddress;

                if (!entity.mJobPhone.Equals(""))
                    tbl.mJobPhone = entity.mJobPhone;

                if (!entity.mPhone.Equals(""))
                    tbl.mPhone = entity.mPhone;

                tbl.fPhone = entity.fPhone;


                db.SaveChanges();

                return true;
            }

            catch
            {
                return false;
            }
        }


        public IQueryable<tbl_teachers> getTeacherInfo(int id)
        {
            return db.tbl_teachers.Where(x => x.idTeacher == id).AsQueryable();
        }


        public bool editingTeacherInfo(tbl_teachers entity)
        {
            tbl_teachers tbl = db.tbl_teachers.Where(x => x.idTeacher == entity.idTeacher).FirstOrDefault();

            try
            {
                //tbl.studentCode = 1;
                tbl.FName = entity.FName;
                tbl.LName = entity.LName;
                tbl.fatherName = entity.fatherName;
                tbl.gender = entity.gender;
                tbl.birthDate = entity.birthDate;
                //tbl.idStatus = 6;
                tbl.natCode = entity.natCode;

                if (entity.imgManifest != null)
                    tbl.imgManifest = entity.imgManifest;

                if (entity.imgPersonal != null)
                    tbl.imgPersonal = entity.imgPersonal;

                tbl.address = entity.address;

                if (!entity.mobile.Equals(""))
                    tbl.mobile = entity.mobile;

                if (!entity.phone.Equals(""))
                    tbl.phone = entity.phone;

                if (!entity.email.Equals(""))
                    tbl.email = entity.email;



                db.SaveChanges();

                return true;
            }

            catch
            {
                return false;
            }
        }


        public string getLastVersion()
        {
            try
            {
                tbl_Setting tbl = db.tbl_Setting.FirstOrDefault();

                if (tbl != null)
                {
                    return tbl.teacherVersion;
                }

                else
                    return null;
            }

            catch
            {
                return null;
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