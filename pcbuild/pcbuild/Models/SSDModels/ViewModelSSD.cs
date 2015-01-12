using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models.SSDModels
{
    public class ViewModelSSD
    {
        public SSD_Model SSD_all { get; set; }
        public Verkrijgbaar_Model Verkrijgbaar_all { get; set; }
        public Webshop_Model Webshop_all { get; set; }
    }
}