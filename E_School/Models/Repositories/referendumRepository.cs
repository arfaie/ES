using E_School.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories
{
    public class referendumRepository : IDisposable
    {
        private schoolEntities db = null;

        public referendumRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_referendums entity, bool autoSave = true)
        {
            try
            {
                db.tbl_referendums.Add(entity);
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

        public bool Update(tbl_referendums entity, bool autoSave = true)
        {
            try
            {
                db.tbl_referendums.Attach(entity);
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

        public bool Delete(tbl_referendums entity, bool autoSave = true)
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
                var entity = db.tbl_referendums.Find(id);
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

        public tbl_referendums Find(int id)
        {
            try
            {
                return db.tbl_referendums.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_referendums> Where(System.Linq.Expressions.Expression<Func<tbl_referendums, bool>> predicate)
        {
            try
            {
                return db.tbl_referendums.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_referendums> Select()
        {
            try
            {
                return db.tbl_referendums.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_referendums, TResult>> selector)
        {
            try
            {
                return db.tbl_referendums.Select(selector);
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
                if (db.tbl_referendums.Any())
                    return db.tbl_referendums.OrderByDescending(p => p.id).First().id;
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

        ~referendumRepository()
        {
            Dispose(false);
        }
    }
}