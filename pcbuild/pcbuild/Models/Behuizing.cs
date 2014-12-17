using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models
{
    public class Behuizing
    {
        public int Behuizing_ID { get; set; }
        public string Behuizing_Name { get; set; }
        public string Behuizing_Desc { get; set; }
        public string Behuizing_URL { get; set; }
        public decimal Behuizing_Price { get; set; }
        public string Behuizing_Webshop { get; set; }
    }
}