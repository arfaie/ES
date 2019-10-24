using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories.api
{
    public class TermRepository : IDisposable
    {
        private schoolEntities db = null;

        public TermRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_terms entity, bool autoSave = true)
        {
            try
            {
                db.tbl_terms.Add(entity);
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

        public bool Update(tbl_terms entity, bool autoSave = true)
        {
            try
            {
                db.tbl_terms.Attach(entity);
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

        public bool Delete(tbl_terms entity, bool autoSave = true)
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
                var entity = db.tbl_terms.Find(id);
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

        public tbl_terms Find(int id)
        {
            try
            {
                return db.tbl_terms.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_terms> Where(System.Linq.Expressions.Expression<Func<tbl_terms, bool>> predicate)
        {
            try
            {
                return db.tbl_terms.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<view_Terms> Select()
        {
            try
            {
                return db.view_Terms.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_terms, TResult>> selector)
        {
            try
            {
                return db.tbl_terms.Select(selector);
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
                if (db.tbl_terms.Any())
                    return db.tbl_terms.OrderByDescending(p => p.idTerm).First().idTerm;
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

        ~TermRepository()
        {
            Dispose(false);
        }
    }
}