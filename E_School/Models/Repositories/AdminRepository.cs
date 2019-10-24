using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class AdminRepository : IDisposable
    {
        private schoolEntities db = null;

        public AdminRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_Admin____ entity, bool autoSave = true)
        {
            try
            {
                db.tbl_Admin____.Add(entity);
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

        public bool Update(tbl_Admin____ entity, bool autoSave = true)
        {
            try
            {
                db.tbl_Admin____.Attach(entity);
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
                var entity = db.tbl_Admin____.Find(id);
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

        public bool Delete(tbl_Admin____ entity, bool autoSave = true)
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
                var entity = db.tbl_Admin____.Find(id);
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

        public tbl_Admin____ Find(int id)
        {
            try
            {
                return db.tbl_Admin____.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_Admin____> Where(System.Linq.Expressions.Expression<Func<tbl_Admin____, bool>> predicate)
        {
            try
            {
                return db.tbl_Admin____.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_Admin____> Select()
        {
            try
            {
                return db.tbl_Admin____.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_Admin____, TResult>> selector)
        {
            try
            {
                return db.tbl_Admin____.Select(selector);
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
                if (db.tbl_Admin____.Any())
                    return db.tbl_Admin____.OrderByDescending(p => p.idAdmin).First().idAdmin;
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

        ~AdminRepository()
        {
            Dispose(false);
        }
    }
}