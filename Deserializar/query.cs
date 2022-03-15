using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WANTED.Deserializar
{
    public class query
    {
        public int page { get; set; }
        public int resultPerPage { get; set; }
        public string nationality { get; set; }
        public int ageMin { get; set; }
        public int ageMax { get; set; }
    
    }
}