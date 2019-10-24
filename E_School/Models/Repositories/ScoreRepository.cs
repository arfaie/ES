using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;
using E_School.Models.ViewModel;

namespace E_School.Models.Repositories
{
    public class ScoreRepository : IDisposable
    {
        private schoolEntities db = null;
        
        public ScoreRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_scores entity, bool autoSave = true)
        {
            try
            {
                db.tbl_scores.Add(entity);
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

        public bool Update(tbl_scores entity, bool autoSave = true)
        {
            try
            {
                db.tbl_scores.Attach(entity);
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

        public bool Delete(tbl_scores entity, bool autoSave = true)
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
                var entity = db.tbl_scores.Find(id);
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

        public tbl_scores Find(int id)
        {
            try
            {
                return db.tbl_scores.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_scores> Where(System.Linq.Expressions.Expression<Func<tbl_scores, bool>> predicate)
        {
            try
            {
                return db.tbl_scores.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_scores> Select()
        {
            try
            {
                return db.tbl_scores.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_scores, TResult>> selector)
        {
            try
            {
                return db.tbl_scores.Select(selector);
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
                if (db.tbl_scores.Any())
                    return db.tbl_scores.OrderByDescending(p => p.idScore).First().idScore;
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

        ~ScoreRepository()
        {
            Dispose(false);
        }
    }
}