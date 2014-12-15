using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models
{
    public class Werkgeheugen
    {
        public int Werkgeheugen_ID { get; set; }
        public string Werkgeheugen_Name { get; set; }
        public string Werkgeheugen_Desc { get; set; }
        public string Werkgeheugen_URL { get; set; }
        public decimal Werkgeheugen_Price { get; set; }
        public string Werkgeheugen_Webshop { get; set; }
    }
}