using E_School.Models;
using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.Repositories.api;
using E_School.Models.ViewModel.api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace E_School.api.Controllers.api.Student
{
    public class messagesController : ApiController
    {
        MessagesRepository bl = new MessagesRepository();

        [ActionName("getMsg")]//Inbox
        [HttpGet]
        public List<messageModel> getMsg([FromUri] int idReciever, [FromUri] int idRecieverType)
        {
            return bl.Messages(idReciever, idRecieverType);
        }


        [ActionName("PTSentMsg")]//OutBox
        [HttpGet]
        public List<messageModel> PTSentMsg([FromUri] int idSender, [FromUri] int idSenderType)
        {
            return bl.PTSentMsg(idSender, idSenderType);
        }



        [ActionName("setMSgRead")]
        [HttpGet]
        public Boolean setMSgRead([FromUri] Boolean isRead, [FromUri] int idMessageRecord)
        {
            if (bl.setMSgRead(isRead, idMessageRecord))
                return true;

            else
                return false;
        }



        [ActionName("getClassTeachers")]
        [HttpGet]
        public List<classTeacherModel> getClassTeachers([FromUri] int idClass)
        {

            return bl.getClassTeachers(idClass);
        }



        [ActionName("getClassStudents")]
        [HttpGet]
        public List<View_classStudents> getClassStudents([FromUri] int idClass)
        {

            return bl.getClassStudents(idClass);
        }



        [ActionName("getTeacherClasses")]
        [HttpGet]
        public List<classTeacherModel> getTeacherClasses([FromUri] int idTeacher)
        {

            return bl.getTeacherClasses(idTeacher);
        }



        [ActionName("recieveParentMessage")]
        [HttpPost]
        public Boolean recieveParentMessage([FromBody] messageRecieversModel entity)
        {
            if (bl.recieveParentMessage(entity))
            {
                return true;
            }
            else
                return false;
        }




        [ActionName("deleteSenedMsg")]
        [HttpGet]
        public Boolean deleteSenedMsg([FromUri] int idMsg)
        {
            return bl.deletSendeMsg(idMsg);
        }

        [ActionName("deleteReciveMsg")]
        [HttpGet]
        public Boolean deleteReciveMsg([FromUri] int idMsg)
        {
            return bl.deleteRciveMsg(idMsg);
        }



        [ActionName("unreadMsgCount")]
        [HttpGet]
        public int unreadMsgCount([FromUri] int idUserType, [FromUri] int idUser)
        {
            return bl.unreadMsgCount(idUserType, idUser);
        }



    }
}
