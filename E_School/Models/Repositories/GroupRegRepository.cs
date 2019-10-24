using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class GroupRegRepository : IDisposable
    {
        private schoolEntities db = null;

        public GroupRegRepository()
        {
            db = new schoolEntities();
        }
        public int Exist(int idStud, int idRegCourse)
        {
            try
            {
                var aa = Where(x => x.idStudent == idStud && x.idRegcourse == idRegCourse).FirstOrDefault();
                if (aa != null)
                {
                    return -3;
                }
            }
            catch
            {
                return -4;
            }
            return -4;
        }
        public bool Add(tbl_registrationList entity, bool autoSave = true)
        {
            try
            {
                db.tbl_registrationList.Add(entity);
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

        public bool Update(tbl_registrationList entity, bool autoSave = true)
        {
            try
            {
                db.tbl_registrationList.Attach(entity);
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
                var entity = db.tbl_registrationList.Find(id);
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

        public bool Delete(tbl_registrationList entity, bool autoSave = true)
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
                var entity = db.tbl_registrationList.Find(id);
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

        public tbl_registrationList Find(int id)
        {
            try
            {
                return db.tbl_registrationList.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_registrationList> Where(System.Linq.Expressions.Expression<Func<tbl_registrationList, bool>> predicate)
        {
            try
            {
                return db.tbl_registrationList.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_registrationList> Select()
        {
            try
            {
                return db.tbl_registrationList.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_registrationList, TResult>> selector)
        {
            try
            {
                return db.tbl_registrationList.Select(selector);
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
                if (db.tbl_registrationList.Any())
                    return db.tbl_registrationList.OrderByDescending(p => p.idRegList).First().idRegList;
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

        ~GroupRegRepository()
        {
            Dispose(false);
        }
    }
}