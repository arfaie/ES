using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class DisciplineTypeRepository : IDisposable
    {
        private schoolEntities db = null;

        public DisciplineTypeRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_disciplineTypes entity, bool autoSave = true)
        {
            try
            {
                db.tbl_disciplineTypes.Add(entity);
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

        public bool Update(tbl_disciplineTypes entity, bool autoSave = true)
        {
            bool result=false;
            try
            {
                //if (entity.idDisType > 0)//jusy for delay
               // {
                    db.tbl_disciplineTypes.Attach(entity);
                    db.Entry(entity).State = EntityState.Modified;
                    if (autoSave)
                        result= Convert.ToBoolean(db.SaveChanges());
                    else
                        result= false;
                //}
                
            }
            catch
            {
                return false;
            }
            return result;
        }
        public bool Update(int id, bool autoSave = true)
        {
            try
            {
                var entity = db.tbl_disciplineTypes.Find(id);
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

        public bool Delete(tbl_disciplineTypes entity, bool autoSave = true)
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
                var entity = db.tbl_disciplineTypes.Find(id);
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

        public tbl_disciplineTypes Find(int id)
        {
            try
            {
                return db.tbl_disciplineTypes.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_disciplineTypes> Where(System.Linq.Expressions.Expression<Func<tbl_disciplineTypes, bool>> predicate)
        {
            try
            {
                return db.tbl_disciplineTypes.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_disciplineTypes> Select()
        {
            try
            {
                return db.tbl_disciplineTypes.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_disciplineTypes, TResult>> selector)
        {
            try
            {
                return db.tbl_disciplineTypes.Select(selector);
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
                if (db.tbl_disciplineTypes.Any())
                    return db.tbl_disciplineTypes.OrderByDescending(p => p.idDisType).First().idDisType;
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

        ~DisciplineTypeRepository()
        {
            Dispose(false);
        }
    }
}