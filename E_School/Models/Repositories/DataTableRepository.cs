using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_School.Models.DomainModels;
using System.Data;

namespace E_School.Models.Repositories
{
    public class DataTableRepository : IDisposable
    {
        private schoolEntities db = null;

        public DataTableRepository()
        {
            db = new schoolEntities();
        }

        public bool Add(tbl_dataTable entity, bool autoSave = true)
        {
            try
            {
                db.tbl_dataTable.Add(entity);
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

        public bool Update(tbl_dataTable entity, bool autoSave = true)
        {
            try
            {
                db.tbl_dataTable.Attach(entity);
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

        public bool Delete(tbl_dataTable entity, bool autoSave = true)
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
                var entity = db.tbl_dataTable.Find(id);
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

        public tbl_dataTable Find(int id)
        {
            try
            {
                return db.tbl_dataTable.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<tbl_dataTable> Where(System.Linq.Expressions.Expression<Func<tbl_dataTable, bool>> predicate)
        {
            try
            {
                return db.tbl_dataTable.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        //public List<view_dataTable> newSelect()
        //{
        //    try
        //    {
        //        return db.view_dataTable.ToList();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<tbl_dataTable, TResult>> selector)
        {
            try
            {
                return db.tbl_dataTable.Select(selector);
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
                if (db.tbl_dataTable.Any())
                    return db.tbl_dataTable.OrderByDescending(p => p.idDataTable).First().idDataTable;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public IQueryable Join(int idClass, int idYear)
        {

            var Result = (from dt in db.tbl_dataTable
                          join days in db.tbl_days
                          on dt.idDay equals days.idDay

                          join c in db.tbl_classes
                          on dt.idClass equals c.idClass
                          where c.idClass == idClass

                          join y in db.tbl_years
                          on dt.idYear equals y.idYear
                          where y.idYear == idYear

                          join bell in db.tbl_bells
                          on dt.idBell equals bell.idBells

                          join l in db.tbl_lessons
                          on dt.idLesson equals l.idLesson

                          join t in db.tbl_teachers
                          on dt.idTeacher equals t.idTeacher

                          select new

                          {
                              dt.bellEnd,
                              dt.bellStart,
                              dt.idBell,
                              dt.idClass,
                              dt.idDataTable,
                              dt.idDay,
                              dt.idLesson,
                              dt.idLesson2,
                              dt.idTeacher,
                              dt.idTeacher2,
                              dt.idYear,
                              dt.isTak,
                              dt.takStart,
                              dt.takEnd,
                              days.dayName,
                              bell.BellName,
                              y.yearName,
                              l.lessonName,
                              t.FName,
                              t.LName,
                              c.className

                          }).ToList().Select(dt => new
                          {
                              End = dt.bellEnd.Hours + ":" + dt.bellEnd.Minutes,
                              Start = dt.bellStart.Hours + ":" + dt.bellStart.Minutes,
                              dt.idBell,
                              dt.idClass,
                              dt.idDataTable,
                              dt.idDay,
                              dt.idLesson,
                              dt.idLesson2,
                              dt.idTeacher,
                              dt.idTeacher2,
                              dt.idYear,
                              dt.isTak,
                              TakStatr = dt.takStart.Value.Hours + ":" + dt.takStart.Value.Minutes,
                              TakEnd = dt.takEnd.Value.Hours + ":" + dt.takEnd.Value.Minutes,
                              dt.dayName,
                              dt.BellName,
                              dt.yearName,
                              dt.lessonName,
                              teacheName = dt.FName + " " + dt.LName,
                              dt.className


                          })
                                 .ToList();

            var Result2 = (from r in Result
                           join l in db.tbl_lessons
                           on r.idLesson2 equals l.idLesson
                           where r.idLesson2 == l.idLesson || r.idLesson2 == 0

                           join t in db.tbl_teachers
                           on r.idTeacher2 equals t.idTeacher
                           where r.idTeacher2 == t.idTeacher || r.idTeacher2 == 0
                           select new

                           {
                               r.End,
                               r.Start,
                               r.idBell,
                               r.idClass,
                               r.idDataTable,
                               r.idDay,
                               r.idLesson,
                               r.idLesson2,
                               r.idTeacher,
                               r.idTeacher2,
                               r.idYear,
                               r.isTak,
                               TakStatr = r.TakStatr,
                               TakEnd = r.TakEnd,
                               r.dayName,
                               r.BellName,
                               r.yearName,
                               LessonName = r.lessonName,
                               LessonName2 = l.lessonName,
                               r.teacheName,
                               t.FName,
                               t.LName,
                               r.className

                           }).ToList().Select(r => new
                           {
                               r.yearName,
                               r.className,
                               r.dayName,
                               r.BellName,
                               r.LessonName,
                               TeacheName = r.teacheName,
                               r.Start,
                               r.End,
                               r.isTak,
                               r.TakStatr,
                               r.TakEnd,
                               LessonName2 = r.LessonName2,
                               TeacheName2 = r.FName + " " + r.LName,
                               r.idBell,
                               r.idClass,
                               r.idDataTable,
                               r.idDay,
                               r.idLesson,
                               r.idLesson2,
                               r.idTeacher,
                               r.idTeacher2,
                               r.idYear,
                           })
                                 .ToList();

            return Result2.AsQueryable();
        }

        public IQueryable TecherDT(int Date, int idTeacher)
        {

            var Result = (from dt in db.tbl_dataTable
                          join days in db.tbl_days

                          on dt.idDay equals days.idDay
                          where dt.idTeacher == idTeacher || dt.idTeacher2 == idTeacher
                         

                          join c in db.tbl_classes
                          on dt.idClass equals c.idClass

                          join y in db.tbl_years
                          on dt.idYear equals y.idYear
                          where y.yearStart < Date & y.yearEnd > Date

                          join bell in db.tbl_bells
                          on dt.idBell equals bell.idBells

                          join l in db.tbl_lessons
                          on dt.idLesson equals l.idLesson

                          join t in db.tbl_teachers
                          on dt.idTeacher equals t.idTeacher


                          select new

                          {
                              dt.bellEnd,
                              dt.bellStart,
                              dt.idBell,
                              dt.idClass,
                              dt.idDataTable,
                              dt.idDay,
                              dt.idLesson,
                              dt.idLesson2,
                              dt.idTeacher,
                              dt.idTeacher2,
                              dt.idYear,
                              dt.isTak,
                              dt.takStart,
                              dt.takEnd,
                              days.dayName,
                              bell.BellName,
                              y.yearName,
                              l.lessonName,
                              t.FName,
                              t.LName,
                              c.className

                          }).ToList().Select(dt => new
                          {
                              End = dt.bellEnd.Hours + ":" + dt.bellEnd.Minutes,
                              Start = dt.bellStart.Hours + ":" + dt.bellStart.Minutes,
                              dt.idBell,
                              dt.idClass,
                              dt.idDataTable,
                              dt.idDay,
                              dt.idLesson,
                              dt.idLesson2,
                              dt.idTeacher,
                              dt.idTeacher2,
                              dt.idYear,
                              dt.isTak,
                              TakStatr = dt.takStart.Value.Hours + ":" + dt.takStart.Value.Minutes,
                              TakEnd = dt.takEnd.Value.Hours + ":" + dt.takEnd.Value.Minutes,
                              dt.dayName,
                              dt.BellName,
                              dt.yearName,
                              dt.lessonName,
                              teacheName = dt.FName + " " + dt.LName,
                              dt.className


                          })

                          .ToList();

            var Result2 = (from r in Result
                           join l in db.tbl_lessons
                           on r.idLesson2 equals l.idLesson
                           where r.idLesson2 == l.idLesson || r.idLesson2 == 0

                           join t in db.tbl_teachers
                           on r.idTeacher2 equals t.idTeacher
                           where r.idTeacher2 == t.idTeacher || r.idTeacher2 == 0
                           select new

                           {
                               r.End,
                               r.Start,
                               r.idBell,
                               r.idClass,
                               r.idDataTable,
                               r.idDay,
                               r.idLesson,
                               r.idLesson2,
                               r.idTeacher,
                               r.idTeacher2,
                               r.idYear,
                               r.isTak,
                               TakStatr = r.TakStatr,
                               TakEnd = r.TakEnd,
                               r.dayName,
                               r.BellName,
                               r.yearName,
                               LessonName = r.lessonName,
                               LessonName2 = l.lessonName,
                               r.teacheName,
                               t.FName,
                               t.LName,
                               r.className

                           }).ToList().Select(r => new
                           {
                               r.yearName,
                               r.className,
                               r.dayName,
                               r.BellName,
                               r.LessonName,
                               TeacheName = r.teacheName,
                               r.Start,
                               r.End,
                               r.isTak,
                               r.TakStatr,
                               r.TakEnd,
                               LessonName2 = r.LessonName2,
                               TeacheName2 = r.FName + " " + r.LName,
                               r.idBell,
                               r.idClass,
                               r.idDataTable,
                               r.idDay,
                               r.idLesson,
                               r.idLesson2,
                               r.idTeacher,
                               r.idTeacher2,
                               r.idYear,
                           })
                                 .ToList();

            return Result2.AsQueryable();
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

        ~DataTableRepository()
        {
            Dispose(false);
        }
    }
}