using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class CostRepository : IDisposable
    {
        private schoolEntities db = null;

        public CostRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_costs entity, bool autoSave = true)
        {
            try
            {
                db.tbl_costs.Add(entity);
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

        public bool Update(tbl_costs entity, bool autoSave = true)
        {
            try
            {
                db.tbl_costs.Attach(entity);
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
                var entity = db.tbl_costs.Find(id);
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

        public bool Delete(tbl_costs entity, bool autoSave = true)
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
                var entity = db.tbl_costs.Find(id);
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

        public tbl_costs Find(int id)
        {
            try
            {
                return db.tbl_costs.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_costs> Where(System.Linq.Expressions.Expression<Func<tbl_costs, bool>> predicate)
        {
            try
            {
                return db.tbl_costs.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_costs> Select()
        {
            try
            {
                return db.tbl_costs.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_costs, TResult>> selector)
        {
            try
            {
                return db.tbl_costs.Select(selector);
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
                if (db.tbl_costs.Any())
                    return db.tbl_costs.OrderByDescending(p => p.idCost).First().idCost;
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

        ~CostRepository()
        {
            Dispose(false);
        }
    }
}