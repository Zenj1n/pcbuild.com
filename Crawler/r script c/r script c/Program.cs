using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace r_script_c
{
    class Program
    {
        static void Main(string[] args)
        {
            string component = "Chieftec Elox BT-04B-U3, behuizing Zwart, 250 Watt";   
            string strCmdText;
            strCmdText = "/C Rscript prijshistory.r " + component.Replace(" ", "_");
            System.Diagnostics.Process.Start("CMD.exe",strCmdText);


        }
    }
}

//Rscript E:/Repositories Git Hub/pcbuild.com/Crawler/afbeeldingen/prijshistory.r Antec_Minuet_350W,_behuizing_Zwart_(Hoogglans)/Zilver