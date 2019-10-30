using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class StudAvrageRepository : IDisposable
    {
        private schoolEntities db = null;

        public StudAvrageRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_Avrages entity, bool autoSave = true)
        {
            try
            {
                db.tbl_Avrages.Add(entity);
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

        public bool Update(tbl_Avrages entity, bool autoSave = true)
        {
            try
            {
                db.tbl_Avrages.Attach(entity);
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

        public bool Delete(tbl_Avrages entity, bool autoSave = true)
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
                var entity = db.tbl_Avrages.Find(id);
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

        public tbl_Avrages Find(int id)
        {
            try
            {
                return db.tbl_Avrages.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_Avrages> Where(System.Linq.Expressions.Expression<Func<tbl_Avrages, bool>> predicate)
        {
            try
            {
                return db.tbl_Avrages.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_Avrages> Select()
        {
            try
            {
                return db.tbl_Avrages.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_Avrages, TResult>> selector)
        {
            try
            {
                return db.tbl_Avrages.Select(selector);
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
                if (db.tbl_Avrages.Any())
                    return db.tbl_Avrages.OrderByDescending(p => p.id).First().id;
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

        ~StudAvrageRepository()
        {
            Dispose(false);
        }
    }
}