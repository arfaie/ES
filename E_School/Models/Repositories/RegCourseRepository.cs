using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;
using E_School.Models.ViewModel;

namespace E_School.Models.Repositories
{
    public class RegCourseRepository : IDisposable
    {
        private schoolEntities db = null;

        public RegCourseRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_registrationCourses entity, bool autoSave = true)
        {
            try
            {
                db.tbl_registrationCourses.Add(entity);
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

        public bool AddList(tbl_regCourseLists entity, bool autoSave = true)
        {
            try
            {
                db.tbl_regCourseLists.Add(entity);
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

        public bool AddLisit(int idRegCourse, int[] idClass)
        {
            int j = idClass.Count();

            tbl_regCourseLists tbl = new tbl_regCourseLists();
            string val = "";

            for (int i = 0; i < j; i++)
            {
                tbl = new tbl_regCourseLists();

                tbl_regCourseLists List = new tbl_regCourseLists();

                tbl.idRegCourse = idRegCourse;
                tbl.idRegCourseList = GetListLastIdentity()+1;
                tbl.idClass = idClass[i];

                if (AddList(tbl) == false)
                {
                    val += 0;
                }
            }
            if (val.Contains("0"))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool Update(tbl_registrationCourses entity, bool autoSave = true)
        {
            try
            {
                db.tbl_registrationCourses.Attach(entity);
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

        public bool Delete(tbl_registrationCourses entity, bool autoSave = true)
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
                var entity = db.tbl_registrationCourses.Find(id);
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

        public bool DeleteList(int id, bool autoSave = true)
        {
            try
            {
                var entity = db.tbl_regCourseLists.Where(x => x.idRegCourse == id).ToList();
                string val = "";
                foreach (var p in entity)
                {
                    if (entity.Count > 0)
                    {
                        db.Entry(p).State = EntityState.Deleted;
                    }

                    if (autoSave)
                    {
                        Convert.ToBoolean(db.SaveChanges());
                    }

                    else
                    {
                        val += "0";
                    }

                }
                if (val.Contains("0"))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }

        public bool DeleteStudentList(int id, bool autoSave = false)
        {
            try
            {
                var entity = db.tbl_registrationList.Where(x => x.idRegcourse == id).ToList();
                string val = "";
                foreach (var p in entity)
                {
                    if (entity.Count > 0)
                    {
                        db.Entry(p).State = EntityState.Deleted;
                    }

                    if (autoSave)
                    {
                        Convert.ToBoolean(db.SaveChanges());
                    }

                }
                if (!autoSave)
                {
                    return Convert.ToBoolean(db.SaveChanges());
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
            
        }

        public tbl_registrationCourses Find(int id)
        {
            try
            {
                return db.tbl_registrationCourses.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_registrationCourses> Where(System.Linq.Expressions.Expression<Func<tbl_registrationCourses, bool>> predicate)
        {
            try
            {
                return db.tbl_registrationCourses.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_registrationCourses> Select()
        {
            try
            {
                return db.tbl_registrationCourses.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_registrationCourses, TResult>> selector)
        {
            try
            {
                return db.tbl_registrationCourses.Select(selector);
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
                if (db.tbl_registrationCourses.Any())
                    return db.tbl_registrationCourses.OrderByDescending(p => p.idRegcourse).First().idRegcourse;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public int GetListLastIdentity()
        {
            try
            {
                if (db.tbl_regCourseLists.Any())
                    return db.tbl_regCourseLists.OrderByDescending(p => p.idRegCourseList).First().idRegCourseList;
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

        ~RegCourseRepository()
        {
            Dispose(false);
        }
    }
}