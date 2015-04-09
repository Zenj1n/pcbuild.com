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
    public partial class Install_python : Form
    {
        public Install_python()
        {
            InitializeComponent();

            // kopieer scripts van relatief pad naar absolute pad
            string copy = "/C copy ..\\..\\Res\\script1.bat C:\\PCBuild.nl\\";
            System.Diagnostics.Process.Start("CMD.exe", copy);
            string copy2 = "/C copy ..\\..\\Res\\script2.bat C:\\PCBuild.nl\\";
            System.Diagnostics.Process.Start("CMD.exe", copy2);
            string copy3 = "/C copy ..\\..\\Res\\easy_install.bat C:\\PCBuild.nl\\";
            System.Diagnostics.Process.Start("CMD.exe", copy3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\python.msi");
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // run script
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\script1.bat");
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
            // run script
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\script2.bat");

            // open folder PCBuild.nl
            Process.Start(@"c:\PCBuild.nl\");

            button3.Enabled = false;
        }
    }
}
