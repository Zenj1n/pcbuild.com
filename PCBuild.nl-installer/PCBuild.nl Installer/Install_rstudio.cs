using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace PCBuild.nl_Installer
{
    public partial class Install_rstudio : Form
    {
        public Install_rstudio()
        {
            InitializeComponent();

            // kopieer script van relatief pad naar absolute pad
            string copy = "/C copy ..\\..\\Res\\script3.bat C:\\PCBuild.nl\\";
            System.Diagnostics.Process.Start("CMD.exe", copy);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\R.exe");
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // run script
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\script3.bat");
            //string window = @"%windir%\System32\rundll32.exe sysdm.cpl,EditEnvironmentVariables";
            //System.Diagnostics.Process.Start(window);

            // sleep voor 100ms
            System.Threading.Thread.Sleep(100);

            // open system settings
            //System.Diagnostics.Process.Start("sysdm.cpl");
            string copy = "/C rundll32 sysdm.cpl,EditEnvironmentVariables";
            System.Diagnostics.Process.Start("CMD.exe", copy);

            button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\script2.bat");
            button3.Enabled = false;
        }
    }
}
