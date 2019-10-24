using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class receivedHomeWorkModel
    {
        public int idHomeWork { get; set; }
        public int idStudent { get; set; }
        public int dateTime { get; set; }
        public String answerFile { get; set; }
        public String answerBody { get; set; }
    }
}