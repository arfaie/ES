using E_School.Models;
using E_School.Models.Repositories;
using E_School.Models.Repositories.api;
using E_School.Models.ViewModel.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_School.api.Controllers.api.Student
{
    public class financilaController : ApiController
    {
        financialRepository bl = new financialRepository();




        [ActionName("studentFinancial")]
        [HttpGet]
        public List<financialModel> studentFinancial([FromUri] int idStudent)
        {
            return bl.studentFinancial(idStudent);
        }


    }
}
