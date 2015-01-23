using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Dashboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Laden";
            chart1.Series["Componenten"].Points.Clear();

            try
            {

                // connect met de database
                var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
                client.Connect();

                //tel processors
                var processor_query = client
                .Cypher
                .Match("(n:" + "processor" + ")")
                .Return(n => new
                {
                    Aantal = (int)n.Count()
                })
               .Results
               .ToArray();
                int processor_aantal = processor_query[0].Aantal;

                label11.Text = processor_aantal.ToString();
                chart1.Series["Componenten"].Points.AddXY("Processor", processor_aantal);

                //tel moederborden
                var moederbord_query = client
                .Cypher
                .Match("(n:" + "moederbord" + ")")
                .Return(n => new
                {
                    Aantal = (int)n.Count()
                })
               .Results
               .ToArray();
                int moederbord_aantal = moederbord_query[0].Aantal;
                chart1.Series["Componenten"].Points.AddXY("Moederbord", moederbord_aantal);

                label12.Text = moederbord_aantal.ToString();

                //tel videokaarten
                var videokaart_query = client
                .Cypher
                .Match("(n:" + "videokaart" + ")")
                .Return(n => new
                {
                    Aantal = (int)n.Count()
                })
               .Results
               .ToArray();
                int videokaart_aantal = videokaart_query[0].Aantal;

                label13.Text = videokaart_aantal.ToString();
                chart1.Series["Componenten"].Points.AddXY("Videokaart", videokaart_aantal);

                //tel werkgeheugen
                var werkgeheugen_query = client
                .Cypher
                .Match("(n:" + "werkgeheugen" + ")")
                .Return(n => new
                {
                    Aantal = (int)n.Count()
                })
               .Results
               .ToArray();
                int werkgeheugen_aantal = werkgeheugen_query[0].Aantal;

                label14.Text = werkgeheugen_aantal.ToString();
                chart1.Series["Componenten"].Points.AddXY("Werkgeheugen", werkgeheugen_aantal);

                //tel behuizingen
                var behuizing_query = client
                .Cypher
                .Match("(n:" + "behuizing" + ")")
                .Return(n => new
                {
                    Aantal = (int)n.Count()
                })
               .Results
               .ToArray();
                int behuizing_aantal = behuizing_query[0].Aantal;
                chart1.Series["Componenten"].Points.AddXY("Behuizing", behuizing_aantal);

                label15.Text = behuizing_aantal.ToString();

                //tel opslag
                var opslag_query = client
                .Cypher
                .Match("(n:" + "opslag" + ")")
                .Return(n => new
                {
                    Aantal = (int)n.Count()
                })
               .Results
               .ToArray();
                int opslag_aantal = opslag_query[0].Aantal;

                label16.Text = opslag_aantal.ToString();
                chart1.Series["Componenten"].Points.AddXY("Opslag", opslag_aantal);

                //tel voeding
                var voeding_query = client
                .Cypher
                .Match("(n:" + "voeding" + ")")
                .Return(n => new
                {
                    Aantal = (int)n.Count()
                })
               .Results
               .ToArray();
                int voeding_aantal = voeding_query[0].Aantal;

                label17.Text = voeding_aantal.ToString();

                int count = processor_aantal + moederbord_aantal + videokaart_aantal + werkgeheugen_aantal + behuizing_aantal + opslag_aantal + voeding_aantal;
                label23.Text = count.ToString();
                chart1.Series["Componenten"].Points.AddXY("Voeding", voeding_aantal);

            }
            catch (Exception a)
            {

                label11.Text = "0";
                label12.Text = "0";
                label13.Text = "0";
                label14.Text = "0";
                label15.Text = "0";
                label16.Text = "0";
                label17.Text = "0";

                label23.Text = "0";
            }

            try
            {
                // connect met de database
                var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data"));
                client.Connect();

                label20.ForeColor = System.Drawing.Color.Green;
                label20.Text = "Online";

            }catch(Exception b){

                label20.ForeColor = System.Drawing.Color.Red;
                label20.Text = "Offline";
            }

            button1.Text = "Refresh";
            label21.Text = DateTime.Now.ToString("G");

        }
    }
}
