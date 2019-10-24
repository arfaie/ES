using E_School.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


public static class Utilty
{
    public static string GetErrors(this ModelStateDictionary modelState)
    {
        return string.Join("<br />", (from item in modelState
                                      where item.Value.Errors.Any()
                                      select item.Value.Errors[0].ErrorMessage).ToList());
    }
    public static string ToLowerFirst(this string str)
    {
        return str.Substring(0, 1).ToLower() + str.Substring(1);
    }

    public static string ToSlashDate(this int dt)
    {
        if (dt.ToString().Length >= 8)
        {
            string slashDate = "";

            string intDate = dt.ToString();
            slashDate = intDate.Substring(0, 4) + "/";
            slashDate += intDate.Substring(4, 2) + "/";
            slashDate += intDate.Substring(6, 2);
            return slashDate;
        }
        else
            return "0";

    }

    public static string ToLongSlashDate(this long dt)
    {
        if (dt.ToString().Length >= 8)
        {
            string slashDate = "";

            string intDate = dt.ToString();
            slashDate = intDate.Substring(0, 4) + "/";
            slashDate += intDate.Substring(4, 2) + "/";
            slashDate += intDate.Substring(6, 2);
            return slashDate;
        }
        else
            return "0";

    }

    public static DateTime ToPersianDate(this DateTime dt)
    {
        PersianCalendar pc = new PersianCalendar();
        int year = pc.GetYear(dt);
        int month = pc.GetMonth(dt);
        int day = pc.GetDayOfMonth(dt);
        int hour = pc.GetHour(dt);
        int min = pc.GetMinute(dt);

        return new DateTime(month, day, year, 0, 0, 0);
        //return month + "/" + day + "/" + year + " 12:0:0 AM";
    }

    public static DateTime ToMiladiDate(this DateTime dt)
    {
        PersianCalendar pc = new PersianCalendar();
        return pc.ToDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0);
    }

    public static int GetPersianDate(this int dt)
    {
        DateTime Today =  DateTime.Today;
        PersianCalendar pc = new PersianCalendar();
        string year = pc.GetYear(Today).ToString();
        string month = pc.GetMonth(Today).ToString();
        if (month.Length < 2)
        {
            month = "0" + month;
        }
        string day = pc.GetDayOfMonth(Today).ToString();
        if (day.Length < 2)
        {
            day = "0" + day;
        }

        //return new DateTime(month, day, year, 0, 0, 0);
        return int.Parse(year+month+day);
    }

    public static int GetMiladiDate(this int dt)
        {
            string time;
            int date;
            time = DateTime.Now.ToString("HHmm");
            date = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            return date;// +time;
        }


    public static string ConvertDataTime(string Start, string End)
    {
        string sDate = "";
        string eDate = "";

        sDate = Start.Substring(0, 4) + "/";
        sDate += Start.Substring(4, 2) + "/";
        sDate += Start.Substring(6, 2);

        eDate = End.Substring(0, 4) + "/";
        eDate += End.Substring(4, 2) + "/";
        eDate += End.Substring(6, 2);

        return sDate + ',' + eDate;

    }

    public static string SplitZero(this int Value)
    {
        string v = Value.ToString();
        if (v != string.Empty)
        {
            v = string.Format("{0:N0}", double.Parse(v.Replace(",", "")));
            
        }
        return v;

    }
    

    


}
