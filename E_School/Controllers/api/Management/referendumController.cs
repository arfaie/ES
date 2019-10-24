using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.ViewModel.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace E_School.Controllers.api.Management
{
    public class referendumController : ApiController
    {
        referendumRepository bl = new referendumRepository();



        [ActionName("select")] //return referendum to teachers, parents and student
        [HttpGet]
        public List<referendumModel> select([FromUri] int date)
        {
            answerRepository bl2 = new answerRepository();
            optionsRepository bl3 = new optionsRepository();


            referendumModel model;
            List<referendumModel> ls = new List<referendumModel>();
            List<tbl_answers> list = new List<tbl_answers>();


            int idOptions, res1 = 0, res2 = 0, res3 = 0, res4 = 0, totalRes = 0;


            try
            {
                List<tbl_referendums> rfr = bl.Where(x => x.endDate >= date && x.startDate <= date && x.Visibility == true).ToList();
                for (int i = 0; i < rfr.Count(); i++)
                {
                    model = new referendumModel();
                    idOptions = rfr.ElementAt(i).idOption;
                    model.id = rfr.ElementAt(i).id;
                    model.question = rfr.ElementAt(i).Q;
                    model.startDate = rfr.ElementAt(i).startDate;
                    model.endDate = rfr.ElementAt(i).endDate;
                    tbl_options opt = bl3.Where(x => x.id == idOptions).FirstOrDefault();

                    model.op1 = opt.O1;
                    model.op2 = opt.O2;
                    model.op3 = opt.O3;
                    model.op4 = opt.O4;


                    list = bl2.Where(x => x.idRef == model.id).ToList();

                    for (int j = 0; j < list.Count(); j++)
                    {
                        if (list.ElementAt(j).userAnswer == 1)
                            res1++;

                        else if (list.ElementAt(j).userAnswer == 2)
                            res2++;

                        else if (list.ElementAt(j).userAnswer == 3)
                            res3++;

                        else if (list.ElementAt(j).userAnswer == 4)
                            res4++;


                        totalRes++;
                    }


                    if (totalRes != 0)
                    {
                        model.res1 = ((float)res1 / (float)totalRes) * 100;
                        model.res2 = ((float)res2 / (float)totalRes) * 100;
                        model.res3 = ((float)res3 / (float)totalRes) * 100;
                        model.res4 = ((float)res4 / (float)totalRes) * 100;
                    }

                    ls.Add(model);
                }

                return ls;

            }

            catch
            {
                return null;
            }

        }





        [ActionName("selectManager")] //return referendum to manager
        [HttpGet]
        public List<referendumModel> selectManager()
        {
            answerRepository bl2 = new answerRepository();
            optionsRepository bl3 = new optionsRepository();


            referendumModel model;
            List<referendumModel> ls = new List<referendumModel>();
            List<tbl_answers> list = new List<tbl_answers>();


            int idOptions, res1 = 0, res2 = 0, res3 = 0, res4 = 0, totalRes = 0;


            try
            {
                List<tbl_referendums> rfr = bl.Select().ToList();
                for (int i = 0; i < rfr.Count(); i++)
                {
                    res1 = 0;
                    res2 = 0;
                    res3 = 0;
                    res4 = 0;
                    totalRes = 0;
                    model = new referendumModel();
                    idOptions = rfr.ElementAt(i).idOption;
                    model.id = rfr.ElementAt(i).id;
                    model.question = rfr.ElementAt(i).Q;
                    model.visibility = rfr.ElementAt(i).Visibility;
                    model.endDate = rfr.ElementAt(i).endDate;
                    model.startDate = rfr.ElementAt(i).startDate;
                    tbl_options opt = bl3.Where(x => x.id == idOptions).FirstOrDefault();
                    model.op1 = opt.O1;
                    model.op2 = opt.O2;
                    model.op3 = opt.O3;
                    model.op4 = opt.O4;


                    list = bl2.Where(x => x.idRef == model.id).ToList();

                    for (int j = 0; j < list.Count(); j++)
                    {
                        if (list.ElementAt(j).userAnswer == 1)
                            res1++;

                        else if (list.ElementAt(j).userAnswer == 2)
                            res2++;

                        else if (list.ElementAt(j).userAnswer == 3)
                            res3++;

                        else if (list.ElementAt(j).userAnswer == 4)
                            res4++;


                        totalRes++;
                    }


                    if (totalRes != 0)
                    {
                        model.res1 = ((float)res1 / (float)totalRes) * 100;
                        model.res2 = ((float)res2 / (float)totalRes) * 100;
                        model.res3 = ((float)res3 / (float)totalRes) * 100;
                        model.res4 = ((float)res4 / (float)totalRes) * 100;
                    }

                    ls.Add(model);
                }

                return ls;

            }

            catch
            {
                return null;
            }

        }





        [ActionName("addReferendum")]
        [HttpPost]
        public int addReferendum([FromBody] addReferendumModel entity)
        {
            int id = 0, idOption = 0;
            tbl_referendums model = new tbl_referendums();
            tbl_referendums model2 = new tbl_referendums();
            tbl_options opt;
            optionsRepository bl2 = new optionsRepository();
            

            try
            {
                id = bl.GetLastIdentity();

                if (id != 0)
                {
                    model2 = bl.Find(id);
                    model2.Visibility = false;
                    if (bl.Update(model2))
                    {
                        opt = new tbl_options();
                        idOption = bl2.GetLastIdentity() + 1;
                        opt.O1 = entity.op1;
                        opt.O2 = entity.op2;
                        opt.O3 = entity.op3;
                        opt.O4 = entity.op4;
                        opt.id = idOption;

                        if (bl2.Add(opt))
                        {
                            model.id = id + 1;
                            model.Q = entity.Q;
                            model.startDate = entity.startDate;
                            model.endDate = entity.endDate;
                            model.Visibility = true;
                            model.idOption = idOption;

                            if (bl.Add(model))
                                return model.id;
                            else
                                return 0;

                        }


                        else
                            return 0;
                    }

                    else
                        return 0;
                }
                


                else
                {
                    opt = new tbl_options();
                    idOption = bl2.GetLastIdentity() + 1;
                    opt.O1 = entity.op1;
                    opt.O2 = entity.op2;
                    opt.O3 = entity.op3;
                    opt.O4 = entity.op4;
                    opt.id = idOption;

                    if (bl2.Add(opt))
                    {
                        model.id = id + 1;
                        model.Q = entity.Q;
                        model.startDate = entity.startDate;
                        model.endDate = entity.endDate;
                        model.Visibility = true;
                        model.idOption = idOption;

                        if (bl.Add(model))
                            return model.id;
                        else
                            return 0;

                    }


                    else
                        return 0;
                }
                


            }

            catch
            {
                return 0;
            }
        }




        [ActionName("update")]
        [HttpPost]
        public bool update([FromBody] addReferendumModel entity)
        {
            tbl_referendums model = new tbl_referendums();
            tbl_options opt = new tbl_options();
            optionsRepository bl2 = new optionsRepository();

            try
            {
                model = bl.Find(entity.id);
                
                opt = bl2.Find(model.idOption);
                opt.O1 = entity.op1;
                opt.O2 = entity.op2;
                opt.O3 = entity.op3;
                opt.O4 = entity.op4;
                

                if (bl2.Update(opt))
                {

                    
                    model.Q = entity.Q;
                    model.startDate = entity.startDate;
                    model.endDate = entity.endDate;
                    model.Visibility = true;

                    if (bl.Update(model))
                        return true;
                    else
                        return false;

                }


                else
                    return false;


            }

            catch
            {
                return false;
            }
        }




        [ActionName("delete")]
        [HttpPost]
        public bool delete([FromBody] int id)
        {
            optionsRepository bl2 = new optionsRepository();
            try
            {
                int idOption = bl.Find(id).idOption;
                if (bl2.Delete(idOption))
                {
                    if (bl.Delete(id))
                        return true;
                    else
                        return false;
                }

                else
                    return false;
            }

            catch
            {
                return false;
            }
            
        }



        [ActionName("vote")]
        [HttpGet]
        public int vote([FromUri] int idUser, [FromUri] int userType, [FromUri] int idRef, [FromUri] int userAnswer)
        {
            answerRepository bl2 = new answerRepository();
            tbl_answers answer = new tbl_answers();
            

            try
            {

                if (bl2.Where(x => x.userType == userType && x.idUser == idUser && x.idRef == idRef).Any()) //user voted before
                    return -1;

                else
                {
                    answer.id = bl2.GetLastIdentity() + 1;
                    answer.idRef = idRef;
                    answer.idUser = idUser;
                    answer.userType = userType;
                    answer.userAnswer = userAnswer;

                    if (bl2.Add(answer))
                        return 1;
                    else
                        return 0;
                }
            }

            catch
            {
                return 0;
            }
        }



        [ActionName("checkNewRef")]
        [HttpGet]
        public bool checkNewRef([FromUri] int idUser, [FromUri] int userType, [FromUri] int date)
        {
            answerRepository bl2 = new answerRepository();
            tbl_referendums refe = null;

            try
            {
                refe = bl.Where(x => x.startDate <= date && x.endDate >= date && x.Visibility == true).FirstOrDefault();
                if (refe != null)
                {
                    if (bl2.Where(x => x.idRef == refe.id && x.idUser == idUser && x.userType == userType).Any())
                        return false;
                    else
                        return true;
                }

                else
                    return false;
            }

            catch
            {
                return false;
            }
        }

    }
}