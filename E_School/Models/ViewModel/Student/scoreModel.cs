using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel.api
{
    public class scoreModel
    {
        public int idExam { set; get; }

        public int date { set; get; }
        public int idDescriptiveScore { set; get; }
        public int[] idStudent { set; get; }
        public float[] score { set; get; }
        public int[] scoreId { set; get; }
        
    }
}