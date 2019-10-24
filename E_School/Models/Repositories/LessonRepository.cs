using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class LevelRepository : IDisposable
    {
        private schoolEntities db = null;

        public LevelRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_levels entity, bool autoSave = true)
        {
            try
            {
                db.tbl_levels.Add(entity);
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

        public bool Update(tbl_levels entity, bool autoSave = true)
        {
            try
            {
                db.tbl_levels.Attach(entity);
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

        public bool Delete(tbl_levels entity, bool autoSave = true)
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
                var entity = db.tbl_levels.Find(id);
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

        public tbl_levels Find(int id)
        {
            try
            {
                return db.tbl_levels.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_levels> Where(System.Linq.Expressions.Expression<Func<tbl_levels, bool>> predicate)
        {
            try
            {
                return db.tbl_levels.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_levels> Select()
        {
            try
            {
                return db.tbl_levels.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_levels, TResult>> selector)
        {
            try
            {
                return db.tbl_levels.Select(selector);
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
                if (db.tbl_levels.Any())
                    return db.tbl_levels.OrderByDescending(p => p.idLevel).First().idLevel;
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

        ~LevelRepository()
        {
            Dispose(false);
        }
    }
}