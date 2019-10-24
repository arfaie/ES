using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class StudentRepository : IDisposable
    {
        private schoolEntities db = null;

        public StudentRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_students entity, bool autoSave = true)
        {
            try
            {
                db.tbl_students.Add(entity);
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

        public bool Update(tbl_students entity, bool autoSave = true,bool Register=false)
        {
            try
            {
                if (Register == true)
                {
                    var select = Where(x => x.idStudent == entity.idStudent).Single();
                    select.isActive = true;
                    select.idStatus = 1;
                    entity = select;
                }
                db.tbl_students.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                if (autoSave)
                {
                    
                    return Convert.ToBoolean(db.SaveChanges());
                }

                
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
                var entity = db.tbl_students.Find(id);
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

        public bool Delete(tbl_students entity, bool autoSave = true)
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
                var entity = db.tbl_students.Find(id);
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

        public tbl_students Find(int id)
        {
            try
            {
                return db.tbl_students.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_students> Where(System.Linq.Expressions.Expression<Func<tbl_students, bool>> predicate)
        {
            try
            {
                return db.tbl_students.Where(predicate);
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

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_students, TResult>> selector)
        {
            try
            {
                return db.tbl_students.Select(selector);
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
                if (db.tbl_students.Any())
                    return db.tbl_students.OrderByDescending(p => p.idStudent).First().idStudent;
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

        ~StudentRepository()
        {
            Dispose(false);
        }
    }
}