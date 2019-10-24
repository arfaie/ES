using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;
using E_School.Models.ViewModel;

namespace E_School.Models.Repositories
{
    public class DebtRepository : IDisposable
    {
        private schoolEntities db = null;

        public DebtRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_debts entity, bool autoSave = true)
        {
            try
            {
                db.tbl_debts.Add(entity);
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

        public bool Update(tbl_editTransections entity, bool autoSave = true)
        {
            try
            {
                db.tbl_editTransections.Add(entity);
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

        public bool Delete(tbl_debts entity, bool autoSave = true)
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
                var entity = db.tbl_debts.Find(id);
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

        public tbl_debts Find(int id)
        {
            try
            {
                return db.tbl_debts.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_debts> Where(System.Linq.Expressions.Expression<Func<tbl_debts, bool>> predicate)
        {
            try
            {
                return db.tbl_debts.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_debts> Select()
        {
            try
            {
                return db.tbl_debts.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_debts, TResult>> selector)
        {
            try
            {
                return db.tbl_debts.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public int GetLastIdentity(bool ch)//if ch==1 => add LastIdentity | if ch==0 => update LastIdentity 
        {
            try
            {
                if (ch == true)
                {
                    if (db.tbl_debts.Any())
                    {
                        return db.tbl_debts.OrderByDescending(p => p.idDebt).First().idDebt;
                    }

                }
                if (ch == false)
                {
                    if (db.tbl_editTransections.Any())
                    {
                        return db.tbl_editTransections.OrderByDescending(p => p.idTrans).First().idTrans;
                    }

                }

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

        ~DebtRepository()
        {
            Dispose(false);
        }
    }
}