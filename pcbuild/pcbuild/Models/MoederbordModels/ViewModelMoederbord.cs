using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models.MoederbordModels
{
    public class ViewModelMoederbord
    {
        public Moederbord_Model Moederbord_all { get; set; }
        public Verkrijgbaar_Model Verkrijgbaar_all { get; set; }
        public Webshop_Model Webshop_all { get; set; }
    }
}