using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WANTED.Deserializar
{
    public class notices
    {
        public string forename { get; set; }
        public string date_of_birth { get; set; }
        public string entity_id { get; set; }
        //public List<string> nationalities { get; set; }
        public string name { get; set; }
        public List<_linksInterno> _links { get; set; }
    }
}