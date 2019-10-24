using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class TransectionRepository : IDisposable
    {
        private schoolEntities db = null;
        
        public TransectionRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_editTransections entity, bool autoSave = true)
        {
            try
            {
                db.tbl_editTransections.Add(entity);
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

        public bool Update(tbl_editTransections entity, bool autoSave = true)
        {
            try
            {
                db.tbl_editTransections.Attach(entity);
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

        public bool Delete(tbl_editTransections entity, bool autoSave = true)
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
                var entity = db.tbl_editTransections.Find(id);
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

        public tbl_editTransections Find(int id)
        {
            try
            {
                return db.tbl_editTransections.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_editTransections> Where(System.Linq.Expressions.Expression<Func<tbl_editTransections, bool>> predicate)
        {
            try
            {
                return db.tbl_editTransections.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        //public List<view_dataTable> newSelect()
        //{
        //    try
        //    {
        //        return db.view_dataTable.ToList();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_editTransections, TResult>> selector)
        {
            try
            {
                return db.tbl_editTransections.Select(selector);
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
                if (db.tbl_editTransections.Any())
                    return db.tbl_editTransections.OrderByDescending(p => p.idTrans).First().idTrans;
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

        ~TransectionRepository()
        {
            Dispose(false);
        }
    }
}