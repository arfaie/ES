using E_School.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories.api
{
    public class RegisterRepository
    {
         private schoolEntities db = null;

         public RegisterRepository()
        {
            db = new schoolEntities();
        }

         public String studentRegister(tbl_students entity)
         {
             string natCode;
             int idStudent;
             List<tbl_students> ls = db.tbl_students.ToList();
             tbl_students tbl = new tbl_students();

             try
             {
                 for (int i = 0; i < ls.Count(); i++)
                 {
                     natCode = ls.ElementAt(i).natCode;
                     if (natCode.Equals(entity.natCode))
                         return "tekrari";
                 }


                 Random rnd = new Random();
                 int pass = rnd.Next(100000, 1000000);

                 if (db.tbl_students.OrderByDescending(x => x.idStudent).Count() > 0)
                     idStudent = db.tbl_students.OrderByDescending(p => p.idStudent).First().idStudent;
                 else
                     idStudent = 0;


                 tbl.idStudent = idStudent + 1;
                 tbl.studentCode = 1;
                 tbl.FName = entity.FName;
                 tbl.LName = entity.LName;
                 tbl.fatherName = entity.fatherName;
                 tbl.gender = entity.gender;
                 tbl.birthDate = entity.birthDate;
                 tbl.idStatus = 6;
                 tbl.natCode = entity.natCode;
                 tbl.imgManifest = entity.imgManifest;

                 if (!entity.imgPersonal.Equals(""))
                     tbl.imgPersonal = entity.imgPersonal;

                 tbl.leftHand = entity.leftHand;
                 tbl.homeAddress = entity.homeAddress;

                 if (!entity.studentMobile.Equals(""))
                     tbl.studentMobile = entity.studentMobile;

                 tbl.idMarrige = entity.idMarrige;
                 tbl.religion = entity.religion;
                 tbl.isActive = false;
                 tbl.studUser = entity.natCode.ToString();
                 tbl.pass = pass.ToString();
                 tbl.pUser = entity.natCode.ToString();

                 int parentPass = rnd.Next(100000, 1000000);

                 tbl.pPass = parentPass.ToString();

                 if (entity.idFatherEdu != 0)
                     tbl.idFatherEdu = entity.idFatherEdu;

                 if (!entity.fJob.Equals(""))
                     tbl.fJob = entity.fJob;

                 if (!entity.fJobAddress.Equals(""))
                     tbl.fJobAddress = entity.fJobAddress;

                 if (!entity.fJobPhone.Equals(""))
                     tbl.fJobPhone = entity.fJobPhone;

                 if (!entity.motherName.Equals(""))
                     tbl.motherName = entity.motherName;

                 if (entity.idMotherEdu != 0)
                     tbl.idMotherEdu = entity.idMotherEdu;

                 if (!entity.mJob.Equals(""))
                     tbl.mJob = entity.mJob;

                 if (!entity.mJobAddress.Equals(""))
                     tbl.mJobAddress = entity.mJobAddress;

                 if (!entity.mJobPhone.Equals(""))
                     tbl.mJobPhone = entity.mJobPhone;

                 if (!entity.mPhone.Equals(""))
                     tbl.mPhone = entity.mPhone;


                 tbl.fPhone = entity.fPhone;




                 db.tbl_students.Add(tbl);
                 db.SaveChanges();
                 string id = (idStudent + 1).ToString();
                 return id;
             }

             catch
             {
                 return "false";
             }
         }
    }
}