using E_School.Models.DomainModels;
using E_School.Models.ViewModel.api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories.api
{
    public class financialRepository
    {
        private schoolEntities db = null;

        public financialRepository()
        {
            db = new schoolEntities();
        }



        public List<financialModel> studentFinancial(int idStudent)
        {
            int idPayType, idCost, idRegCourse, idPayment, today = getTodayDate();
            int idYear = db.tbl_years.Where(x => x.yearStart <= today && x.yearEnd > today).FirstOrDefault().idYear;
            List<tbl_debts> debtList = db.tbl_debts.Where(x => x.idYear == idYear && x.idStud == idStudent).OrderByDescending(x => x.idDebt).ToList();
            List<tbl_payments> payList = db.tbl_payments.Where(x => x.idYear == idYear && x.idStud == idStudent).OrderByDescending(x => x.idPay).ToList();
            tbl_editTransections editTranaction;
            List<financialModel> list = new List<financialModel>();
            financialModel model;
            tbl_costs costs;
            tbl_registrationCourses regCourse;
            Decimal debtSum = 0, paySum = 0;

            try
            {
                for (int i = 0; i < debtList.Count(); i++) // لیست بدهی های دانش آموز
                {
                    model = new financialModel();
                    model.Date = debtList.ElementAt(i).Date;
                    model.payType = "";    // داخل این حلقه، مقدار بدهی ها حساب میشوند. به همین خاطر، نوع پرداخت در این حلقه تعیین نمی شود
                    if (debtList.ElementAt(i).idRegCourse == -1)
                    {
                        costs = new tbl_costs();
                        idCost = (int)debtList.ElementAt(i).idCost;
                        costs = db.tbl_costs.Where(x => x.idCost == idCost).FirstOrDefault();
                        model.Name = costs.Name;
                        model.debtAmount = costs.Value;
                        model.des = costs.Des;
                        debtSum += costs.Value;
                    }

                    else if (debtList.ElementAt(i).idCost == -1)
                    {
                        regCourse = new tbl_registrationCourses();
                        idRegCourse = (int)debtList.ElementAt(i).idRegCourse;
                        regCourse = db.tbl_registrationCourses.Where(x => x.idRegcourse == idRegCourse).FirstOrDefault();
                        model.Name = regCourse.title;
                        model.debtAmount = regCourse.value;
                        model.des = "شهریه دوره";
                        debtSum += regCourse.value;
                    }
                    model.debtSum = debtSum;

                    list.Add(model);
                }

                for (int i = 0; i < payList.Count(); i++)  // لیست پراختی های دانش آموز
                {
                    model = new financialModel();

                    if (!payList.ElementAt(i).isOrg)
                    {
                        editTranaction = new tbl_editTransections();
                        idPayment = payList.ElementAt(i).idPay;
                        editTranaction = db.tbl_editTransections.Where(x => x.idPayment == idPayment).FirstOrDefault();
                        model.Date = editTranaction.Date;
                        model.payAmount = editTranaction.value;
                        idPayType = payList.ElementAt(i).idPayType;
                        model.payType = db.tbl_payTypes.Where(x => x.idPayType == idPayType).FirstOrDefault().payType;
                        model.Name = "پرداختی ویرایش شده";
                        model.des = editTranaction.Des;
                        paySum += editTranaction.value;
                    }

                    else
                    {
                        model.Date = payList.ElementAt(i).financialDate;
                        model.payAmount = payList.ElementAt(i).value;
                        idPayType = payList.ElementAt(i).idPayType;
                        model.payType = db.tbl_payTypes.Where(x => x.idPayType == idPayType).FirstOrDefault().payType;
                        model.des = payList.ElementAt(i).Des;
                        model.Name = "پرداختی";   // داخل این حلقه، لیست پرداختی ها پیمایش می شوند. بنابراین عنوان همه انها با عبارت "پرداختی" مشخص می شود
                        paySum += payList.ElementAt(i).value;
                    }
                    model.paySum = paySum;
                    model.debtSum = debtSum;

                    list.Add(model);
                }

                list = list.OrderByDescending(x => x.Date).ToList();

                return list;
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