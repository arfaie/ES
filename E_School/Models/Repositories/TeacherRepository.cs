using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class TeacherRepository : IDisposable
    {
        private schoolEntities db = null;
        
        public TeacherRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_teachers entity, bool autoSave = true)
        {
            try
            {
                db.tbl_teachers.Add(entity);
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

        public bool Update(tbl_teachers entity, bool autoSave = true)
        {
            try
            {
                db.tbl_teachers.Attach(entity);
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
                var entity = db.tbl_teachers.Find(id);
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

        public bool Delete(tbl_teachers entity, bool autoSave = true)
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
                var entity = db.tbl_teachers.Find(id);
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

        public tbl_teachers Find(int id)
        {
            try
            {
                return db.tbl_teachers.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_teachers> Where(System.Linq.Expressions.Expression<Func<tbl_teachers, bool>> predicate)
        {
            try
            {
                return db.tbl_teachers.Where(predicate);
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

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_teachers, TResult>> selector)
        {
            try
            {
                return db.tbl_teachers.Select(selector);
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
                if (db.tbl_teachers.Any())
                    return db.tbl_teachers.OrderByDescending(p => p.idTeacher).First().idTeacher;
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

        ~TeacherRepository()
        {
            Dispose(false);
        }
    }
}