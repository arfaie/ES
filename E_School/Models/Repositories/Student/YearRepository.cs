using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;
using System.Globalization;

namespace E_School.Models.Repositories.api
{
    public class YearRepository : IDisposable
    {
        private schoolEntities db = null;

        public YearRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_years entity, bool autoSave = true)
        {
            try
            {
                db.tbl_years.Add(entity);
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

        public bool Update(tbl_years entity, bool autoSave = true)
        {
            try
            {
                db.tbl_years.Attach(entity);
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
                var entity = db.tbl_years.Find(id);
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

        public bool Delete(tbl_years entity, bool autoSave = true)
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
                var entity = db.tbl_years.Find(id);
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

        public tbl_years Find(int id)
        {
            try
            {
                return db.tbl_years.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_years> Where(System.Linq.Expressions.Expression<Func<tbl_years, bool>> predicate)
        {
            try
            {
                return db.tbl_years.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_years> Select()
        {
            try
            {
                return db.tbl_years.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_years, TResult>> selector)
        {
            try
            {
                return db.tbl_years.Select(selector);
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
                if (db.tbl_years.Any())
                    return db.tbl_years.OrderByDescending(p => p.idYear).First().idYear;
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
        public int getThisYear()
        {
            DateTime d = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            string y = pc.GetYear(d).ToString();
            string m = pc.GetMonth(d).ToString();
            if (m.Count() == 1)
                m = "0" + m;
            string day = pc.GetDayOfMonth(d).ToString();
            if (day.Count() == 1)
                day = "0" + day;
            int date = int.Parse(y + m + day);
            int idYear = Where(x => x.yearStart <= date && x.yearEnd >= date).FirstOrDefault().idYear;
            return idYear;
        }

        ~YearRepository()
        {
            Dispose(false);
        }
    }
}