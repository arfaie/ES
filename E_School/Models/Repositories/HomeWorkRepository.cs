using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_School.Models.DomainModels;

namespace E_School.Models.Repositories
{
    public class HomeWorkRepository
    {
        private schoolEntities db = null;

        public HomeWorkRepository()
        {
            db = new schoolEntities();
        }

        public IQueryable<tbl_homeWorks> Where(System.Linq.Expressions.Expression<Func<tbl_homeWorks, bool>> predicate)
        {
            try
            {
                return db.tbl_homeWorks.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<View_homeWork> View_homeWork()
        {
            try
            {
                return db.View_homeWork.AsQueryable();
            }
            catch
            {
                return null;
            }
        }


        public IQueryable<View_HomeWorkResponse> homeWorkResponse()
        {
            try
            {
                return db.View_HomeWorkResponse.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        //public IQueryable<View_studentHomeWork> studentHomeWork(int idStudent)
        //{
        //    int idClass = db.tbl_studentRegister.OrderByDescending(z => z.idStudent == idStudent).First().idClass;
        //    try
        //    {
        //        return db.View_studentHomeWork.Where(x => x.idClass == idClass).AsQueryable();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

    }
}