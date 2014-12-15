using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models
{
    public class Videokaart
    {
        public int Videokaart_ID { get; set; }
        public string Videokaart_Name { get; set; }
        public string Videokaart_Desc { get; set; }
        public string Videokaart_URL { get; set; }
        public decimal Videokaart_Price { get; set; }
        public string Videokaart_Webshop { get; set; }
    }
}