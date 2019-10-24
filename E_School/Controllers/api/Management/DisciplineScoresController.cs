using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using Newtonsoft.Json;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Web.Script.Serialization;
using E_School.Models.ViewModel;
using E_School.Helpers.Utitlies;

namespace E_School.Controllers.api.Management
{
    public class DisciplineScoresController : ApiController
    {
        DataTableRepository bl = new DataTableRepository();
        schoolEntities db = new schoolEntities();
        [ActionName("Select")]
        [HttpGet]
        public HttpResponseMessage select([FromUri]int idClass, int idYear)
        {

            var Result = (from st in db.tbl_studentsDisciplines

                          join dt in db.tbl_disciplineTypes
                          on st.idDisType equals dt.idDisType

                          join s in db.tbl_students
                         on st.idStudent equals s.idStudent

                          join sr in db.tbl_studentRegister
                          on st.idStudent equals sr.idStudent

                          where sr.idClass==idClass&&sr.idYear==idYear

                          select new

                          {
                              st.idStudent,
                              st.justified,
                              dt.action,
                              dt.value,
                              s.FName,
                              s.LName

                          }).ToList();

            List<LsDisciplineScores> listModel = new List<LsDisciplineScores>();

            double value = 20;
            int id = 0;
            List<int> Lsid = new List<int>();
            LsDisciplineScores model;
            foreach (var a in Result)
            {
                model= new LsDisciplineScores();
                id = a.idStudent;
                model.id = a.idStudent;
                if (!Lsid.Contains(a.idStudent))
                {
                    foreach (var b in Result)
                    {
                        if (b.idStudent == id)
                        {
                            if (b.justified == true)
                            {
                                if (b.action == true)
                                {
                                    value += b.value;
                                }
                                else
                                {
                                    value -= b.value;
                                }

                            }
                            if (value >= 20)
                            {
                                model.Score = 20;
                            }
                            else
                            {
                                model.Score = value;
                            }
                            model.FName = b.FName;
                            model.LName = b.LName;
                        }


                    }
                    value = 20;
                    listModel.Add(model);
                    Lsid.Add(a.idStudent);
                }



            }
            StudentRegisterRepository StReg = new StudentRegisterRepository();
            var select = StReg.Select().Where(x=>x.idYear==idYear&&x.idClass==idClass);
            foreach (var c in select)
            {
                if (!Lsid.Contains(c.idStudent))
                {
                    model = new LsDisciplineScores();
                    model.Score = 20;

                    model.FName = c.FName;
                    model.LName = c.LName;
                    listModel.Add(model);
                    
                }
            }

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(listModel);

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    json.ToString()

                )
            };

        }

        [ActionName("Add")]
        [HttpPost]
        public int Add([FromBody]tbl_dataTable entity)
        {
            try
            {


                if (entity == null)
                {
                    return int.Parse("-1");
                }
                else
                {
                    entity.idDataTable = bl.GetLastIdentity() + 1;
                    if (bl.Add(entity) == false)
                    {
                        return int.Parse("0");
                    }
                    else
                    {
                        return int.Parse(entity.idDataTable.ToString());
                    }

                }

            }
            catch (Exception EX)
            {
                return int.Parse("-2");
            }
        }

        [ActionName("Update")]
        [HttpPost]
        public bool Update([FromBody]tbl_dataTable entity)
        {
            try
            {

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    Methods ob = new Methods();
                    if (ob.isEditable(entity.idYear))
                    {
                        if (bl.Update(entity))
                            return true;
                        else
                            return false;
                    }
                    return false;
                }

            }
            catch (Exception EX)
            {
                return false;
            }

        }

        [ActionName("Delete")]
        [HttpPost]
        public bool Delete([FromBody]int id)
        {
            try
            {

                if (id == null)
                {
                    return false;
                }
                else
                {
                    if (bl.Delete(id))
                        return true;
                    else
                        return false;
                }

            }
            catch (Exception EX)
            {
                return false;
            }

        }
    }
}