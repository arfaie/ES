using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class PeymentRepository : IDisposable
    {
        private schoolEntities db = null;

        public PeymentRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_payments entity, bool autoSave = true)
        {
            try
            {
                db.tbl_payments.Add(entity);
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
                var Transections = db.tbl_editTransections.Where(x => x.idPayment == entity.idPayment);
                var Payment = db.tbl_payments.Where(x => x.idPay == entity.idPayment);
                foreach (var a in Transections.ToList())
                {
                    a.isOrg = false;
                    try
                    {
                        db.tbl_editTransections.Attach(a);
                        db.Entry(a).State = EntityState.Modified;
                        //Convert.ToBoolean(db.SaveChanges());
                        db.SaveChanges();
                    }
                    catch
                    {
                        return false;
                    }
                }
                foreach (var a in Payment.ToList())
                {
                    a.isOrg = false;
                    try
                    {
                        db.tbl_payments.Attach(a);
                        db.Entry(a).State = EntityState.Modified;
                       // Convert.ToBoolean(db.SaveChanges());
                        db.SaveChanges();
                    }
                    catch
                    {
                        return false;
                    }
                }

                db.tbl_editTransections.Add(entity);
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

        public bool Delete(tbl_payments entity, bool autoSave = true)
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
                var entity = db.tbl_payments.Find(id);
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

        public tbl_payments Find(int id)
        {
            try
            {
                return db.tbl_payments.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_payments> Where(System.Linq.Expressions.Expression<Func<tbl_payments, bool>> predicate)
        {
            try
            {
                return db.tbl_payments.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_editTransections> WhereEdid(System.Linq.Expressions.Expression<Func<tbl_editTransections, bool>> predicate)
        {
            try
            {
                return db.tbl_editTransections.Where(predicate);
            }
            catch
            {
                return null;
            }
        }


        public IQueryable<tbl_payments> Select()
        {
            try
            {
                return db.tbl_payments.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_payments, TResult>> selector)
        {
            try
            {
                return db.tbl_payments.Select(selector);
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
                    if (db.tbl_payments.Any())
                    {
                        return db.tbl_payments.OrderByDescending(p => p.idPay).First().idPay;
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

        ~PeymentRepository()
        {
            Dispose(false);
        }
    }
}