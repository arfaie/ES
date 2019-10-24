using E_School.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.Student
{
    public class StudentAvrage
    {
        public List<string> DescriptiveScores { get; set; }
        public List<double> NumeralScores { get; set; }
        public string Avrage { get; set; }
    }
}