using E_School.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories.Student
{
    public class SettingsRepository
    {

        private schoolEntities db = null;

        public SettingsRepository()
        {
            db = new schoolEntities();
        }


        public string getOnlineTestUrl()
        {
            try
            {
                tbl_Setting tbl = db.tbl_Setting.FirstOrDefault();

                if (tbl != null)
                {
                    //return tbl.azmoonWebsite;
                    return null;
                }

                else
                return null;

            }

            catch
            {
                return null;
            }
        }
    }
}