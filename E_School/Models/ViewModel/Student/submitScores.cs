using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class submitScores
    {
        public int idScore { get; set; }
        public int idStudent { get; set; }
        public int idExam { get; set; }
        public float score { get; set; } = -1;
        public int idDescriptiveScore { get; set; }
        public String studentName { get; set; }
        public String desScore { get; set; }
    }
}