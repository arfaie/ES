using E_School.Models.Repositories.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace E_School.Controllers.api.Student
{
    public class SettingsController
    {
        SettingsRepository bl = new SettingsRepository();

        [ActionName("getOnlineTestUrl")]
        [HttpGet]
        public string getOnlineTestUrl()
        {
            return bl.getOnlineTestUrl();
        }
    }
}