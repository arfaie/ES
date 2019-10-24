using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Models.ViewModel
{
    public class LsDebt
    {
        public int idDebt { get; set; }
        public int idStud { get; set; }
        public int idYear { get; set; }
        public int[] idCost { get; set; }
        public int idRegCourse { get; set; }
        public int Date { get; set; }

    }
}