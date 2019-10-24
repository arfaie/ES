using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class NowDateRepository : IDisposable
    {
        private schoolEntities db = null;

        public NowDateRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_nowDate entity, bool autoSave = true)
        {
            try
            {
                db.tbl_nowDate.Add(entity);
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

        public bool Update( bool autoSave = true)
        {
            try
            {
                tbl_nowDate entity;
                entity = Where(x => x.idNowDate == 1).Single();
                int perToday = 0;
                int milToday = 0;
                perToday = perToday.GetPersianDate();
                milToday = milToday.GetMiladiDate();
                entity.shamsiDate = perToday.ToSlashDate();
                entity.miladiDate = milToday.ToSlashDate();

                db.tbl_nowDate.Attach(entity);
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

        public bool Delete(tbl_nowDate entity, bool autoSave = true)
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
                var entity = db.tbl_nowDate.Find(id);
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

        public tbl_nowDate Find(int id)
        {
            try
            {
                return db.tbl_nowDate.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_nowDate> Where(System.Linq.Expressions.Expression<Func<tbl_nowDate, bool>> predicate)
        {
            try
            {
                return db.tbl_nowDate.Where(predicate);
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

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_nowDate, TResult>> selector)
        {
            try
            {
                return db.tbl_nowDate.Select(selector);
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
                if (db.tbl_nowDate.Any())
                    return db.tbl_nowDate.OrderByDescending(p => p.idNowDate).First().idNowDate;
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

        ~NowDateRepository()
        {
            Dispose(false);
        }
    }
}