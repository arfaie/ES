//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_School.Models.DomainModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_lessons
    {
        public int idLesson { get; set; }
        public string lessonName { get; set; }
        public int idLevel { get; set; }
        public int idMajor { get; set; }
        public int idLessonType { get; set; }
        public Nullable<int> theoryUnitCounts { get; set; }
        public Nullable<int> practicalUnitCounts { get; set; }
        public string des { get; set; }
    }
}
