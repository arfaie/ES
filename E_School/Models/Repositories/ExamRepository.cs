using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class ExamRepository : IDisposable
    {
        private schoolEntities db = null;

        public ExamRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_exams entity, bool autoSave = true)
        {
            try
            {
                db.tbl_exams.Add(entity);
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

        public bool Update(tbl_exams entity, bool autoSave = true)
        {
            try
            {
                db.tbl_exams.Attach(entity);
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
                var entity = db.tbl_exams.Find(id);
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

        public bool Delete(tbl_exams entity, bool autoSave = true)
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
                var entity = db.tbl_exams.Find(id);
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

        public tbl_exams Find(int id)
        {
            try
            {
                return db.tbl_exams.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_exams> Where(System.Linq.Expressions.Expression<Func<tbl_exams, bool>> predicate)
        {
            try
            {
                return db.tbl_exams.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_exams> Select()
        {
            try
            {
                return db.tbl_exams.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_exams, TResult>> selector)
        {
            try
            {
                return db.tbl_exams.Select(selector);
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
                if (db.tbl_exams.Any())
                    return db.tbl_exams.OrderByDescending(p => p.idExam).First().idExam;
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

        ~ExamRepository()
        {
            Dispose(false);
        }
    }
}