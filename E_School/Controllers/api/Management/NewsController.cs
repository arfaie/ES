using E_School.Models.DomainModels;
using E_School.Models.Repositories;
using E_School.Models.ViewModel.Management;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace E_School.Controllers.api.Management
{
    public class NewsController : ApiController
    {
        NewsRepository bl = new NewsRepository();
        imageRepository bl2 = new imageRepository();
        YearRepository yearRepository = new YearRepository();


        [ActionName("add")]
        [HttpPost]
        public int add([FromBody] newsModel entity)
        {
            tbl_news model = new tbl_news();
            tbl_images image;

            int id = 0;
            string name;
            id = bl.GetLastIdentity() + 1;
            model.id = id;
            model.date = entity.date;
            model.body = entity.body;
            model.receiverType = entity.receiverType;
            model.visitCount = 0;
            model.title = entity.title;


            try
            {
                for (int i = 0; i < entity.images.Count(); i++)
                {
                    image = new tbl_images();
                    image.id = bl2.GetLastIdentity() + 1;
                    image.idRow = id;
                    name = entity.images.ElementAt(i);
                    image.name = name;
                    image.type = 1;

                    bl2.Add(image);
                }

                if (bl.Add(model))
                    return id;
                else
                    return 0;
            }

            catch
            {
                return 0;
            }
        }



        [ActionName("update")]
        [HttpPost]
        public bool update([FromBody] newsModel entity)
        {

            tbl_news model = new tbl_news();
            tbl_images image;
            string name, entityName;
            bool isChanged = false, isExtra = true, isNew = true;

            try
            {
                List<tbl_images> list = bl2.Where(x => x.type == 1 && x.idRow == entity.id).ToList();


                if (list.Count() == entity.images.Count()) //تعداد عکس های خبر با تعداد عکسهایی که از قبل داشت، برابر می باشند
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        name = list.ElementAt(i).name;

                        for (int j = 0; j < entity.images.Count(); j++)
                        {
                            isChanged = false;
                            entityName = entity.images.ElementAt(j);

                            if (!name.Equals(entityName))
                            {
                                isChanged = true;
                                break;
                            }
                        }

                        if (isChanged)
                        {
                            image = new tbl_images();
                            image.id = list.ElementAt(i).id;
                            image.idRow = entity.id;
                            image.name = entity.images.ElementAt(i);
                            image.type = 1;

                            bl2.Update(image);
                        }
                    }
                }





                else if (list.Count() > entity.images.Count()) // در آپدیت جدید، تعداد عکس های کمتری نسبت به حالت قبلی دارد. پس عکس هایی که در خبر جدید نیستند را از دیتابیس حذف می کنیم
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        name = list.ElementAt(i).name;

                        for (int j = 0; j < entity.images.Count(); j++)
                        {
                            isExtra = true;
                            entityName = entity.images.ElementAt(j);

                            if (name.Equals(entityName))
                            {
                                isExtra = false;
                                break;
                            }
                        }

                        if (isExtra)
                        {
                            bl2.Delete(list.ElementAt(i).id);
                        }
                    }
                }





                else // در آپدیت جدید، تعداد عکس ها بیشتر از حالت قبلی است که در دیتابیس ثبت شده. پس عکسهای جدیدی را برای خبر اضافه می کنیم
                {
                    for (int i = 0; i < entity.images.Count(); i++)
                    {
                        name = entity.images.ElementAt(i);

                        for (int j = 0; j < list.Count(); j++)
                        {
                            isNew = true;
                            entityName = list.ElementAt(j).name;

                            if (name.Equals(entityName))
                            {
                                isNew = false;
                                break;
                            }
                        }

                        if (isNew)
                        {
                            image = new tbl_images();
                            //image = new tbl_images();
                            image.id = bl2.GetLastIdentity() + 1;
                            image.idRow = entity.id;
                            name = entity.images.ElementAt(i);
                            image.name = name;
                            image.type = 1;

                            bl2.Add(image);
                        }
                    }
                }


                model.id = entity.id;
                model.date = entity.date;
                model.body = entity.body;
                model.receiverType = entity.receiverType;
                model.visitCount = entity.visitCount;
                model.title = entity.title;

                if (bl.Update(model))
                    return true;

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
            List<tbl_images> list = bl2.Where(x => x.type == 1 && x.idRow == id).ToList();

            for (int i = 0; i < list.Count(); i++)
            {
                bl2.Delete(list.ElementAt(i).id);
            }


            if (bl.Delete(id))
                return true;
            else
                return false;
        }



        [ActionName("select")]
        [HttpGet]
        public List<newsModel> select([FromUri] int type)
        {
            List<newsModel> ls = new List<newsModel>();
            List<tbl_news> list;
            newsModel model;
            string name;


            if (type == 0) // select whole list for manager
                list = bl.Select().ToList();
            else // list for others ( all : 1  ,  teachers : 2  ,  students & parents : 3 )
                list = bl.Where(x => x.receiverType == type || x.receiverType == 1).ToList();

            for (int i = 0; i < list.Count(); i++)
            {
                model = new newsModel();

                model.id = list.ElementAt(i).id;
                model.title = list.ElementAt(i).title;
                model.body = list.ElementAt(i).body;
                model.date = list.ElementAt(i).date;
                model.visitCount = list.ElementAt(i).visitCount;
                model.receiverType = list.ElementAt(i).receiverType;

                List<tbl_images> imgList = bl2.Where(x => x.idRow == model.id && x.type == 1).ToList();

                for (int j = 0; j < imgList.Count(); j++)
                {
                    name = imgList.ElementAt(j).name;
                    model.images.Add(name);
                }

                ls.Add(model);

            }

            return ls;
        }



        [ActionName("increaseNewsReadCount")]
        [HttpGet]
        public bool increaseNewsReadCount([FromUri] int id)
        {
            try
            {
                tbl_news news = bl.Find(id);
                news.visitCount++;

                if (bl.Update(news))
                    return true;
                else
                    return false;
            }

            catch
            {
                return false;
            }
        }




        [ActionName("getAllNewsCount")]
        [HttpGet]
        public int getAllNewsCount()
        {
            int newsCount = 0;
            int today = getTodayDate();
            tbl_years year = yearRepository.Where(x => x.yearStart <= today && x.yearEnd >= today).FirstOrDefault();
            newsCount = bl.Where(x => x.date <= year.yearEnd && x.date >= year.yearStart).Count();
            return newsCount;
        }


        [HttpPost]
        [ActionName("uploadImage")] // upload images of news to server
        public string uploadImage()
        {
            int iUploadedCnt = 0;
            System.Web.HttpPostedFile hpf = null;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Uploaded/News/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                hpf = hfc[iCnt];


                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
            }

            return "Files Uploaded Successfully";


        }




        private int getTodayDate()
        {
            DateTime d = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string y = pc.GetYear(d).ToString();
            string m = pc.GetMonth(d).ToString();
            if (m.Count() == 1)
                m = "0" + m;
            string day = pc.GetDayOfMonth(d).ToString();
            if (day.Count() == 1)
                day = "0" + day;
            int date = int.Parse(y + m + day);

            return date;
        }
    }
}