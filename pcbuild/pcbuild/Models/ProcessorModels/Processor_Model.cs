using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pcbuild.Models.ProcessorModels
{
    public class Processor_Model
    {
        public string naam { get; set; }
        public string kloksnelheid { get; set; }
        public string socket { get; set; }
        public string kernen { get; set; }
    }
}