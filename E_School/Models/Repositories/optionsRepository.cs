using E_School.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories
{
    public class optionsRepository : IDisposable
    {
        private schoolEntities db = null;

        public optionsRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_options entity, bool autoSave = true)
        {
            try
            {
                db.tbl_options.Add(entity);
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

        public bool Update(tbl_options entity, bool autoSave = true)
        {
            try
            {
                db.tbl_options.Attach(entity);
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

        public bool Delete(tbl_options entity, bool autoSave = true)
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
                var entity = db.tbl_options.Find(id);
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

        public tbl_options Find(int id)
        {
            try
            {
                return db.tbl_options.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_options> Where(System.Linq.Expressions.Expression<Func<tbl_options, bool>> predicate)
        {
            try
            {
                return db.tbl_options.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_options> Select()
        {
            try
            {
                return db.tbl_options.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_options, TResult>> selector)
        {
            try
            {
                return db.tbl_options.Select(selector);
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
                if (db.tbl_options.Any())
                    return db.tbl_options.OrderByDescending(p => p.id).First().id;
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

        ~optionsRepository()
        {
            Dispose(false);
        }
    }
}