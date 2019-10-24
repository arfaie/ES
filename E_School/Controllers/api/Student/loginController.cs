using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.Repositories.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace E_School.api.Controllers.api.Student
{
    public class loginController : ApiController
    {
        LoginRepository bl = new LoginRepository();


        [ActionName("StudentLogin")]
        [HttpGet]
        public List<View_studentInfo> StudentLogin([FromUri] int idUserType, [FromUri] string user, [FromUri] string pass)
        {
            return bl.login(idUserType, user, pass);
        }




        [ActionName("TeacherLogin")]
        [HttpGet]
        public IQueryable<tbl_teachers> TeacherLogin([FromUri] string user, [FromUri] string pass)
        {
            return bl.TeacherLogin(user, pass);
        }




        [ActionName("ipValidation")]
        [HttpGet]
        public int ipValidation()
        {
            return 1;
        }



        [ActionName("changePassword")]
        [HttpGet]
        public int changePassword([FromUri] int idUserType, [FromUri] int idUser, [FromUri] String lastPass, [FromUri] String newPass)
        {
            return bl.changePassword(idUserType, idUser, lastPass, newPass);
        }





        [ActionName("getStudentInformation")]
        [HttpGet]
        public IQueryable<View_studentInfo> getStudentInformation([FromUri] int idUser)
        {
            return bl.getStudentInformation(idUser);
        }



        [ActionName("getTeacherInformation")]
        [HttpGet]
        public IQueryable<tbl_teachers> getTeacherInformation([FromUri] int id)
        {
            return bl.getTeacherInformation(id);
        }



        [ActionName("editStudentInfo")]
        [HttpGet]
        public IQueryable<tbl_students> editStudentInfo([FromUri] int id)
        {
            return bl.editStudentInfo(id);
        }




        [ActionName("editingStudentInfo")]
        [HttpPost]
        public bool editingStudentInfo([FromBody] tbl_students entity)
        {
            return bl.editingStudentInfo(entity);
        }




        [ActionName("getTeacherInfo")]
        [HttpGet]
        public IQueryable<tbl_teachers> getTeacherInfo([FromUri] int id)
        {
            return bl.getTeacherInfo(id);
        }





        [ActionName("editingTeacherInfo")]
        [HttpPost]
        public bool editingTeacherInfo([FromBody] tbl_teachers entity)
        {
            return bl.editingTeacherInfo(entity);
        }



        [ActionName("getLastVersion")]
        [HttpGet]
        public string getLastVersion()
        {
            return bl.getLastVersion();
        }

    }
}
