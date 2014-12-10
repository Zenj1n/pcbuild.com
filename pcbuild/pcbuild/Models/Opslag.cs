using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models
{
    public class Opslag
    {
        public int Opslag_ID { get; set; }
        public string Opslag_Name { get; set; }
        public string Opslag_Desc { get; set; }
        public string Opslag_URL { get; set; }
        public decimal Opslag_Price { get; set; }
        public string Opslag_Webshop { get; set; }
    }
}