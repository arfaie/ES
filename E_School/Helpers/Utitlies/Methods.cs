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
            int test= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 67));
            return text.Length <= 70 ? 1 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 67));
        }

        public string Monthes(int id) 
        {
            int today = getTodayDate();
            switch (id)
            {
                case 1:
                    return today.ToString() + "0701,0730";
                case 2:
                    return today.ToString() + "0801,0830";
                case 3:
                    return today.ToString() + "0901,0930";
                case 4:
                    return today.ToString() + "1001,1030";
                case 5:
                    return today.ToString() + "1101,1130";
                case 6:
                    return today.ToString() + "1201,1230";
                case 7:
                    return today.ToString() + "0101,0131";
                case 8:
                    return today.ToString() + "0201,0231";
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