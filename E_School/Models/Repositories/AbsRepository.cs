using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;
using E_School.Models.ViewModel;

namespace E_School.Models.Repositories
{
    public class AbsRepository : IDisposable
    {
        private schoolEntities db = null;
        
        public AbsRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_absentation entity, bool autoSave = true)
        {
            try
            {
                db.tbl_absentation.Add(entity);
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

        public bool Update(tbl_absentation entity, bool autoSave = true)
        {
            try
            {
                db.tbl_absentation.Attach(entity);
                db.Entry(entity).State =EntityState.Modified;
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

        public bool Delete(tbl_absentation entity, bool autoSave = true)
        {
            try
            {
                db.Entry(entity).State =EntityState.Deleted;
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
                var entity = db.tbl_absentation.Find(id);
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

        public tbl_absentation Find(int id)
        {
            try
            {
                return db.tbl_absentation.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_absentation> Where(System.Linq.Expressions.Expression<Func<tbl_absentation, bool>> predicate)
        {
            try
            {
                return db.tbl_absentation.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_absentation> Select()
        {
            try
            {
                return db.tbl_absentation.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_absentation, TResult>> selector)
        {
            try
            {
                return db.tbl_absentation.Select(selector);
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
                if (db.tbl_absentation.Any())
                    return db.tbl_absentation.OrderByDescending(p => p.idAbsentation).First().idAbsentation;
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

        ~AbsRepository()
        {
            Dispose(false);
        }
    }
}