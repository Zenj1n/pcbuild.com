using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PCBuild.nl_Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (!Directory.Exists(@"C:\PCBuild.nl\"))
                Directory.CreateDirectory(@"C:\PCBuild.nl\");
        }

        /// <summary>
        /// opent form Download_neo4j
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Download_neo4j f1 = new Download_neo4j();
            f1.Show();
            button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Download_python f2 = new Download_python();
            f2.Show();
            button4.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Download_rstudio f3 = new Download_rstudio();
            f3.Show();
            button6.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Tijdelijke bestanden verwijderen?" + "\n" + "\n" + "Pad naar tijdelijke bestanden: C:\\PCBuild.nl\\",
                       "PCBuild.nl - Opruimer",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
            {
                // Delete the folder 'c:\Projects' and all of its content
                Directory.Delete(@"C:\PCBuild.nl\", true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Install_neo4j f4 = new Install_neo4j();
            f4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Install_python f5 = new Install_python();
            f5.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Install_rstudio f6 = new Install_rstudio();
            f6.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.pcbuild.nl");
        }
    }
}
