using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models
{
    public class Processor
    {
        public int Processor_ID { get; set; }
        public string Processor_Name { get; set; }
        public string Processor_Desc { get; set; }
        public string Processor_URL { get; set; }
        public decimal Processor_Price { get; set; }
        public string Processor_Webshop { get; set; }
    }
}