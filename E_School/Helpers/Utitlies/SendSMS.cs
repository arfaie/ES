using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace E_School.Helpers.Utitlies
{
    public class SendSMS
    {
        public int send(string Recipient, string MessageBody, int type, bool isGroup)
        {
            MelliPayamak.Send sms = new MelliPayamak.Send();
            string retval = null;
            Methods Methods = new Methods();

            //Methods.CalculateSmsLength(MessageBody);

            SettingRepository bl = new SettingRepository();
            var select = bl.Select().FirstOrDefault();
            if (select.smsCount != 0 && select.smsCount > 0)
            {

                retval = sms.SendByBaseNumber2("09144942959", "2478", MessageBody, Recipient, 3546);
                if (long.Parse(retval) > 100)
                {
                    updateSMSCount(type, false);
                }
            }
            return 1;
        }

        //public int groupSend(string Recipient, string MessageBody, int type)
        //{
        //    IList<string> Numbers = Recipient.Split(',').Reverse().ToList<string>();
        //    foreach (var a in Numbers)
        //    {
        //        send(a, MessageBody, type, true);
        //    }
        //    int numCounts = Numbers.Count();
        //    int Count = numCounts * 2;
        //    updateSMSCount()
        //    return 1;
        //}

        public void updateSMSCount(int type, bool isGroup, int Count = 0/*برای تعداد اسمس های گروهی ارسال شده*/)
        {
            Methods Methods = new Methods();
            SettingRepository bl = new SettingRepository();
            var select = bl.Select().FirstOrDefault();

            int oldCount = select.smsCount;
            int Length = 0;
            if (type == 1)//ثبت نام
            {
                Length = 4;
            }
            else//تاخیر و غیبت
            {
                Length = 2;
            }
            int newCount = oldCount - Length;
            if (isGroup)
            {
                newCount = oldCount - (2 * Count);
            }
            select.smsCount = newCount;
            bl.Update(select);

        }

        public bool SendToAllParent()
        {
            StudentRepository blStud = new StudentRepository();
            SettingRepository settingBL = new SettingRepository();
            var setting = settingBL.Select().Single();
            string schoolName = setting.schoolName.ToString().Replace("مدرسه", "");

            List<tbl_students> studs = new List<tbl_students>();
            studs = blStud.Where(x => x.idStatus == 1).ToList();
            string number;
            bool check = true;
            foreach (var a in studs)
            {
                try
                {
                    string studName = a.FName + " " + a.LName;
                    string text = schoolName + ";" + studName + ";" + a.pUser + ";" + a.pPass + ";" + a.studUser + ";" + a.pass + ";" + setting.Website;
                    number = a.fPhone;
                    send(number, text, 1, true);
                    Thread.Sleep(200);

                }
                catch
                {
                    check = false;
                }
            }
            return check;
        }
    }
}