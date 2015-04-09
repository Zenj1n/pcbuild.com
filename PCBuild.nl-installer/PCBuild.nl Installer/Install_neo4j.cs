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
    public partial class Install_neo4j : Form
    {
        public Install_neo4j()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\Neo4j.exe");
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\script1.bat");
            //string window = @"%windir%\System32\rundll32.exe sysdm.cpl,EditEnvironmentVariables";
            //System.Diagnostics.Process.Start(window);
            System.Diagnostics.Process.Start("sysdm.cpl");
            button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\PCBuild.nl\script2.bat");
            button3.Enabled = false;
        }
    }
}
