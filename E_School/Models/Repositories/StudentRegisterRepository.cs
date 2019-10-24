using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class StudentRegisterRepository : IDisposable
    {
        private schoolEntities db = null;

        public StudentRegisterRepository()
        {
            db = new schoolEntities();
        }
        
        public bool Add(tbl_studentRegister entity, bool autoSave = true)
        {
            try
            {
                
                db.tbl_studentRegister.Add(entity);
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

        public bool Update(tbl_studentRegister entity, bool autoSave = true)
        {
            try
            {
                db.tbl_studentRegister.Attach(entity);
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

        public bool Delete(tbl_studentRegister entity, bool autoSave = true)
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
                var entity = db.tbl_studentRegister.Find(id);
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

        public tbl_studentRegister Find(int id)
        {
            try
            {
                return db.tbl_studentRegister.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<view_studentRegister> Where(System.Linq.Expressions.Expression<Func<view_studentRegister, bool>> predicate)
        {
            try
            {
                return db.view_studentRegister.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<view_studentRegister> Select()
        {
            try
            {
                return db.view_studentRegister.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public int Exist(int idStud,int idYear,int idLevel)
        {
            try
            {
                var aa =Where(x => x.idStudent == idStud && x.idYear == idYear && x.idLevel == idLevel).FirstOrDefault();
                if (aa!=null){
                    return -3;
                }
            }
            catch
            {
                return -4;
            }
            return -4;
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_studentRegister, TResult>> selector)
        {
            try
            {
                return db.tbl_studentRegister.Select(selector);
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
                if (db.tbl_studentRegister.Any())
                    return db.tbl_studentRegister.OrderByDescending(p => p.idStudReg).First().idStudReg;
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

        ~StudentRegisterRepository()
        {
            Dispose(false);
        }
    }
}