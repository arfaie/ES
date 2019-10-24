using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class StudDisciplineRepository : IDisposable
    {
        private schoolEntities db = null;

        public StudDisciplineRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_studentsDisciplines entity, bool autoSave = true)
        {
            try
            {
                db.tbl_studentsDisciplines.Add(entity);
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

        public bool Update(tbl_studentsDisciplines entity, bool autoSave = true)
        {
            try
            {
                db.tbl_studentsDisciplines.Attach(entity);
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
                var entity = db.tbl_studentsDisciplines.Find(id);
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

        public bool Delete(tbl_studentsDisciplines entity, bool autoSave = true)
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
                var entity = db.tbl_studentsDisciplines.Find(id);
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

        public tbl_studentsDisciplines Find(int id)
        {
            try
            {
                return db.tbl_studentsDisciplines.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_studentsDisciplines> Where(System.Linq.Expressions.Expression<Func<tbl_studentsDisciplines, bool>> predicate)
        {
            try
            {
                return db.tbl_studentsDisciplines.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_studentsDisciplines> Select()
        {
            try
            {
                return db.tbl_studentsDisciplines.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_studentsDisciplines, TResult>> selector)
        {
            try
            {
                return db.tbl_studentsDisciplines.Select(selector);
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
                if (db.tbl_studentsDisciplines.Any())
                    return db.tbl_studentsDisciplines.OrderByDescending(p => p.idStudentDisciplines).First().idStudentDisciplines;
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

        ~StudDisciplineRepository()
        {
            Dispose(false);
        }
    }
}