using E_School.Models;
using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.Repositories.api;
using E_School.Models.ViewModel.api;
using E_School.Models.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_School.api.Controllers.api.Student
{
    public class disciplineController : ApiController
    {
        DisciplineRepository bl = new DisciplineRepository();



        [ActionName("getClassDisciplines")]
        [HttpGet]
        public List<disciplineModel> getClassDisciplines([FromUri] int idClass)
        {
            return bl.getClassDisciplines(idClass);
        }



        [ActionName("getDiscipTypes")]
        [HttpGet]
        public List<disciplineModel> getClassDisciplines()
        {
            return bl.getDiscipTypes();
        }



        [ActionName("getDiscipline")]
        [HttpPost]
        public Boolean getDiscipline( [FromBody] getDiscipModel entity)
        {
            return bl.getDiscipline(entity);
        }
    }
}
