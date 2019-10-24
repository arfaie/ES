using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class LessonRepository : IDisposable
    {
        private schoolEntities db = null;

        public LessonRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_lessons entity, bool autoSave = true)
        {
            try
            {
                db.tbl_lessons.Add(entity);
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

        public bool Update(tbl_lessons entity, bool autoSave = true)
        {
            try
            {
                db.tbl_lessons.Attach(entity);
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

        public bool Delete(tbl_lessons entity, bool autoSave = true)
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
                var entity = db.tbl_lessons.Find(id);
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

        public tbl_lessons Find(int id)
        {
            try
            {
                return db.tbl_lessons.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_lessons> Where(System.Linq.Expressions.Expression<Func<tbl_lessons, bool>> predicate)
        {
            try
            {
                return db.tbl_lessons.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<view_Lesson> Select()
        {
            try
            {
                return db.view_Lesson.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_lessons, TResult>> selector)
        {
            try
            {
                return db.tbl_lessons.Select(selector);
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
                if (db.tbl_lessons.Any())
                    return db.tbl_lessons.OrderByDescending(p => p.idLesson).First().idLesson;
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

        ~LessonRepository()
        {
            Dispose(false);
        }
    }
}