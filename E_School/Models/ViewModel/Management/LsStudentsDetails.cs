using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Helpers.Utitlies
{
    public class LsStudentsDetails
    {
        public int idStudent { get; set; }
        public int studentCode { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string fatherName { get; set; }
        public bool gender { get; set; }
        public string birthDate { get; set; }
        public int idStatus { get; set; }
        public string natCode { get; set; }
        public string imgPersonal { get; set; }
        public string imgManifest { get; set; }
        public bool leftHand { get; set; }
        public string homeAddress { get; set; }
        public string studentMobile { get; set; }
        public int idMarrige { get; set; }
        public string religion { get; set; }
        public bool isActive { get; set; }
        public string studUser { get; set; }
        public string pass { get; set; }
        public string pUser { get; set; }
        public string pPass { get; set; }
        public string englishName { get; set; }
        public Nullable<int> idFatherEdu { get; set; }
        public string fJob { get; set; }
        public string fJobAddress { get; set; }
        public string fJobPhone { get; set; }
        public string fPhone { get; set; }
        public string motherName { get; set; }
        public Nullable<int> idMotherEdu { get; set; }
        public string mJob { get; set; }
        public string mJobAddress { get; set; }
        public string mJobPhone { get; set; }
        public string mPhone { get; set; }
        public string ModerEdue { get; set; }
        public string fatherEdu { get; set; }
        public string homePhone { get; set; }
    }
}