using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class ClassRepository : IDisposable
    {
        private schoolEntities db = null;

        public ClassRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_classes entity, bool autoSave = true)
        {
            try
            {
                db.tbl_classes.Add(entity);
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

        public bool Update(tbl_classes entity, bool autoSave = true)
        {
            try
            {
                db.tbl_classes.Attach(entity);
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
                var entity = db.tbl_classes.Find(id);
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

        public bool Delete(tbl_classes entity, bool autoSave = true)
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
                var entity = db.tbl_classes.Find(id);
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

        public tbl_classes Find(int id)
        {
            try
            {
                return db.tbl_classes.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_classes> Where(System.Linq.Expressions.Expression<Func<tbl_classes, bool>> predicate)
        {
            try
            {
                return db.tbl_classes.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<view_Classes> Select()
        {
            try
            {
                return db.view_Classes.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_classes, TResult>> selector)
        {
            try
            {
                return db.tbl_classes.Select(selector);
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
                if (db.tbl_classes.Any())
                    return db.tbl_classes.OrderByDescending(p => p.idClass).First().idClass;
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

        ~ClassRepository()
        {
            Dispose(false);
        }
    }
}