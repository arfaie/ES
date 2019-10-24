using E_School.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace E_School.Models.Repositories
{
    public class imageRepository : IDisposable
    {
        private schoolEntities db = null;

        public imageRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_images entity, bool autoSave = true)
        {
            try
            {
                db.tbl_images.Add(entity);
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

        public bool Update(tbl_images entity, bool autoSave = true)
        {
            try
            {
                var v = db.tbl_images.Find(entity.id);
                v.name = entity.name;
                db.Entry(v).State = EntityState.Modified;

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

        public bool Delete(tbl_images entity, bool autoSave = true)
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
                var entity = db.tbl_images.Find(id);
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

        public tbl_images Find(int id)
        {
            try
            {
                return db.tbl_images.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_images> Where(System.Linq.Expressions.Expression<Func<tbl_images, bool>> predicate)
        {
            try
            {
                return db.tbl_images.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_images> Select()
        {
            try
            {
                return db.tbl_images.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_images, TResult>> selector)
        {
            try
            {
                return db.tbl_images.Select(selector);
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
                if (db.tbl_images.Any())
                    return db.tbl_images.OrderByDescending(p => p.id).First().id;
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

        ~imageRepository()
        {
            Dispose(false);
        }
    }
}