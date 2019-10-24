using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class SettingRepository : IDisposable
    {
        private schoolEntities db = null;

        public SettingRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_Setting entity, bool autoSave = true)
        {
            try
            {
                db.tbl_Setting.Add(entity);
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

        public bool Update(tbl_Setting entity, bool autoSave = true)
        {
            try
            {
                db.tbl_Setting.Attach(entity);
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

        public bool Delete(tbl_Setting entity, bool autoSave = true)
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
                var entity = db.tbl_Setting.Find(id);
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

        public tbl_Setting Find(int id)
        {
            try
            {
                return db.tbl_Setting.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_Setting> Where(System.Linq.Expressions.Expression<Func<tbl_Setting, bool>> predicate)
        {
            try
            {
                return db.tbl_Setting.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_Setting> Select()
        {
            try
            {
                return db.tbl_Setting.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_Setting, TResult>> selector)
        {
            try
            {
                return db.tbl_Setting.Select(selector);
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
                if (db.tbl_Setting.Any())
                    return db.tbl_Setting.OrderByDescending(p => p.idSetting).First().idSetting;
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

        ~SettingRepository()
        {
            Dispose(false);
        }
    }
}