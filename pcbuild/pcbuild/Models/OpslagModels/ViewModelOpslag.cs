using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models.OpslagModels
{
    public class ViewModelOpslag
    {
        public Opslag_Model Opslag_m{ get; set; }
        public Verkrijgbaar_Model Verkrijgbaar_m { get; set; }
        public Webshop_Model Webshop_m { get; set; }
    }
}