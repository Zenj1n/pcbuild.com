using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models
{
    public class Videokaart
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public decimal Price { get; set; }
    }

    public class Processors
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public decimal Price { get; set; }
    }

    public class Behuizing
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public decimal Price { get; set; }
    }

    public class Werkgeheugen
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public decimal Price { get; set; }
    }

    public class Opslag
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public decimal Price { get; set; }
    }
}