using E_School.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace E_School.Helpers.Utitlies
{
    public class Methods
    {
        public bool isEditable(int idYear)
        {
            int Date = 0;
            Date = Date.GetPersianDate();
            YearRepository bl = new YearRepository();
            var select = bl.Where(x => x.idYear == idYear).Single();
            bool isOK = select.yearStart < Date & select.yearEnd > Date;
            if (isOK)
            {
                return true;
            }
            else
                return false;
        }

        public string StudentName(int idStud)
        {
            StudentRepository blStud = new StudentRepository();
            string fName = (blStud.Where(x => x.idStudent == idStud).Single().FName);
            string lName = (blStud.Where(x => x.idStudent == idStud).Single().LName);

            return fName + " " + lName;
        }

        public string yearName()
        {
            int today = 0;
            today = today.GetPersianDate();
            YearRepository year = new YearRepository();
            string Name = year.Where(x => x.yearStart < today && x.yearEnd > today).Single().yearName;
            return Name;
        }

        public string DiPath(int idClass, int idStud)
        {
            string _yearName = yearName();
            ClassRepository blClass = new ClassRepository();
            string className = blClass.Where(x => x.idClass == idClass).Single().className;

            string studName = StudentName(idStud);

            return System.Web.Hosting.HostingEnvironment.MapPath("~/Content/گزارشات/" + _yearName + "/" + className + "/" + studName);
        }

        public int CalculateSmsLength(string text)
        {

            int l = text.Length;
            int test = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 67));
            return text.Length <= 70 ? 1 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 67));
        }

        public string Monthes(int id)
        {
            int today = getTodayDate();
            string Today = today.ToString().Substring(0, 4);
            switch (id)
            {
                case 1:
                    return Today + "0701," + Today + "0730";
                case 2:
                    return Today + "0801," + Today + "0830";
                case 3:
                    return Today + "0901," + Today + "0930";
                case 4:
                    return Today + "1001," + Today + "1030";
                case 5:
                    return Today + "1101," + Today + "1130";
                case 6:
                    return Today + "1201," + Today + "1230";
                case 7:
                    return Today + "0101," + Today + "0131";
                case 8:
                    return Today + "0201," + Today + "0231";
            }
            return null;
        }

        public int getTodayDate()
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
        //public string yearName()
        //{
        //    int today = 0;
        //    today = today.GetPersianDate();
        //    YearRepository year = new YearRepository();
        //    string Name = year.Where(x => x.yearStart < today && x.yearEnd > today).Single().yearName;
        //    return Name;
        //}




    }
}