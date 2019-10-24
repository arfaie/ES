using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class DescriptiveScoreRepository : IDisposable
    {
        private schoolEntities db = null;

        public DescriptiveScoreRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_descriptiveScores entity, bool autoSave = true)
        {
            try
            {
                db.tbl_descriptiveScores.Add(entity);
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

        public bool Update(tbl_descriptiveScores entity, bool autoSave = true)
        {
            try
            {
                db.tbl_descriptiveScores.Attach(entity);
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

        public bool Delete(tbl_descriptiveScores entity, bool autoSave = true)
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
                var entity = db.tbl_descriptiveScores.Find(id);
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

        public tbl_descriptiveScores Find(int id)
        {
            try
            {
                return db.tbl_descriptiveScores.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_descriptiveScores> Where(System.Linq.Expressions.Expression<Func<tbl_descriptiveScores, bool>> predicate)
        {
            try
            {
                return db.tbl_descriptiveScores.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_descriptiveScores> Select()
        {
            try
            {
                return db.tbl_descriptiveScores.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_descriptiveScores, TResult>> selector)
        {
            try
            {
                return db.tbl_descriptiveScores.Select(selector);
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
                if (db.tbl_descriptiveScores.Any())
                    return db.tbl_descriptiveScores.OrderByDescending(p => p.idDescriptiveScore).First().idDescriptiveScore;
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

        ~DescriptiveScoreRepository()
        {
            Dispose(false);
        }
    }
}