using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_School.Helpers.Utitlies
{
    public class LsTerms
    {
        public int idTerm { get; set; } 
        public int idYear { get; set; }
        public string yearName { get; set; }
        public string termName { get; set; }
        public string termStart { get; set; }
        public string termEnd { get; set; }
        public string des { get; set; }
    }
}