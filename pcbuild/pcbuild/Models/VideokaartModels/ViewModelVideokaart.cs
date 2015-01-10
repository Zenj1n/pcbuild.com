using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models.VideokaartModels
{
    public class ViewModelVideokaart
    {
        public Videokaart_Model Videokaart_all { get; set; }
        public Verkrijgbaar_Model Verkrijgbaar_all { get; set; }
        public Webshop_Model Webshop_all { get; set; }
    }
}