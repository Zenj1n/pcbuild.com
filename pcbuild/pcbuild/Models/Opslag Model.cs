using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pcbuild.Models.HardDiskModels;
using pcbuild.Models.SSDModels;

namespace pcbuild.Models
{
    public class Opslag_Model
    {
        public ViewModelSSD SSD_V_ALL { get; set; }
        public ViewModelHardDisk HD_V_ALL { get; set; }
    }
}