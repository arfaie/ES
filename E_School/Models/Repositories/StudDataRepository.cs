using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class StudDataRepository   : IDisposable
    {
        private schoolEntities db = null;
        
        public StudDataRepository ()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_studentsData entity, bool autoSave = true)
        {
            try
            {
                db.tbl_studentsData.Add(entity);
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(tbl_studentsData entity, bool autoSave = true)
        {
            try
            {
                db.tbl_studentsData.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(int id, bool autoSave = true)
        {
            try
            {
                var entity = db.tbl_studentsData.Find(id);
                db.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(tbl_studentsData entity, bool autoSave = true)
        {
            try
            {
                db.Entry(entity).State = EntityState.Deleted;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id, bool autoSave = true)
        {
            try
            {
                var entity = db.tbl_studentsData.Find(id);
                db.Entry(entity).State = EntityState.Deleted;
                if (autoSave)
                    return Convert.ToBoolean(db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public tbl_studentsData Find(int id)
        {
            try
            {
                return db.tbl_studentsData.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_studentsData> Where(System.Linq.Expressions.Expression<Func<tbl_studentsData, bool>> predicate)
        {
            try
            {
                return db.tbl_studentsData.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        //public IQueryable<view_Student_Edus> Select()
        //{
        //    try
        //    {
        //        return db.view_Student_Edus.AsQueryable();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_studentsData, TResult>> selector)
        {
            try
            {
                return db.tbl_studentsData.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public int GetLastIdentity()
        {
            try
            {
                if (db.tbl_studentsData.Any())
                    return db.tbl_studentsData.OrderByDescending(p => p.idStudData).First().idStudData;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public int Save()
        {
            try
            {
                return db.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }

        ~StudDataRepository()
        {
            Dispose(false);
        }
    }
}