using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models.HardDiskModels
{
    public class ViewModelHardDisk
    {
        public Harddisk_Model Harddisk_all { get; set; }
        public SSDModel SSD_all { get; set; }
        public Verkrijgbaar_Model Verkrijgbaar_all { get; set; }
        public Webshop_Model Webshop_all { get; set; }
    }
}